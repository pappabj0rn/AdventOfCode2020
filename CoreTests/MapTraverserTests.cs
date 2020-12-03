using System;
using System.Collections.Generic;
using Core;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;

namespace CoreTests
{
    public abstract class MapTraverserTests
    {
        public class CountTreesForDescentVector
        {
            [Fact]
            public void Should_match_example()
            {
                var mt = new MapTraverser
                {
                    MapSegment = new[]
                    {
                        "..##.......",
                        "#...#...#..",
                        ".#....#..#.",
                        "..#.#...#.#",
                        ".#...##..#.",
                        "..#.##.....",
                        ".#.#.#....#",
                        ".#........#",
                        "#.##...#...",
                        "#...##....#",
                        ".#..#...#.#"
                    }
                };

                var count = mt.CountTreesForDescentVector(new IntVector2 {X = 3, Y = 1});

                Assert.Equal(7, count);
            }
            
            [Theory]
            [InlineData(1,1,2)]
            [InlineData(3,1,7)]
            [InlineData(5,1,3)]
            [InlineData(7,1,4)]
            [InlineData(1,2,2)]
             public void Should_match_example_b(int x, int y, int expected)
             {
                 var mt = new MapTraverser
                 {
                     MapSegment = new[]
                     {
                         "..##.......",
                         "#...#...#..",
                         ".#....#..#.",
                         "..#.#...#.#",
                         ".#...##..#.",
                         "..#.##.....",
                         ".#.#.#....#",
                         ".#........#",
                         "#.##...#...",
                         "#...##....#",
                         ".#..#...#.#"
                     }
                 };
 
                 var count = mt.CountTreesForDescentVector(new IntVector2 {X = x, Y = y});

                 Assert.Equal(expected, count);
             }
            
            [Theory]
            [InlineData(1,1,50)]
            [InlineData(3,1,148)]
            [InlineData(5,1,53)]
            [InlineData(7,1,64)]
            [InlineData(1,2,29)]
            public void Puzzle_3A(int x, int y, int expected)
            {
                var mt = new MapTraverser
                {
                    MapSegment = PuzzleInputs.Puzzle3
                };

                var count = mt.CountTreesForDescentVector(new IntVector2 {X = x, Y = y});

                Assert.Equal(expected, count);
            }
        }
    }
}