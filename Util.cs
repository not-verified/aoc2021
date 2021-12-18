using System;

class Util
{
    public static string[] ReadLines(string file)
    {
        var ret = new List<string>();

        foreach (string line in System.IO.File.ReadLines(file))
        {
            ret.Add(line);
        }

        return ret.ToArray();
    }

    private class Graph<T>
    {
        // KoderDojo graph class
        public Graph() { }
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }
    }

    private class Route<T>
    {
        public Route() { }
        public Route(T start)
        {
            AddToRoute(start);
        }

        public Route(List<T> c, HashSet<T> v, T h, bool j = false)
        {
            currentRoute = c;
            visitedVertices = v;
            head = h;
            hasUsedSmallRoomJoker = j;
        }

        public List<T> currentRoute = new List<T>();

        public HashSet<T> visitedVertices = new HashSet<T>();

        public bool isComplete => currentRoute.Last().ToString() == _END;

        public bool hasReturned = false;

        public bool hasUsedSmallRoomJoker = false;

        public T head;

        public void AddToRoute(T vertex)
        {
            currentRoute.Add(vertex);
            visitedVertices.Add(vertex);
            head = vertex;
        }

        public bool Contains(T element)
        {
            return visitedVertices.Contains(element);
        }
    }
}