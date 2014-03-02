using System.Runtime.InteropServices;

namespace TopDownShooterSpike.World
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Tile
    {
        [FieldOffset(0)] 
        public ushort TileType;

        public static ushort EMPTY = 0x0000;
        public static ushort FLOOR = 0x0001;
        public static ushort WALL = 0x0002;
        public static ushort DOOR = 0x0003;
    }
}
