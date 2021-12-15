using System;
using System.Drawing;
using System.Text;
using static Util;

class Day13
{
    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/13.txt");
        (var points, var rules) = ParseLines(lines);

        if (exercise == 1)
        {
            rules = new List<(string dim, int border)> { rules.First() };
        }

        var ret = points.Select(p => ApplyRules(p, rules)).Where(x => !x.drop).Distinct().ToList();
        
        var sb = new StringBuilder();
        for (int y = 0; y <= ret.Max(p => p.y); y++)
        {
            for (int x = 0; x <= ret.Max(p => p.x); x++)
            {
                if (ret.Contains((x, y, false)))
                {
                    sb.Append("#");
                } else
                {
                    sb.Append(" ");
                }
            }
            sb.Append('\n');
        }

        System.Console.WriteLine($"Count after applying rules: { ret.Count() }");
        System.Console.Write(sb.ToString());
    }

    private static (int x, int y, bool drop) ApplyRules((int x, int y, bool drop) p, List<(string dim, int border)> rules)
    {
        var ret = p;

        foreach (var rule in rules)
        {
            if (rule.dim == "y")
            {
                if (ret.y == rule.border)
                {
                    return (ret.x, ret.y, true);
                }
                ret.y = ret.y > rule.border ? rule.border - (Math.Abs(rule.border - ret.y)) : ret.y;
            }

            if (rule.dim == "x")
            {
                if (ret.x == rule.border)
                {
                    return (ret.x, ret.y, true);
                }
                ret.x = ret.x > rule.border ? rule.border - (Math.Abs(rule.border - ret.x)) : ret.x;
            }
        }

        return ret;
    }

    private static (List<(int x, int y, bool drop)>, List<(string dim, int border)>) ParseLines(string[] lines)
    {
        var points = new List<(int x, int y, bool drop)>();
        var rules = new List<(string dim, int border)>();

        var ruleSection = false;
        foreach (var line in lines)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                ruleSection = true;
            }

            else if (!ruleSection)
            {
                var vals = line.Split(',');
                points.Add((Int32.Parse(vals[0]), Int32.Parse(vals[1]), false));
            }

            else
            {
                var vals = line.Split(' ').Last().Split('=');
                rules.Add((vals[0], Int32.Parse(vals[1])));
            }
        }

        return (points, rules);
    }
}