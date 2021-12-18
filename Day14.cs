using System;
using static Util;

class Day14
{
    public static void Run(int exercise = 1)
    {
        var rounds = exercise == 1 ? 10 : 40;
        var lines = ReadLines("../../../input/14.txt");

        var template = lines.First();
        var currentPairs = template.Zip(template.Skip(1), Tuple.Create).GroupBy(x => x).ToDictionary(item => item.Key, item => (long)item.Count());
        var currentCounts = template.GroupBy(c => c).ToDictionary(g => g.Key, g => (long)g.Count());

        var rules = lines.Skip(2).Select(line => line.Split(" -> ")).ToDictionary(t => new Tuple<char, char>(t[0][0], t[0][1]), t => t[1].First());

        for (int _ = 0; _ < rounds; _++)
        {
            var newPairs = new Dictionary<Tuple<char, char>, long>();

            foreach((var pair, var count) in currentPairs)
            {
                if (rules.TryGetValue(pair, out char insert))
                {
                    newPairs[new Tuple<char, char>(pair.Item1, insert)] = newPairs.TryGetValue(new Tuple<char, char>(pair.Item1, insert), out var cur) ? cur + count: count;
                    newPairs[new Tuple<char, char>(insert, pair.Item2)] = newPairs.TryGetValue(new Tuple<char, char>(insert, pair.Item2), out cur) ? cur + count : count;
                    currentCounts[insert] = currentCounts.TryGetValue(insert, out cur) ? cur + count : count;
                }
                else
                {
                    newPairs[pair] = count;
                }
            }
            currentPairs = newPairs;
        }

        Console.Write($"{currentCounts.Max(x => x.Value) - currentCounts.Min(x => x.Value)}");
    }
}