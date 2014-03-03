using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopDownShooterSpike.Simulation
{
    public class Map
    {
        private byte[,] _data;

        public Map() : this(64, 64)
        {
            
        }

        public Map(int width, int height)
        {
            _data = new byte[width, height];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0},{1}\n", _data.GetLength(0), _data.GetLength(1));

            for (int x = 0; x < _data.GetLength(0); x++)
            {
                for (int y = 0; y < _data.GetLength(1); y++)
                    builder.Append(_data[x, y]);

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static Map FromString(string mapData)
        {
            var rows = mapData.Split('\n');

            var dimensions = rows.Take(1).SelectMany(x => x.Split(',')).Select(byte.Parse).ToArray();

            var width = dimensions[0];
            var height = dimensions[1];

            var result = new Map(width, height);

            var data = rows.Skip(1)
                        .AsParallel()
                        .Select(row => row.ToCharArray().Select(item => item.ToString()).Select(byte.Parse))
                        .Select((row, index) => new {Index = index, Item = row.ToArray()})
                        .ToArray()
                        .Aggregate(new byte[width, height], (acc, tuple) =>
                        {
                            for (var index = 0; index < tuple.Item.Length; index++)
                                acc[tuple.Index, index] = tuple.Item[index];

                            return acc;
                        });

            result._data = data;

            return result;
        }

        public byte this[int x, int y]
        {
            get { return _data[x, y]; }
            set { _data[x, y] = value; }
        }
    }
}
