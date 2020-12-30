using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public class CrabCombatTests
    {
        [Fact]
        public void Example_1()
        {
            var cc = new CrabCombat
            {
                P1 = new Queue<int>( new [] { 9, 2, 6, 3, 1 }),
                P2 = new Queue<int>( new [] { 5, 8, 4, 7, 10 })
            };

            cc.Battle();
            
            Assert.Equal(29, cc.RoundsPlayed);
            Assert.Equal(0, cc.P1Score);
            Assert.Equal(306, cc.P2Score);
        }
        
        [Fact]
        public void Puzzle_22A()
        {
            var cc = new CrabCombat
            {
                P1 = new Queue<int>(PuzzleP1),
                P2 = new Queue<int>(PuzzleP2)
            };

            cc.Battle();
            
            Assert.Equal(687, cc.RoundsPlayed);
            Assert.Equal(33098, cc.P1Score);
            Assert.Equal(0, cc.P2Score);
        }

        [Fact]
        public void Compare_array_test()
        {
            var a1 = new[] {1, 2};
            var a2 = new[] {1, 2};
            
            Assert.Equal(a1, a2);
            Assert.True(a1.SequenceEqual(a2));
        }
        
        [Fact]
        public void Example_2()
        {
            var cc = new CrabCombat
            {
                P1 = new Queue<int>( new [] { 9, 2, 6, 3, 1 }),
                P2 = new Queue<int>( new [] { 5, 8, 4, 7, 10 })
            };

            cc.BattleRecursive(cc.P1, cc.P2);
            
            Assert.Equal(0, cc.P1Score);
            Assert.Equal(291, cc.P2Score);
        }
        
        [Fact]
        public void Example_3()
        {
            var cc = new CrabCombat
            {
                P1 = new Queue<int>( new [] { 43,19 }),
                P2 = new Queue<int>( new [] { 2,29,14 })
            };

            cc.BattleRecursive(cc.P1, cc.P2);
            
            Assert.Equal(105, cc.P1Score);
            Assert.Equal(0, cc.P2Score);
        }
        
        [Fact(Skip = "done")]
        public void Puzzle_22B()
        {
            var cc = new CrabCombat
            {
                P1 = new Queue<int>(PuzzleP1),
                P2 = new Queue<int>(PuzzleP2)
            };

            cc.BattleRecursive(cc.P1, cc.P2);
            
            Assert.True(cc.P1Score>11908,"too low");
            Assert.True(cc.P1Score>15110,"too low");
            Assert.Equal(35055, cc.P1Score);
            Assert.Equal(0, cc.P2Score);
        }
        
        private static readonly int[] PuzzleP1 = {
            19,
            5,
            35,
            6,
            12,
            22,
            45,
            39,
            14,
            42,
            47,
            38,
            2,
            26,
            13,
            30,
            4,
            34,
            43,
            40,
            16,
            8,
            23,
            50,
            36
        };
        
        private static readonly int[] PuzzleP2 = {
            1,
            21,
            29,
            41,
            32,
            28,
            9,
            37,
            49,
            20,
            17,
            27,
            24,
            3,
            33,
            44,
            48,
            31,
            15,
            25,
            18,
            46,
            7,
            10,
            11
        };
    }
}