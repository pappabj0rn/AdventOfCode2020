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
}