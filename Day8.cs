using System;
using static Util;

class Day8
{
    private static Dictionary<HashSet<char>, char> hashsetCharMapping = new Dictionary<HashSet<char>, char>(HashSet<char>.CreateSetComparer())
    {
        { new HashSet<char>(new [] {'a', 'b', 'c', 'e', 'f', 'g' }), '0' },
        { new HashSet<char>(new [] {'c', 'f' }), '1' },
        { new HashSet<char>(new [] {'a', 'c', 'd', 'e', 'g' }), '2' },
        { new HashSet<char>(new [] {'a', 'c', 'd', 'f', 'g' }), '3' },
        { new HashSet<char>(new [] {'b', 'c', 'd', 'f' }), '4' },
        { new HashSet<char>(new [] {'a', 'b', 'd', 'f', 'g' }), '5' },
        { new HashSet<char>(new [] {'a', 'b', 'd', 'e', 'f', 'g' }), '6' },
        { new HashSet<char>(new [] {'a', 'c', 'f' }), '7' },
        { new HashSet<char>(new [] {'a', 'b', 'c', 'd', 'e', 'f', 'g' }), '8' },
        { new HashSet<char>(new [] {'a', 'b', 'c', 'd', 'f', 'g' }), '9' },

    };

    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/8.txt");
        if (exercise == 1)
        {
            var answer = lines
            .Select(line => line.Split(new string[] { " | " }, StringSplitOptions.None)[1])
            .SelectMany(end => end.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Where(s => s.Length is 2 or 3 or 4 or 7)
            .Count();
            System.Console.Write(answer);
        }
        else
        {
            var splitLines = lines
            .Select(line => line.Split(new string[] { " | " }, StringSplitOptions.None))
            .Select(lineParts =>
                (
                    lineParts[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries),
                    lineParts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                )
            );

            var answer = 0;

            foreach(var splitLine in splitLines)
            {
                var charCounts = new Dictionary<char, int>();
                FillCounts(splitLine.Item1, charCounts);

                var lengthCharCounts = new Dictionary<int, Dictionary<char, int>>();
                FillLengthCharCounts(splitLine.Item1, lengthCharCounts);

                var mappings = new Dictionary<char, char>();
                mappings['e'] = charCounts.
                    Where(x => x.Value == 4)
                    .Select(x => x.Key)
                    .First();
                mappings['b'] = charCounts.
                    Where(x => x.Value == 6)
                    .Select(x => x.Key)
                    .First();
                mappings['f'] = charCounts.
                    Where(x => x.Value == 9)
                    .Select(x => x.Key)
                    .First();
                mappings['a'] = lengthCharCounts[3].Keys
                    .Except(lengthCharCounts[2].Keys)
                    .First();
                mappings['c'] = lengthCharCounts[2].Keys
                    .Except(new [] { mappings['f'] })
                    .First();
                mappings['d'] = lengthCharCounts[6].Where(item => item.Value ==2).Select(item => item.Key)
                    .Intersect(lengthCharCounts[5]
                        .Where(item => item.Value == 3)
                        .Select(item => item.Key))
                    .First();
                mappings['g'] = charCounts.
                    Where(x => x.Value == 7)
                    .Select(x => x.Key)
                    .Except(new[] { mappings['d'] })
                    .First();

                var reverseMappings = mappings.ToDictionary(x => x.Value, x => x.Key);
                var x = splitLine.Item2.Select(s => s.ToList().Select(c => reverseMappings[c])).Select(s => new HashSet<char>(s));
                answer += Int32.Parse(String.Concat(x.Select(c => hashsetCharMapping[c])));
            }
            System.Console.Write(answer);
        }
    }

    private static void FillLengthCharCounts(string[] item1, Dictionary<int, Dictionary<char, int>> lengthCharCounts)
    {
        foreach (string s in item1)
        {
            var charCountsForLength = lengthCharCounts.TryGetValue(s.Length, out var x) ? x : new Dictionary<char, int>();
            foreach(char c in s)
            {
                charCountsForLength[c] = charCountsForLength.TryGetValue(c, out int i) ? i + 1 : 1;
            }

            lengthCharCounts[s.Length] = charCountsForLength;
        }
    }

    private static void FillCounts(string[] strings, Dictionary<char, int> counts)
    {
        foreach (string s in strings)
        {
            foreach (char c in s)
            {
                counts[c] = counts.TryGetValue(c, out int cur) ? cur + 1 : 1;
            }
        }
    }
}