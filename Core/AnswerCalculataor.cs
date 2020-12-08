using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class AnswerCalculataor
    {
        public int CountUnique(IEnumerable<string> input)
        {
            return input.Aggregate("", (cur, next) => cur + next).Distinct().Count();
        }

        public IEnumerable<int> FInGroups(IEnumerable<string> input, Func<IEnumerable<string>,int> f)
        {
            void CalcGroup(List<string> list, List<int> ints)
            {
                if (!list.Any()) 
                    return;
                
                ints.Add(f(list));
                list.Clear();
            }

            var output = new List<int>();
            var curGroup = new List<string>();

            foreach (var line in input)
            {
                if(line.IsNullOrEmpty())
                {
                    CalcGroup(curGroup, output);
                    continue;
                }
                curGroup.Add(line);
            }
            
            CalcGroup(curGroup, output);
            
            
            return output;
        }

        public int CountRecurring(IEnumerable<string> input)
        {
            var grouped = input
                .Aggregate("", (cur, next) => cur + next)
                .GroupBy(c => c);
            var matchingInputCount = grouped.Count(g => g.Count() == input.Count());
            return matchingInputCount;
        }
    }
}