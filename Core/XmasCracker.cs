using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class XmasCracker
    {
        private Queue<long> Preamble { get; set; }
        private Queue<long> FullList { get; set; }

        public void Init(IEnumerable<long> preamble)
        {
            Preamble = new Queue<long>(preamble);
            FullList = new Queue<long>(preamble);
        }
        
        public bool ValidateNumber(in long candidate)
        {
            var ok = false;

            foreach (var d in Preamble)
            {
                var remainder = candidate - d;
                if(remainder == d) 
                    continue;

                if (Preamble.Contains(remainder))
                {
                    ok = true;
                    Preamble.Dequeue();
                    Preamble.Enqueue(candidate);
                    FullList.Enqueue(candidate);
                    break;
                }
            }

            return ok;
        }

        public long[] FindContiguousNumbersOfSum(long target)
        {
            var values = new Queue<long>(FullList.ToArray());
            var contiguousSet = new Queue<long>();

            while (contiguousSet.Sum() != target)
            {
                if (contiguousSet.Sum() < target)
                {
                    contiguousSet.Enqueue(values.Dequeue());
                }
                else
                {
                    contiguousSet.Dequeue();
                }
            }
            
            return contiguousSet.ToArray();
        }
    }
}
