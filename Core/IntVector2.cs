using System.Diagnostics;

namespace Core
{
    public struct IntVector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }
            
        public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new IntVector2{X = a.X+b.X, Y = a.Y+b.Y}; 
    }
    
    [DebuggerDisplay("{X},{Y},{Z}")]
    public struct IntVector3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public IntVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
            
        public static IntVector3 operator +(IntVector3 a, IntVector3 b) 
            => new IntVector3
            {
                X = a.X+b.X, 
                Y = a.Y+b.Y,
                Z = a.Z+b.Z
            }; 
    }
    
    public struct IntVector4
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }

        public IntVector4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
            
        public static IntVector4 operator +(IntVector4 a, IntVector4 b) 
            => new IntVector4
            {
                X = a.X+b.X, 
                Y = a.Y+b.Y,
                Z = a.Z+b.Z,
                W = a.W+b.W
            }; 
    }
}