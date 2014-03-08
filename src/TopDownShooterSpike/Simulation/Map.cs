﻿using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TopDownShooterSpike.Simulation
{
    public class Map
    {
        private byte[,] _data;

        public Map() : this(64, 64)
        {
            this.FillRect(0, 0, Width, Height, false, Tile.Wall);

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
                        .Select(row => row.ToCharArray().Select(item => item.ToString(CultureInfo.InvariantCulture)).Select(byte.Parse))
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

        public int Height
        {
            get { return _data.GetLength(0); }
        }
        public int Width
        {
            get { return _data.GetLength(1); }
        }
    }

    public enum TileType : byte
    {
        Empty = 0,
        Floor = 0x01,
        Wall = 0x02
    }

    public enum IlluminationType : byte
    {
        Unlit = 0,
        Occlude = 0x01,
        Illuminated = 0x02
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Tile
    {
        [FieldOffset(0)]
        public byte Type;

        public Tile(byte type)
        {
            Type = type;
        }

        public static readonly Tile Wall = new Tile(0);
    }

    public static class MapBuilderExtension
    {
        public static Map FillRect(this Map map, int startx, int starty, int rectWidth, int rectHeight, bool fill, Tile tileType)
        {
            var invalidIndex = startx < 0 || starty < 0 || startx >= map.Width || starty >= map.Height;
            var invalidDimensions = rectHeight < 0 || rectHeight < 0 || rectWidth >= map.Width || rectHeight >= map.Height;

            if (invalidDimensions || invalidIndex)
                throw new ArgumentOutOfRangeException();

            for (int x = startx; x < rectWidth; x++)
            {
                if (!fill)
                {
                    map[x, 0] = tileType;
                    map[x, rectWidth] = tileType;
                }

                for (int y = starty; y < rectHeight; y++)
                {
                    if (fill)
                        map[x, y] = tileType;
                    else
                    {
                        map[0, y] = tileType;
                        map[rectHeight, y] = tileType;
                    }
                    
                }
            }

            return map;
        }
    }
    
}