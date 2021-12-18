using System;
using static Util;

class Day12
{
    private const string _START = "start";
    private const string _END = "end";

    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/12.txt");
        var vertices = lines.SelectMany(elem => elem.Split("-"));
        var edges = lines.Select(elem => elem.Split("-")).Select(x => new Tuple<string, string>(x.First(), x.Last()));
        var graph = new Graph<string>(vertices, edges);
        var routes = ExhaustiveBFS<string>(graph, _START, _END, exercise);
        var dbg = routes.Select(r => r.currentRoute).ToList();
        Console.Write(routes.Count());
    }

    private static IEnumerable<Route<T>> ExhaustiveBFS<T>(Graph<T> graph, T start, T end, int exercise)
    {
        var routes = new List<Route<T>>();
        routes.Add(new Route<T>(start));
        

        var canExpand = true;
        while (canExpand)
        {
            if (!TryExpand(routes, graph, out var newRoutes, exercise))
            {
                canExpand = false;
            }

            foreach (var completedRoute in newRoutes.Where(r => r.isComplete && !r.hasReturned))
            {
                completedRoute.hasReturned = true; // Only want to return this route once
                yield return completedRoute;
            }

            routes = newRoutes;
        }
    }

    private static bool TryExpand<T>(List<Route<T>> routes, Graph<T> graph, out List<Route<T>> newRoutes, int exercise)
    {
        var didExpand = false;
        newRoutes = new List<Route<T>>();

        foreach(var route in routes)
        {
            if (!route.isComplete && graph.AdjacencyList.TryGetValue(route.head, out var neighbors))
            {
                // For each not visited neighbour + all uppercase neighbours
                var candidates = neighbors.Except(route.visitedVertices)
                                          .Union(neighbors.Where(s => s.ToString().All(c => Char.IsUpper(c))));

                if (exercise == 2 && !route.hasUsedSmallRoomJoker)
                {
                    candidates = neighbors.Except(new HashSet<T>() { (T)Convert.ChangeType(_START, typeof(T)) });
                }

                foreach (var vertex in candidates)
                {
                    var newCurrentRoute = route.currentRoute.Append<T>(vertex).ToList();
                    var newHashset = route.visitedVertices.Union(new HashSet<T> { vertex }).ToHashSet();
                    var routeToAdd = new Route<T>(newCurrentRoute, newHashset, vertex, route.hasUsedSmallRoomJoker);

                    if (exercise == 2 && routeToAdd.visitedVertices.Count == route.visitedVertices.Count && vertex.ToString().All(c => Char.IsLower(c))) // We've used our joker, apparently
                    {
                        routeToAdd.hasUsedSmallRoomJoker = true;
                    }

                    newRoutes.Add(routeToAdd);
                    didExpand = true;
                }
            }
        }

        return didExpand;
    }
}