using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class BusCalculatorTests
    {
        public class CalculateWaitTimeForNextBus
        {
            [Fact]
            public void Should_return_calculate_waitTime_with_one_buss()
            {
                var calc = new BusCalculator();

                var result = calc.CalculateWaitTimeForNextBus(25, new[] {7});

                Assert.Equal(3, result.waitTime);
            }

            [Fact]
            public void Example_1()
            {
                var calc = new BusCalculator();

                var result = calc.CalculateWaitTimeForNextBus(939, new[] {7, 13, 59, 31, 19});

                Assert.Equal(5, result.waitTime);
                Assert.Equal(59, result.id);
                Assert.Equal(295, result.waitTime * result.id);
            }

            [Fact]
            public void Puzzle_13A()
            {
                var arrivalTime = int.Parse(PuzzleInputs.Puzzle13[0]);

                var buses = PuzzleInputs.Puzzle13[1]
                    .Split(',')
                    .Where(b => b != "x")
                    .Select(int.Parse)
                    .ToArray();

                var calc = new BusCalculator();

                var result = calc.CalculateWaitTimeForNextBus(arrivalTime, buses);

                Assert.Equal(153, result.waitTime * result.id);
            }
        }

        public class FindEarliestTimestampForSubsequentDepartures
        {
            [Theory]
            [InlineData(new[]{"6","x","7","5"}, 12)]
            [InlineData(new[]{"17","x","13","19"}, 3_417)]
            [InlineData(new[]{"7", "13", "x", "x", "59", "x", "31", "19"}, 1_068_781)]
            [InlineData(new[]{"67","7","59","61"}, 754018)]
            [InlineData(new[]{"67","x","7","59","61"}, 779210)]
            [InlineData(new[]{"67","7","x","59","61"}, 1261476)]
            [InlineData(new[]{"1789","37","47","1889"}, 1202161486)]
            public void Examples(string[] buses, long expTimestamp)
            {
                var calc = new BusCalculator();

                var result = calc.FindEarliestTimestampForSubsequentDepartures(buses);

                Assert.Equal(expTimestamp, result);
            }

            [Fact]
            public void Puzzle_13B()
            {
                var buses = PuzzleInputs.Puzzle13[1]
                    .Split(',')
                    .ToArray();
                
                var calc = new BusCalculator();

                var result = calc.FindEarliestTimestampForSubsequentDepartures(buses, 100_000_000_000_000);

                Assert.Equal(471_793_476_184_394, result);
            } 
        }
    }
}