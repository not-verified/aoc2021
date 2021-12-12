using System;
using static Util;

class Day11
{

    public static void Run(int exercise = 1)
    {
        int _ROUNDS = exercise == 1 ? 100 : 9999999;
        var lines = ReadLines("../../../input/11.txt");
        var width = lines.First().Length;
        var height = lines.Length;
        var mapSize = width * height;

        var map = new (int val, bool hasFlashed)[width, height];

        FillMap(lines, width, height, map);

        var totalFlashes = 0;
        int firstFullRound = -1;

        for (int x = 1; x <= _ROUNDS; x++)
        {
            var prevTotalFlashes = totalFlashes; // for exercise 2

            StepOne(map); // Increase all by 1
            StepTwo(map, ref totalFlashes); // Flash
            StepThree(map); // Reset flashed octopuses

            if (totalFlashes - prevTotalFlashes == mapSize)// for exercise 2
            {
                firstFullRound = x;
                break;
            }
        }

        
        System.Console.Write(exercise == 1 ? totalFlashes : firstFullRound);
    }

    private static void FillMap(string[] lines, int width, int height, (int val, bool hasFlashed)[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var val = Int32.Parse(lines[y][x].ToString());
                map[x, y] = (val, false);
            }
        }
    }

    private static void StepOne((int val, bool hasFlashed)[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = (map[x, y].val + 1, false);
            }
        }
    }

    private static void StepTwo((int, bool)[,] map, ref int totalFlashes)
    {
        while (TryGetAboutToFlash(map, out var aboutToFlash))
        {
            totalFlashes += aboutToFlash.Count;
            Flash(map, aboutToFlash);
        }

    }

    private static void Flash((int val, bool hasFlashed)[,] map, List<(int x, int y)> aboutToFlash)
    {
        foreach (var octoPosition in aboutToFlash)
        {
            (var x, var y) = (octoPosition.x, octoPosition.y);
            map[x, y] = (map[x, y].val, true); // Set hasFlashed to true

            var positionsToUpdate = new List<(int, int)>
            {
                {(x-1, y-1)},
                {(x-1, y  )},
                {(x-1, y+1)},
                {(x  , y-1)},
                {(x  , y+1)},
                {(x+1, y-1)},
                {(x+1, y  )},
                {(x+1, y+1)}
            }.Where(tup => tup.Item1 >= 0  && tup.Item1 < map.GetLength(0) && tup.Item2 >= 0 && tup.Item2 < map.GetLength(1));

            foreach(var positionToUpdate in positionsToUpdate)
            {
                (var UpdX, var UpdY) = (positionToUpdate.Item1, positionToUpdate.Item2);
                var current = map[UpdX, UpdY];
                map[UpdX, UpdY] = (current.val + 1, current.hasFlashed);
            }

        }
    }

    private static bool TryGetAboutToFlash((int val, bool hasFlashed)[,] map, out List<(int, int)> aboutToFlash)
    {
        aboutToFlash = new List<(int, int)>();
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y].val > 9 && !map[x, y].hasFlashed)
                {
                    aboutToFlash.Add((x, y));
                }
            }
        }
        return aboutToFlash.Any();
    }

    private static void StepThree((int val, bool hasFlashed)[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y].hasFlashed)
                {
                    map[x, y] = (0, false);
                }
            }
        }
    }
}
