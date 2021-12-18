using static Util;
using System.Linq;
using System.Collections;

class Day15
{
    /* 
     * Thanks a lot to https://github.com/jarmovanlenthe/aoc2021/blob/master/Challenges/Days/Day15.cs and
     * https://github.com/encse/adventofcode/blob/master/2021/Day15/Solution.cs
     * My solution is roughly a copy of what they did, but - just as for jarmovanlenthe - taught me a lot:
     * - Record types
     * - PriorityQueue
     * - with statements
     */
    public record Node(int x, int y);

    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/15.txt");
        var input = CreateNodes(lines, exercise).ToList();

        var nodes = input.Select(x => x.Item1);
        var riskDictionary = input.ToDictionary(x => x.Item1, x => x.Item2);

        var maxX = nodes.Max(n => n.x);
        var maxY = nodes.Max(n => n.y);

        var start = nodes.First();
        var stop = nodes.Last();

        var exploredSpace = Solve(start, stop, maxX, maxY, riskDictionary);

        System.Console.WriteLine($"{exploredSpace[stop]}");
    }

    private static Dictionary<Node, int> Solve(Node start, Node stop, int maxX, int maxY, Dictionary<Node, int> riskDictionary)
    {
        var exploredSpace = new Dictionary<Node, int>();
        exploredSpace.Add(start, 0);

        var priorities = new PriorityQueue<Node, int>();
        priorities.Enqueue(start, 0);

        var done = false;
        while (!done && priorities.TryDequeue(out var node, out var cost))
        {
            var neighbors = GetNeighbors(node, maxX, maxY);

            foreach (Node neighbor in neighbors)
            {
                if (!exploredSpace.TryGetValue(neighbor, out var _))
                {
                    var risk = exploredSpace[node] + riskDictionary[neighbor];
                    exploredSpace[neighbor] = risk;
                    priorities.Enqueue(neighbor, risk);
                }

                if (neighbor == stop)
                {
                    done = true;
                    break;
                }
            }
        }

        return exploredSpace;
    }

    private static IEnumerable<Node> GetNeighbors(Node node, int maxX, int maxY) =>
        new[]
        {
            node with { y = node.y + 1 },
            node with { y = node.y - 1 },
            node with { x = node.x + 1 },
            node with { x = node.x - 1 },
        }.Where(n => n.x >= 0 && n.y >= 0 && n.x <= maxX && n.y <= maxY);

    private static IEnumerable<(Node, int)> GetDimensionExpansion(Node node, int cost, int maxX, int maxY)
    {
        var range = Enumerable.Range(0, 5);
        var dims =
            from x in range
            from y in range
            where x != 0 || y != 0
            select (x, y);

        foreach (var dim in dims)
        {
            var newCost = (cost + dim.x + dim.y) % 9;
            yield return (node with { x = node.x + dim.x * maxX, y = node.y + dim.y * maxY}, newCost == 0 ? 9 : newCost);
        }
    }
        

    private static IEnumerable<(Node, int)> CreateNodes(string[] lines, int exercise)
    {
        int width = lines.First().Length;
        int height = lines.Count();
        int y = 0;
        foreach (var line in lines)
        {
            int x = 0;
            foreach (var c in line)
            {
                var ret = new Node(x, y);
                var cost = Int32.Parse(c.ToString());
                x++;
                yield return (ret, cost);
                if (exercise == 2)
                {
                    foreach (var extra in GetDimensionExpansion(ret, cost, width, height))
                    {
                        yield return extra;
                    }
                }
            }
            y++;
        }
    }

    /*
    public class Node
    {
        public int? MinCostToStart;
        public int Cost;
        public bool Visited;
        public Node? Parent;

        public int x;
        public int y;

        public override bool Equals(Object obj)
        {
            if (!(obj is Node)) return false;

            Node n = (Node)obj;
            return x == n.x & y == n.y;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
    */
}