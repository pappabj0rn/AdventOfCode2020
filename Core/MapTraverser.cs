using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class MapTraverser
    {
        private const char Three = '#';
        private const char Clear = '.';
        
        public string[] MapSegment { get; set; }
        
        private IntVector2 currentPos = new IntVector2();

        public int CountTreesForDescentVector(IntVector2 v)
        {
            var count = 0;
            do
            {
                currentPos += v;
                currentPos.X %= MapSegment[0].Length;
                var elevation = MapSegment[currentPos.Y];
                count += elevation[currentPos.X] == Three ? 1 : 0;
            } while (currentPos.Y < MapSegment.Length-1);
            
            return count;
        }

    }
        public struct IntVector2
        {
            public int X { get; set; }
            public int Y { get; set; }
            
            public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new IntVector2{X = a.X+b.X, Y = a.Y+b.Y}; 
        }
}