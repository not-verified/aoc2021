using System;
using static Util;

class Day4
{
    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/4.txt");
        var drawNumbers = lines.First().Split(',').Select(x => Int32.Parse(x)).ToArray();
        var boards = ParseBoards(lines.Skip(2).ToArray()).ToArray();
        var haveWinner = false;
        int idx = 0;
        Board? winner = null;
        int? winningNumber = null;
        while (exercise == 1 ? !haveWinner : idx < drawNumbers.Count())
        {
            var cur = drawNumbers[idx];
            foreach(var board in boards.Where(board => !board.HasWon))
            {
                foreach(var line in board.Lines)
                {
                    board.Members = board.Members.Select(member => member.Value == cur ? new Member(cur, true) : member).ToArray();
                    line.Members = line.Members.Select(member => member.Value == cur ? new Member(cur, true) : member).ToArray();
                    if (line.Members.All(member => member.Set))
                    {
                        haveWinner = true;
                        winningNumber = cur;
                        winner = board;
                        board.HasWon = true;
                    }
                }
            }
            idx++;
        }
        var uncrossedSum = winner.Members.Where(member => !member.Set).Select(member => member.Value).Sum();
        System.Console.Write(uncrossedSum * winningNumber);
    }

    private static IEnumerable<Board> ParseBoards(string[] lines)
    {
        var idx = 0;
        while(lines.Count() >= idx + 5)
        {
            yield return new Board(lines.Skip(idx).Take(5).SelectMany(x => x.Split(' ').Where(y => y != "")).ToArray());
            idx += 6;
        }
    }

    private class Board
    {
        public Board(string[] values)
        {
            var horizontal = values.Select((x, idx) => new { x, idx }).GroupBy(x => x.idx / 5, y => y.x).Select(x => new Line(x.ToArray())).ToArray();
            var vertical = values.Select((x, idx) => new { x, idx }).GroupBy(x => x.idx % 5, y => y.x).Select(x => new Line(x.ToArray())).ToArray();
            Lines = horizontal.Concat(vertical).ToArray();
            Members = values.Select(val => new Member(Int32.Parse(val), false)).ToArray();
            HasWon = false;
        }

        public Line[] Lines;

        public Member[] Members;

        public bool HasWon;
    }

    public class Line
    {
        public Line(string[] values)
        {
            Members = values.Select(val => new Member(Int32.Parse(val), false)).ToArray();
        }

        public Member[] Members;
    }

    public class Member
    {
        public Member(int val, bool set)
        {
            Value = val;
            Set = set;
        }
        public int Value;
        public bool Set;
    }
}
