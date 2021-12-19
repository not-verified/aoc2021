using static Util;
using System.Linq;
using System.Collections;
using System.Globalization;

class Day16
{
    private class Packet
    {
        public int Version;
        public int Type;
        public List<Packet> ContainedPackets = new List<Packet>();
        public long Value;
        public char? LengthTypeId;

        public int VersionSum
        {
            get
            {
                return Version + ContainedPackets.Sum(packet => packet.VersionSum);
            }
        }

        public long PacketValue
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return ContainedPackets.Sum(p => p.PacketValue);
                    case 1:
                        return ContainedPackets.Aggregate((long)1, (acc, p) => acc * p.PacketValue);
                    case 2:
                        return ContainedPackets.Min(p => p.PacketValue);
                    case 3:
                        return ContainedPackets.Max(p => p.PacketValue);
                    case 5:
                        return Convert.ToInt64(ContainedPackets.First().PacketValue > ContainedPackets.Skip(1).Single().PacketValue);
                    case 6:
                        return Convert.ToInt64(ContainedPackets.First().PacketValue < ContainedPackets.Skip(1).Single().PacketValue);
                    case 7:
                        return Convert.ToInt64(ContainedPackets.First().PacketValue == ContainedPackets.Skip(1).Single().PacketValue);
                    case 4:
                    default:
                        return Value;
                }
            }
        }
    }

    public static void Run(int exercise = 1)
    {
        var lines = ReadLines("../../../input/16.txt");
        var bits = new Queue<char>(HexStringToBinary(lines.First()));
        var packet = Parse(bits).First();

        Console.WriteLine(packet.VersionSum);
        Console.WriteLine(packet.PacketValue);

    }

    private static IEnumerable<Packet> Parse(Queue<char> bits, int current = 0, int? expected = null)
    {
        while ((expected == null || current < expected) && bits.Any(x => x == '1'))
        {
            var packet = new Packet();
            packet.Version = Convert.ToInt16(String.Join("", bits.DequeueChunk(3).ToArray()), 2);
            packet.Type = Convert.ToInt16(String.Join("", bits.DequeueChunk(3).ToArray()), 2);

            switch (packet.Type)
            {
                case 4:
                    packet.Value = ParseLiteral(bits);
                    break;
                default:
                    packet.LengthTypeId = bits.Dequeue();
                    break;
            }

            if (packet.Type != 4)
            {
                switch (packet.LengthTypeId)
                {
                    case '0':
                        var length = Convert.ToInt16(String.Join("", bits.DequeueChunk(15).ToArray()), 2);
                        packet.ContainedPackets = Parse(new Queue<char>(bits.DequeueChunk(length)), 0, null).ToList();
                        break;
                    case '1':
                        var expectedNumberOfPackets = Convert.ToInt32(String.Join("", bits.DequeueChunk(11).ToArray()).ToString(), 2);
                        packet.ContainedPackets = Parse(bits, 0, expectedNumberOfPackets).ToList();
                        break;
                }
            }

            current++;
            yield return packet;
        }
    }

    private static long ParseLiteral(Queue<char> bits)
    {
        var last = false;
        var parts = new List<char>();
        while (!last)
        {
            var part = bits.DequeueChunk(5).ToList();
            last = part.First() == '0';
            parts.AddRange(part.Skip(1));
        }
        return Convert.ToInt64(String.Join("", parts), 2);
    }
}

