using System;
using static Util;

class Day5
{
    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/5.txt");
        var map = new Dictionary<(int, int), int>();
        foreach (var line in lines)
        {
            var points = GeneratePoints(line, exercise);
            foreach (var point in points)
            {
                map[point] = map.TryGetValue(point, out int old) ? old + 1 : 1;
            }
        }
        var res = map.Values.Where(x => x >= 2).Count();
        System.Console.Write(res);

    }

    public static IEnumerable<(int, int)> GeneratePoints(string line, int exercise)
    {
        var parts0 = line.Split(new String[] { " -> " }, StringSplitOptions.None);
        var parts = parts0.SelectMany(x => x.Split(',')).Select(y => Int32.Parse(y)).ToArray();
        if (parts.Length == 4)
        {
            (var x1, var y1, var x2, var y2) = (parts[0], parts[1], parts[2], parts[3]);
            if (x1 == x2)
            {
                foreach (int y in Enumerable.Range(Math.Min(y1, y2), Math.Abs(y2 - y1) + 1))
                {
                    yield return (x1, y);
                }
            }
            else if (y1 == y2)
            {
                foreach (int x in Enumerable.Range(Math.Min(x1, x2), Math.Abs(x2 - x1) + 1))
                {
                    yield return (x, y1);
                }
            }
            else if (exercise == 2)
            {
                var xs = x1 > x2 ? Enumerable.Range(x2, Math.Abs(x1 - x2) + 1).Reverse() : Enumerable.Range(x1, Math.Abs(x1 - x2) + 1);
                var ys = y1 > y2 ? Enumerable.Range(y2, Math.Abs(y1 - y2) + 1).Reverse() : Enumerable.Range(y1, Math.Abs(y1 - y2) + 1);
                foreach ((var x, var y) in xs.Zip(ys))
                {
                    yield return (x, y);
                }
            }

        }
        else
        {
            throw new Exception("AAARGH!");
        }
    }
}