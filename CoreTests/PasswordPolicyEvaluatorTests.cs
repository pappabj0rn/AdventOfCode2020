using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class PasswordPolicyTests
    {
        public class Evaluate
        {
            [Theory]
            [InlineData(1,3,'a',"abcde",true)]
            [InlineData(1,3,'b',"cdefg",false)]
            [InlineData(2,9,'c',"ccccccccc",true)]
            public void Should_evaluate_according_to_sample_data(
                int min, 
                int max,
                char @char,
                string input,
                bool expected)
            {
                var policy = new PasswordPolicy
                {
                    Min = min,
                    Max = max,
                    Char = @char
                };

                var result = policy.Evaluate(input);

                Assert.Equal(expected,result);
            }

            [Fact(Skip = "done")]
            public void Puzzle_2A()
            {
                var inputs = PuzzleInputs.Puzzle2;

                var okPasswords = new ConcurrentBag<int>();
                
                Parallel.ForEach(inputs, input =>
                {
                    var parts = input.Split(':');
                    var policy = PasswordPolicy.Parse(parts[0]);
                    if (policy.Evaluate(parts[1].Trim()))
                        okPasswords.Add(1);
                });
                
                Assert.Equal(0,okPasswords.Sum());
            }
        }

        public class Parse
        {
            [Theory]
            [InlineData("1-3 a", 1, 3, 'a')]
            [InlineData("1-3 b", 1, 3, 'b')]
            [InlineData("2-9 c", 2, 9, 'c')]
            [InlineData("16-17 d", 16, 17, 'd')]
            public void Should_parse_example_policies(
                string policyString,
                int expectedMin,
                int expectedMax,
                char expectedChar)
            {
                var policy = PasswordPolicy.Parse(policyString);
                
                Assert.Equal(expectedMin,policy.Min);
                Assert.Equal(expectedMax,policy.Max);
                Assert.Equal(expectedChar,policy.Char);
            }
        }
    }

    public abstract class PasswordPolicy2Tests
    {
        public class Parse
        {
            [Theory]
            [InlineData("1-3 a", 0, 2, 'a')]
            [InlineData("1-3 b", 0, 2, 'b')]
            [InlineData("2-9 c", 1, 8, 'c')]
            [InlineData("16-17 d", 15, 16, 'd')]
            public void Should_parse_example_policies(
                string policyString,
                int expectedPos1,
                int expectedPos2,
                char expectedChar)
            {
                var policy = PasswordPolicy2.Parse(policyString);
                
                Assert.Equal(expectedPos1,policy.Pos1);
                Assert.Equal(expectedPos2,policy.Pos2);
                Assert.Equal(expectedChar,policy.Char);
            }
        }

        public class Evaluate
        {
            [Theory]
            [InlineData(0,2,'a',"abcde",true)]
            [InlineData(0,2,'b',"cdefg",false)]
            [InlineData(1,8,'c',"ccccccccc",false)]
            public void Should_evaluate_according_to_sample_data(
                int pos1, 
                int pos2,
                char @char,
                string input,
                bool expected)
            {
                var policy = new PasswordPolicy2
                {
                    Pos1 = pos1,
                    Pos2 = pos2,
                    Char = @char
                };

                var result = policy.Evaluate(input);

                Assert.Equal(expected,result);
            }
            
            [Fact(Skip ="done")]
            public void Puzzle_2B()
            {
                var inputs = PuzzleInputs.Puzzle2;

                var okPasswords = new ConcurrentBag<int>();
                
                Parallel.ForEach(inputs, input =>
                {
                    var parts = input.Split(':');
                    var policy = PasswordPolicy2.Parse(parts[0]);
                    if (policy.Evaluate(parts[1].Trim()))
                        okPasswords.Add(1);
                });
                
                Assert.Equal(0,okPasswords.Sum());
            }

            
        }
    }
}