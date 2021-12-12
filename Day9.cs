using System;
using static Util;

class Day9
{
    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/9.txt");
        var width = lines.First().Length;
        var height = lines.Length;

        var map = new int[width, height];

        FillMapAndGlobalMax(lines, width, height, map, out var globalMax);

        var localMinima = new List<(int val, int x, int y, int size)>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cur = map[x, y];
                var isLocalMinimum = cur < (new[]
                {
                    x > 0 ? map[x - 1, y] : globalMax,
                    y > 0 ? map[x, y - 1] : globalMax,
                    x < width - 1 ? map[x + 1, y] : globalMax,
                    y < height - 1 ? map[x, y + 1] : globalMax
                }).Min();

                if (isLocalMinimum)
                {
                    localMinima.Add((cur, x, y, 1));
                }
            }
        }

        var basinScores = new List<int>();

        foreach (var localMinimum in localMinima)
        {
            var positions = new HashSet<(int, int)>
            {
                {(localMinimum.x, localMinimum.y)}
            };

            while (TryExpandLocalMinimum(map, positions))
            {
                // Do nothing, just keep expanding
            }

            basinScores.Add(positions.Count);
        }

        var basinScore = basinScores.OrderByDescending(x => x).Take(3).Aggregate(1, (x, y) => x * y);
        System.Console.Write(exercise == 1 ? localMinima.Sum(item => item.val) + localMinima.Count : basinScore);
    }

    private static bool TryExpandLocalMinimum(int[,] map, HashSet<(int, int)> positions)
    {
        var candidateNeighbours = FindCandidateNeighbours(map, positions);
        var ret = !candidateNeighbours.IsSubsetOf(positions);
        positions.UnionWith(candidateNeighbours);
        return ret;
    }

    private static HashSet<(int, int)> FindCandidateNeighbours(int[,] map, HashSet<(int x, int y)> positions)
    {
        var width = map.GetLength(0);
        var height = map.GetLength(1);
        var ret = new HashSet<(int, int)>();

        foreach ((var x, var y) in positions)
        {
            var cur = map[x, y];
            var positionsToCompare = new[]
            {
                x > 0 ? (x-1, y, map[x - 1, y]) : (x-1, y, 0),
                y > 0 ? (x, y-1, map[x, y - 1]) : (x, y-1, 0),
                x < width - 1 ? (x+1, y, map[x + 1, y]) : (x+1, y, 0),
                y < height - 1 ? (x, y+1, map[x, y + 1]) : (x, y+1, 0)
            };

            foreach ((var x_, var y_, var val) in positionsToCompare)
            {
                if (val > cur && val != 9)
                {
                    ret.Add((x_, y_));
                }
            }
        }

        return ret;
    }

    private static void FillMapAndGlobalMax(string[] lines, int width, int height, int[,] map, out int globalMax)
    {
        globalMax = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var val = Int32.Parse(lines[y][x].ToString());
                map[x, y] = val;
                globalMax = Math.Max(globalMax, val);
            }
        }
    }
}
            