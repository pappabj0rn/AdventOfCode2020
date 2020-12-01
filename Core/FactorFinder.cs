using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class FactorFinder
    {
        public IEnumerable<int> Candidates { get; set; }

        public IEnumerable<int> FindTwoCandidatesThatSumTo(int target)
        {
            foreach (var c in Candidates)
            {
                var remainder = target - c;
                var c2 = Candidates.SingleOrDefault(x => x == remainder);
                if (c2 > 0)
                    return new[] {c, c2};
            }
            
            return new int[0];
        }
        
        public IEnumerable<int> FindThreeCandidatesThatSumTo(int target)
        {
            foreach (var c in Candidates)
            {
                var remainder = target - c;
                var stage2Candidates = Candidates.Where(x => x < remainder).ToArray();

                foreach (var c2 in stage2Candidates)
                {
                    var stage2Remainder = remainder - c2;
                    var c3 = Candidates.SingleOrDefault(x => x == stage2Remainder);
                    if (c3 > 0)
                        return new[] {c, c2, c3};
                }
            }
            
            return new int[0];
        }
    }
}