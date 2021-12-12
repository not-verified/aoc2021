using System;
using static Util;

class Day10
{
    private static char[] _openChars = new[] { '(', '[', '{', '<' };
    private static char[] _closeChars = new[] { ')', ']', '}', '>' };
    private static Dictionary<char, int> _errorCost = new ()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 },
    };

    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/10.txt");
        var errors = new List<char>();
        var incompleteStackParts = new List<Stack<char>>();
        foreach (var line in lines)
        {
            var hasError = false;
            var stack = new Stack<char>();
            foreach (char c in line)
            {
                if (_openChars.Contains(c))
                {
                    stack.Push(c);
                    continue; // added opening bracket
                }

                var topElement = stack.Pop();
                if (Array.IndexOf(_openChars, topElement) == Array.IndexOf(_closeChars, c))
                     continue; // closed most recent bracket

                hasError = true; // Not opening, but also could not close!
                errors.Add(c); 
                break;
            }

            if (!hasError)
            {
                incompleteStackParts.Add(stack);
            }
        }

        if (exercise == 1)
        {
            Console.Write(errors.Select(c => _errorCost[c]).Sum());
        }
        else
        {
            var scores = new List<long>();
            foreach (var stack in incompleteStackParts)
            {
                long score = 0;
                while (stack.TryPop(out char c))
                {
                    score *= 5;
                    score += Array.IndexOf(_openChars, c) + 1;
                }
                scores.Add(score);
            }
            scores.Sort();
            Console.Write(scores[scores.Count / 2]);
        }
    }
}
            