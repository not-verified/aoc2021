using System;

class Util
{
    public static string[] ReadLines(string file)
    {
        var ret = new List<string>();

        foreach (string line in System.IO.File.ReadLines(file))
        {
            ret.Add(line);
        }

        return ret.ToArray();
    }
}