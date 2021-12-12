using System;
using static Util;

class Day3
{
    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/3.txt");
        var oxygenRatingCriteriumValue = FindRatingCriterium(lines, oxygenRatingCriterium);
        var co2RatingCriteriumValue = FindRatingCriterium(lines, co2RatingCriterium);

        System.Console.Write(Convert.ToInt32(oxygenRatingCriteriumValue, 2) * Convert.ToInt32(co2RatingCriteriumValue, 2));
    }

    private static string FindRatingCriterium(string[] lines, Func<(string[], int), string[]> criterium)
    {
        int pos = 0;

        while (lines.Length > 1)
        {
            lines = criterium.Invoke((lines, pos));
            pos++;
        }

        return lines[0];
    }

    private static string[] oxygenRatingCriterium((string[], int) arg)
    {
        (var counts, var zeroIdx) = GetCountsAndZeroIdx(arg);
     
        if (counts[0] > counts[1]) // keep if in zeroIdx
        {
            return arg.Item1
                .Select((value, idx) => new { Value = value, Index = idx })
                .Where(item => zeroIdx.Contains(item.Index))
                .Select(item => item.Value)
                .ToArray();
        }
        return arg.Item1
            .Select((value, idx) => new { Value = value, Index = idx })
            .Where(item => !zeroIdx.Contains(item.Index))
            .Select(item => item.Value)
            .ToArray();
    }

    private static string[] co2RatingCriterium((string[], int) arg)
    {
        (var counts, var zeroIdx) = GetCountsAndZeroIdx(arg);

        if (counts[1] < counts[0])
        {
            return arg.Item1
                .Select((value, idx) => new { Value = value, Index = idx })
                .Where(item => !zeroIdx.Contains(item.Index))
                .Select(item => item.Value)
                .ToArray();
        }
        return arg.Item1
            .Select((value, idx) => new { Value = value, Index = idx })
            .Where(item => zeroIdx.Contains(item.Index))
            .Select(item => item.Value)
            .ToArray();
    }

    private static (Dictionary<int, int>, List<int>) GetCountsAndZeroIdx((string[], int) arg)
    {
        var counts = new Dictionary<int, int>();
        var zeroIdx = new List<int>();
        var pos = 0;

        foreach (var line in arg.Item1)
        {
            var consideredBit = line[arg.Item2];
            if (consideredBit == '0')
            {
                counts[0] = counts.GetValueOrDefault(0) + 1;
                zeroIdx.Add(pos);
            }
            else
            {
                counts[1] = counts.GetValueOrDefault(1) + 1;
            }
            pos++;
        }

        return (counts, zeroIdx);
    }
}