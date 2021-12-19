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

    public static string HexStringToBinary(string hexString)
    {
        var mapping = new Dictionary<char, string>{
            { '0', "0000"},
            { '1', "0001"},
            { '2', "0010"},
            { '3', "0011"},

            { '4', "0100"},
            { '5', "0101"},
            { '6', "0110"},
            { '7', "0111"},

            { '8', "1000"},
            { '9', "1001"},
            { 'A', "1010"},
            { 'B', "1011"},

            { 'C', "1100"},
            { 'D', "1101"},
            { 'E', "1110"},
            { 'F', "1111"}};

        var ret = string.Join("", from character in hexString
                                  select mapping[character]);
        return ret;
    }

    public class Graph<T>
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

    public class Route<T>
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

        public bool isComplete => currentRoute.Last().ToString() == "_end";

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

public static class QueueExtensions
{
    public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
    {
        for (int i = 0; i < chunkSize && queue.Count > 0; i++)
        {
            yield return queue.Dequeue();
        }
    }
}