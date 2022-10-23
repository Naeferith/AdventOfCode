using AdventOfCode.Core.AoC;
using AdventOfCode.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.V2021.Days
{
    internal class Day16 : IDay
    {
        public string PuzzleName => "Packet Decoder";

        public string Solution1(string[] lines) => Solve(lines, p => p.VersionSum().ToString());

        public string Solution2(string[] lines) => Solve(lines, p => p.ComputedData.ToString());

        private static string Solve(string[] lines, Func<Packet, string> f)
        {
            var packet = string.Join(string.Empty, lines[0].Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            return f.Invoke(Parse(packet, out var _));
        }

        private static Packet Parse(string packet, out int parsedChars)
        {
            var version = Convert.ToInt32(packet[0..3], 2);
            var typeId = Convert.ToInt32(packet[3..6], 2);
            parsedChars = 6;

            var model = new Packet(version, typeId);

            if (typeId == 4)
            {
                var data = packet.Skip(6).ChunkBy(5);
                var buffer = new StringBuilder();

                foreach (var d in data)
                {
                    var s = new string(d.ToArray());
                    buffer.Append(s[^4..]);
                    parsedChars += 5;
                    if (s[0] == '0')
                        break;
                }
                model.LiteralData = buffer.ToString();
                return model;
            }

            var lenthTypeId = packet[6];
            var subs = new List<Packet>();
            var l = 0;

            switch (packet[6])
            {
                case '0':
                    {
                        parsedChars += 16;
                        var len = Convert.ToInt32(packet[7..22], 2);
                        while (l < len)
                        {
                            subs.Add(Parse(packet[(22 + l)..], out var pc));
                            l += pc;
                        }
                    }
                    break;
                case '1':
                    {
                        parsedChars += 12;
                        var len = Convert.ToInt32(packet[7..18], 2);
                        for (int i = 0; i < len; i++)
                        {
                            subs.Add(Parse(packet[(18 + l)..], out var pc));
                            l += pc;
                        }
                    }
                    break;
            }

            parsedChars += l;
            model.SubPackets.AddRange(subs.Where(s => s != null));
            return model;
        }

        private class Packet
        {
            public int Version { get; }
            public int TypeID { get; }
            public List<Packet> SubPackets { get; }

            public Packet(int version, int typeId)
            {
                Version = version;
                TypeID = typeId;
                SubPackets = new();
            }

            public string LiteralData { get; set; }

            public int VersionSum()
            {
                if (!SubPackets.Any())
                    return Version;

                var s = 0;
                foreach (var sub in SubPackets)
                {
                    s += sub.VersionSum();
                }
                return Version + s;
            }

            public long ComputedData
            {
                get
                {
                    return TypeID switch
                    {
                        0 => SubPackets.Sum(p => p.ComputedData),
                        1 => SubPackets.Select(p => p.ComputedData).Aggregate(1L, (a, p) => a * p),
                        2 => SubPackets.Min(p => p.ComputedData),
                        3 => SubPackets.Max(p => p.ComputedData),
                        5 => SubPackets[0].ComputedData > SubPackets[1].ComputedData ? 1 : 0,
                        6 => SubPackets[0].ComputedData < SubPackets[1].ComputedData ? 1 : 0,
                        7 => SubPackets[0].ComputedData == SubPackets[1].ComputedData ? 1 : 0,
                        _ => Convert.ToInt64(LiteralData, 2),
                    };
                }
            }
        }
    }
}
