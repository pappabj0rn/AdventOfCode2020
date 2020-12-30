using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class CrabCups
    {
        public List<int> Circle { get; }

        public CrabCups(IEnumerable<int> circle)
        {
            Circle = circle.ToList();
        }

        private int curCupIndex = 0;
        
        public void Move()
        {
            var curCup = Circle[curCupIndex];
            var movingCups = Circle
                .Skip(curCupIndex+1)
                .Take(3)
                .ToList();
            if (movingCups.Count < 3)
            {
                movingCups.AddRange(Circle.Take(3 - movingCups.Count));
            }
            
            foreach (var movingCup in movingCups)
            {
                Circle.Remove(movingCup);
            }

            var targetIndex = -1;
            var i = 1;
            while (targetIndex == -1)
            {
                var targetLabel = curCup - i;
                i++;
                
                if (targetLabel < Circle.Min())
                {
                    targetLabel = Circle.Max();
                }
                
                targetIndex =  Circle.IndexOf(targetLabel);
            }
            
            Circle.InsertRange(targetIndex+1, movingCups);
            curCupIndex = Circle.IndexOf(curCup);
            curCupIndex++;
            curCupIndex %= Circle.Count;
        }

        public List<int> GetOrderFromOne()
        {
            var list = new List<int>();
            
            var startIndex = Circle.IndexOf(1);
            var i = startIndex;
            
            do
            {
                i++;
                i %= Circle.Count;
                list.Add(Circle[i]);
            } while (i != startIndex);

            return list.Take(list.Count-1).ToList();
        }
    }
}