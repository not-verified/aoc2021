using System;
using static Util;

class Day7
{
    
    public static void Run(int exercise = 1)
    {
        var population = ReadLines("../../../input/7.txt").First().Split(',')
            .Select(s => Int32.Parse(s));

        if (exercise == 1)
        {
            var median = GetMedian(population);
            System.Console.Write(population.Select(i => Math.Abs(i - median)).Sum());
        }
        else
        {
            double cost = 99999999999;
            for (int i = population.Min(); i <= population.Max(); i++)
            {
                var newCost = population.Select(elem => Math.Abs(i - elem) * ((Math.Abs(i - elem) + 1.0) / 2)).Sum();
                cost = Math.Min(cost, newCost);
            }
            System.Console.Write(cost);
        }
    }

    private static decimal GetMedian(IEnumerable<int> source)
    {
        // Create a copy of the input, and sort the copy
        int[] temp = source.ToArray();
        Array.Sort(temp);
        int count = temp.Length;
        if (count == 0)
        {
            throw new InvalidOperationException("Empty collection");
        }
        else if (count % 2 == 0)
        {
            // count is even, average two middle elements
            int a = temp[count / 2 - 1];
            int b = temp[count / 2];
            return (a + b) / 2m;
        }
        else
        {
            // count is odd, return the middle element
            return temp[count / 2];
        }
    }
}