using Core;
using Xunit;

namespace CoreTests
{
    public class ElfMemoryTests
    {
        [Fact]
        public void Speak_should_advance_turn_by_one()
        {
            var game = new ElfMemoryGame();
            
            game.Speak();
            
            Assert.Equal(1, game.Turn);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 3)]
        [InlineData(3, 6)]
        [InlineData(4, 0)]
        [InlineData(5, 3)]
        [InlineData(6, 3)]
        [InlineData(7, 1)]
        [InlineData(8, 0)]
        [InlineData(9, 4)]
        [InlineData(10, 0)]
        public void Example_1(int testTurn, int expectedNumber)
        {
            var game = new ElfMemoryGame
            {
                StartingNumbers = new[] {0, 3, 6}
            };

            for (var i = 0; i < testTurn; i++)
            {
                game.Speak();                
            }
            
            Assert.Equal(expectedNumber, game.LastNumber);
        }
        
        [Theory]
        [InlineData(new[]{1,3,2}, 1)]
        [InlineData(new[]{2,1,3}, 10)]
        [InlineData(new[]{1,2,3}, 27)]
        [InlineData(new[]{2,3,1}, 78)]
        [InlineData(new[]{3,2,1}, 438)]
        [InlineData(new[]{3,1,2}, 1836)]
        public void Example_2(int[] startingNumbers, int expectedNumberAtTurn2020)
        {
            var game = new ElfMemoryGame
            {
                StartingNumbers = startingNumbers
            };

            for (var i = 0; i < 2020; i++)
            {
                game.Speak();                
            }
            
            Assert.Equal(expectedNumberAtTurn2020, game.LastNumber);
        }

        [Fact(Skip = "done")]
        public void Puzzle_15A()
        {
            var game = new ElfMemoryGame
            {
                StartingNumbers = PuzzleInputs.Puzzle15
            };

            for (var i = 0; i < 2020; i++)
            {
                game.Speak();                
            }
            
            Assert.Equal(1259, game.LastNumber);
        }
        
        [Theory(Skip = "slow, but ok")]
        [InlineData(new[]{0,3,6}, 175594)]
        [InlineData(new[]{1,3,2}, 2578)]
        [InlineData(new[]{2,1,3}, 3544142)]
        [InlineData(new[]{1,2,3}, 261214)]
        [InlineData(new[]{2,3,1}, 6895259)]
        [InlineData(new[]{3,2,1}, 18)]
        [InlineData(new[]{3,1,2}, 362)]
        public void Example_3(int[] startingNumbers, int expectedNumberAtTurn30000000)
        {
            var game = new ElfMemoryGame
            {
                StartingNumbers = startingNumbers
            };

            for (var i = 0; i < 30000000; i++)
            {
                game.Speak();                
            }
            
            Assert.Equal(expectedNumberAtTurn30000000, game.LastNumber);
        }

        [Fact(Skip = "done")]
        public void Puzzle_15B()
        {
            var game = new ElfMemoryGame
            {
                StartingNumbers = PuzzleInputs.Puzzle15
            };

            for (var i = 0; i < 30000000; i++)
            {
                game.Speak();                
            }
            
            Assert.Equal(689, game.LastNumber);
        }
    }
}