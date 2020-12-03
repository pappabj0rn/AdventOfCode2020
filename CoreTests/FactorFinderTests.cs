using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class FactorFinderTests
    {
        public class FindTwoCandidatesThatSumTo : FactorFinderTests
        {
            [Fact]
            public void Should_find_candidates_that_sum_to_given_target()
            {
                var finder = new FactorFinder
                {
                    Candidates = new[]
                    {
                        1721,979,366,299,675,1456
                    }
                };
                
                var result = finder.FindTwoCandidatesThatSumTo(2020).ToList();

                Assert.Contains(1721, result);
                Assert.Contains(299, result);
            }

            [Fact(Skip = "enable for answer")]
            public void Puzzle_1A()
            {
                var input = PuzzleInputs.Puzzle1;
                
                var finder = new FactorFinder
                {
                    Candidates = input
                };

                var result = finder.FindTwoCandidatesThatSumTo(2020).ToArray();
                
                Assert.Equal(2020, result[0] + result[1]);
                Assert.Equal(1, result[0]*result[1]);
            }
        }

        public class FindThreeCandidatesThatSumTo : FactorFinderTests
        {
            [Fact]
            public void Should_find_three_candidates_that_sum_to_target()
            {
                var finder = new FactorFinder
                {
                    Candidates = new[]
                    {
                        1721,979,366,299,675,1456
                    }
                };
                
                var result = finder.FindThreeCandidatesThatSumTo(2020).ToList();

                Assert.Equal(3, result.Count);
                Assert.Contains(979, result);
                Assert.Contains(366, result);
                Assert.Contains(675, result);
            }
            
            [Fact(Skip = "enable for answer")]
            public void Puzzle_1B()
            {
                var input = PuzzleInputs.Puzzle1;
                
                var finder = new FactorFinder
                {
                    Candidates = input
                };

                var result = finder.FindThreeCandidatesThatSumTo(2020).ToArray();

                Assert.Equal(2020, result[0] + result[1] + result[2]);
                Assert.Equal(1, result[0] * result[1] * result[2]);
            }
        }
    }
}