using static Util;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

class Day17
{
    public static void Run(int exercise = 1)
    {
        var line = ReadLines("../../../input/17.txt").First();
        var regex = new Regex(@"x=(?<xmin>\-?\d+)..(?<xmax>\-?\d+)\, y=(?<ymin>\-?\d+)..(?<ymax>\-?\d+)");
        var match = regex.Match(line);
        if (match.Success)
        {
            var xMin = Math.Min(Int32.Parse(match.Groups["xmin"].Value), Int32.Parse(match.Groups["xmax"].Value));
            var xMax = Math.Max(Int32.Parse(match.Groups["xmin"].Value), Int32.Parse(match.Groups["xmax"].Value));
            var yMin = Math.Min(Int32.Parse(match.Groups["ymin"].Value), Int32.Parse(match.Groups["ymax"].Value));
            var yMax = Math.Max(Int32.Parse(match.Groups["ymin"].Value), Int32.Parse(match.Groups["ymax"].Value));

            var maxYLaunchVelocity = Math.Abs(yMin + 1);
            var maxXLaunchVelocity = Math.Abs(xMax);

            Console.WriteLine(Enumerable.Range(0, maxYLaunchVelocity + 1).Aggregate(0, (acc, val) => acc + val));

            var validVelocities = new List<(int, int)>();

            var dims =
                from x in Enumerable.Range(0, maxXLaunchVelocity + 1)
                from y in Enumerable.Range(-maxYLaunchVelocity - 1, 2 * maxYLaunchVelocity + 2)
                select (x, y);

            foreach (var option in dims)
            {
                if (TryVelocity(option.x, option.y, xMin, xMax, yMin, yMax))
                {
                    validVelocities.Add(option);
                }
            }

            Console.WriteLine(validVelocities.Distinct().Count());
        }
    }

    private static bool TryVelocity(int xVelocity, int yVelocity, int xMin, int xMax, int yMin, int yMax)
    {
        var xPos = 0;
        var yPos = 0;

        var xRange = Enumerable.Range(xMin, xMax - xMin + 1);
        var yRange = Enumerable.Range(yMin, Math.Abs(yMax - yMin) + 1);
        while ((xVelocity > 0 || xPos >= xMin || xPos <= xMax) && yVelocity >= yMin)
        {
            xPos += xVelocity;
            yPos += yVelocity;
            if (xRange.Contains(xPos) && yRange.Contains(yPos))
            {
                return true;
            }
            yVelocity--;
            xVelocity = Math.Max(0, xVelocity-1);
        }

        return false;
    }
}

