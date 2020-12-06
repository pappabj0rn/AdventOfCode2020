using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class AnswerCalculatorTests
    {
        public class CountUnique
        {
            [Fact]
            public void Should_ignore_duplicates_in_data()
            {
                var input = new[] {"abc", "abc"};
                var calc = new AnswerCalculataor();
                var result = calc.CountUnique(input);
                
                Assert.Equal(3, result);
            }
            
            [Fact]
            public void Should_ignore_duplicates_in_data_2()
            {
                var input = new[] {"abcd", "abc"};
                var calc = new AnswerCalculataor();
                var result = calc.CountUnique(input);
                
                Assert.Equal(4, result);
            }
            
            [Fact]
            public void Should_ignore_duplicates_in_data_3()
            {
                var input = new[] {"abc", "abcz"};
                var calc = new AnswerCalculataor();
                var result = calc.CountUnique(input);
                
                Assert.Equal(4, result);
            }
            
            [Fact]
            public void Should_ignore_duplicates_in_data_4()
            {
                var input = new[] {"a", "b","cd","d","ab"};
                var calc = new AnswerCalculataor();
                var result = calc.CountUnique(input);
                
                Assert.Equal(4, result);
            }
        }

        public class FInGroups
        {
            [Fact]
            public void Should_group_input_by_line()
            {
                var input = new[] {"a", "a","","d","ab","c"};
                var calc = new AnswerCalculataor();
                var result = calc.FInGroups(input, calc.CountUnique);
                
                Assert.Equal(2, result.Count());
                Assert.Equal(1, result.First());
                Assert.Equal(4, result.Skip(1).First());
            }

            [Fact]
            public void Example_sum()
            {
                var input = new[]
                {
                    "abc", 
                    "", 
                    "a", "b", "c", 
                    "", 
                    "ab", "ac", 
                    "", 
                    "a", "a", "a", "a", 
                    "", 
                    "b"
                };
                
                var calc = new AnswerCalculataor();
                var result = calc.FInGroups(input, calc.CountUnique);
                Assert.Equal(5, result.Count());
                Assert.Equal(11, result.Sum());
            }

            [Fact]
            public void Puzzle_6A()
            {
                var calc = new AnswerCalculataor();
                var result = calc.FInGroups(PuzzleInputs.Puzzle6, calc.CountUnique);
                Assert.Equal(6587, result.Sum());
            }
        }

        
        public class CountRecurring
        {
            [Theory]
            [InlineData(new[] {"abc"}, 3)]
            [InlineData(new[] {"a","b","c"}, 0)]
            [InlineData(new[] {"ab","ac"}, 1)]
            [InlineData(new[] {"a","a","a","a"}, 1)]
            [InlineData(new[] {"b"}, 1)]
            public void Should_count_chars_found_in_every_line(IEnumerable<string> input, int recurring)
            {
                var calc = new AnswerCalculataor();
                var result = calc.CountRecurring(input);
                
                Assert.Equal(recurring, result);
            }

            [Fact]
            public void Puzzle_6B()
            {
                var calc = new AnswerCalculataor();
                var result = calc.FInGroups(PuzzleInputs.Puzzle6, calc.CountRecurring);
                Assert.Equal(3235, result.Sum());
            }
        }
    }
}