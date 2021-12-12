using System;
using static Util;

class Day6
{
    private const int _DAYS = 256;

    public static void Run(int exercise = 1)
    {
        var population = ReadLines("../../../input/6.txt").First().Split(',')
            .Select(s => long.Parse(s))
            .GroupBy(i => i)
            .ToDictionary(group => group.Key, group => (long)group.Count());

        long grandTotal = 0;

        if (exercise == 1)
        {
            foreach (var kvp in population)
            {
                var totalCountPerFish = GetTotalCountFromDays(kvp.Key, _DAYS);
                grandTotal += totalCountPerFish * kvp.Value;
            }

            System.Console.Write(grandTotal);
        }
        else
        {
            for (int i = 0; i < _DAYS; i++)
            {
                long spawnCount = population.TryGetValue(0, out long x) ? x : 0;
                for (int j = 1; j <= 8; j++)
                {
                    population[j - 1] = population.TryGetValue(j, out long z) ? z : 0;
                }
                population[8] = spawnCount;
                population[6] += spawnCount; // spawnCount is also resetCount
            }

            grandTotal = population.Select(kvp => kvp.Value).Sum();
            System.Console.Write(grandTotal);
        }
    }

    private static int GetTotalCountFromDays(long key, int days)
    {
        days -= 1;
        if (days < 0)
        {
            return 1;
        }
        else
        {
            if (key == 0)
            {
                return GetTotalCountFromDays(6, days) + GetTotalCountFromDays(8, days);
            }
            else
            {
                return GetTotalCountFromDays(key - 1, days);
            }
        }
    }
}