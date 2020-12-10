using Core;
using Xunit;

namespace CoreTests
{
    public abstract class JoltageAdapterCalculatorTests
    {
        public class FindDiffs
        {
            [Fact]
            public void Example_1()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[] {16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4}
                };

                var result = calc.FindDiffs();

                Assert.Equal(7, result.ones);
                Assert.Equal(5, result.threes);
            }

            [Fact]
            public void Should_add_first_adapter_to_ones_when_lowest_adapter_is_one()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[] /*0*/{1, 4}/*7*/
                };

                var result = calc.FindDiffs();

                Assert.Equal(1, result.ones);
                Assert.Equal(2, result.threes);
            }

            [Fact]
            public void Should_add_first_adapter_to_threes_when_lowest_adapter_is_three()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[] /*0*/{3,6}/*9*/
                };

                var result = calc.FindDiffs();

                Assert.Equal(0, result.ones);
                Assert.Equal(3, result.threes);
            }

            [Fact]
            public void Example_2()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[]
                    {
                        28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7,
                        9, 4, 2, 34, 10, 3
                    }
                };

                var result = calc.FindDiffs();

                Assert.Equal(22, result.ones);
                Assert.Equal(10, result.threes);
            }

            [Fact(Skip = "done")]
            public void Puzzle_10A()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = PuzzleInputs.Puzzle10
                };

                var result = calc.FindDiffs();

                Assert.Equal(2590, result.ones*result.threes);
            }
        }

        public class FindVariations
        {
            [Fact]
            public void Example_1()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[] {16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4}
                };

                var variations = calc.FindVariations();

                Assert.Equal(8, variations);
            }
            
            [Theory]
            [InlineData(new []{1,4,5,6}, 2)]
            [InlineData(new []{1,4,5,6,9,10,11}, 4)]
            public void Variations(int[] adapters, int expected)
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = adapters
                };

                var variations = calc.FindVariations();

                Assert.Equal(expected, variations);
            }
            
            [Fact]
            public void Example_2()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = new[]
                    {
                        28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7,
                        9, 4, 2, 34, 10, 3
                    }
                };

                var variations = calc.FindVariations();

                Assert.Equal(19208, variations);
            }
            
            [Fact]
            public void Puzzle_10B()
            {
                var calc = new JoltageAdapterCalculator
                {
                    Adapters = PuzzleInputs.Puzzle10
                };

                var variations = calc.FindVariations();
                Assert.Equal(226775649501184, variations);
            }
        }
    }
}