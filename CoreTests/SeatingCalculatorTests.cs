using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class SeatingCalculatorTests
    {
        public class CalculateSeatNo
        {
            [Theory]
            [InlineData("FBFBBFFRLR",44,5,357)]
            [InlineData("BFFFBBFRRR",70,7,567)]
            [InlineData("FFFBBBFRRR",14,7,119)]
            [InlineData("BBFFBBFRLL",102,4,820)]
            public void Should_match_example_data(string input, int expectedRow, int expectedSeat, int expectedSeatId)
            {
                var sut = new SeatingCalculator();

                var result = sut.CalculateSeatNo(input);
                
                Assert.Equal(expectedRow, result.row);
                Assert.Equal(expectedSeat, result.seat);
                Assert.Equal(expectedSeatId, result.seatId);
            }

            [Fact]
            public void Puzzle_5A()
            {
                var highest = 0;
                var highestInput = "";
                
                var calculator = new SeatingCalculator();
                foreach (var input in PuzzleInputs.Puzzle5)
                {
                    var result = calculator.CalculateSeatNo(input);
                    if (result.seatId > highest)
                    {
                        highest = result.seatId;
                        highestInput = input;
                    }
                }
                
                Assert.Equal(826, highest);
            }

            [Fact]
            public void Puzzle_5B()
            {
                var prev = 0;
                var gap = 0;
                
                var calculator = new SeatingCalculator();
                var calcs = PuzzleInputs.Puzzle5.Select(calculator.CalculateSeatNo).OrderBy(x => x.seatId);
                foreach (var calc in calcs)
                {
                    if (calc.seatId == prev+2)
                    {
                        gap = prev + 1;
                    }

                    prev = calc.seatId;
                }
                
                Assert.Equal(678, gap);
            }
        }
    }
}