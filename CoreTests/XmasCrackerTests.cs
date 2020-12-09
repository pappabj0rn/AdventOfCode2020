using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class XmasCrackerTests
    {
        public class ValidateNumber
        {
            [Theory]
            [InlineData(new long[]{1,2}, 3)]
            [InlineData(new long[]{1,2,3}, 3)]
            [InlineData(new long[]{1,2,3}, 4)]
            [InlineData(new long[]{1,2,3}, 5)]
            public void Should_return_true_for_a_number_having_at_least_one_pair_in_preamble_summing_to_input(
                long[] preamble, 
                long candidate)
            {
                var cracker = new XmasCracker();
                cracker.Init(preamble);

                var ok = cracker.ValidateNumber(candidate);

                Assert.True(ok);
            }
            
            [Theory]
            [InlineData(new long[]{1,2}, 4)]
            [InlineData(new long[]{1,2,3}, 2)]
            [InlineData(new long[]{1,2,3}, 6)]
            [InlineData(new long[]{1,2,3}, 1)]
            public void Should_return_false_for_a_number_note_having_any_pair_of_numbers_in_preamble_summing_to_input(
                long[] preamble, 
                long candidate)
            {
                var cracker = new XmasCracker();
                cracker.Init(preamble);

                var ok = cracker.ValidateNumber(candidate);

                Assert.False(ok);
            }
            
            [Theory]
            [InlineData(new long[]{1,2}, new long[]{3,5})]
            //[InlineData(new long[]{1,2,3}, 3)]
            //[InlineData(new long[]{1,2,3}, 4)]
            //[InlineData(new long[]{1,2,3}, 5)]
            public void Should_remove_first_digit_of_preamble_and_add_valid_candidate_to_preamble(
                long[] preamble, 
                long[] candidates)
            {
                var cracker = new XmasCracker();
                cracker.Init(preamble);

                var ok = false;
                long c = 0;
                foreach (var candidate in candidates)
                {
                    c = candidate;
                    ok = cracker.ValidateNumber(candidate);                    
                }

                Assert.True(ok,$"{c} did not validate");
            }

            [Fact]
            public void Example()
            {
                var cracker = new XmasCracker();
                cracker.Init(new long[] {35, 20, 15, 25, 47});

                var candidates = new long[] {40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576};

                long firstFail = 0;
                foreach (var candidate in candidates)
                {
                    var valid = cracker.ValidateNumber(candidate);
                    if (!valid)
                    {
                        firstFail = candidate;
                        break;
                    }
                }
                
                Assert.Equal(127,firstFail);
            }
            
            [Fact(Skip = "done")]
            public void Puzzle_9A()
            {
                var cracker = new XmasCracker();
                cracker.Init(PuzzleInputs.Puzzle9.Take(25));

                var candidates = PuzzleInputs.Puzzle9.Skip(25);

                long firstFail = 0;
                foreach (var candidate in candidates)
                {
                    var valid = cracker.ValidateNumber(candidate);
                    if (!valid)
                    {
                        firstFail = candidate;
                        break;
                    }
                }
                
                Assert.Equal(14144619,firstFail);
            }
        }

        public class FindContiguousNumbersOfSum
        {
            [Fact]
            public void Example_2()
            {
                var cracker = new XmasCracker();
                cracker.Init(new long[] {35, 20, 15, 25, 47});

                var candidates = new long[] {40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576};
                
                var contiguousSet = new long[0];
                foreach (var candidate in candidates)
                {
                    var valid = cracker.ValidateNumber(candidate);
                    if (!valid)
                    {
                        contiguousSet = cracker.FindContiguousNumbersOfSum(candidate);
                        break;
                    }
                }
                
                Assert.Equal(4, contiguousSet.Length);
                Assert.Equal(15, contiguousSet[0]);
                Assert.Equal(25, contiguousSet[1]);
                Assert.Equal(47, contiguousSet[2]);
                Assert.Equal(40, contiguousSet[3]);
                
                var result = contiguousSet.Min() + contiguousSet.Max();
                Assert.Equal(62, result);
            }
            
            [Fact]
            public void Puzzle_9B()
            {
                var cracker = new XmasCracker();
                cracker.Init(PuzzleInputs.Puzzle9.Take(25));

                var candidates = PuzzleInputs.Puzzle9.Skip(25);
                
                var contiguousSet = new long[0];
                foreach (var candidate in candidates)
                {
                    var valid = cracker.ValidateNumber(candidate);
                    if (!valid)
                    {
                        contiguousSet = cracker.FindContiguousNumbersOfSum(candidate);
                        break;
                    }
                }
                
                Assert.Equal(17, contiguousSet.Length);
                var result = contiguousSet.Min() + contiguousSet.Max();
                Assert.Equal(1766397, result);
            }
        }
    }
}