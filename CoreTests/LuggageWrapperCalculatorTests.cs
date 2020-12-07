using Core;
using Xunit;

namespace CoreTests
{
    public abstract class LuggageWrapperCalculatorTests
    {
        public class FindWrapperVariationsFor
        {
            [Fact]
            public void Should_find_direct_containers_for_target_bag()
            {
                var rules = new[]
                {
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.FindWrapperVariationsFor(target);
                
                Assert.Equal(2, variations);
            }
            
            [Fact]
            public void Should_find_direct_and_second_level_wrappers()
            {
                var rules = new[]
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.", "dotted black bags contain no other bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.FindWrapperVariationsFor(target);
                
                Assert.Equal(4, variations);
            }
            
            
            [Fact]
            public void Should_find_direct_second_and_third_level_wrappers()
            {
                var rules = new[]
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    
                    "vibrant olive bags contain 3 faded blue bags, 4 light red bags.",
                    
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.", "dotted black bags contain no other bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.FindWrapperVariationsFor(target);
                
                Assert.Equal(5, variations);
            }
            
            
            [Fact]
            public void Should_skip_already_counted_targets()
            {
                var rules = new[]
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 light red bags.",
                    "vibrant olive bags contain 3 faded blue bags, 4 light red bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.", "dotted black bags contain no other bags."
                };
                
                var target = "shiny gold";
                
                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };

                AssertAsync.CompletesIn(3, () =>
                {
                    var variations = calc.FindWrapperVariationsFor(target);
                
                    Assert.Equal(5, variations);
                });
            }

            [Fact]
            public void Puzzle_7A()
            {
                var target = "shiny gold";
                
                var calc = new LuggageWrapperCalculator
                {
                    Rules = PuzzleInputs.Puzzle7
                };

                AssertAsync.CompletesIn(10, () =>
                {
                    var variations = calc.FindWrapperVariationsFor(target);
                
                    Assert.True(450 > variations, "Should be less than 450");
                    Assert.True(373 > variations, "Should be less than 373");
                    Assert.True(327 > variations, "Should be less than 327");
                    Assert.Equal(326, variations);
                });
            }
        }

        public class CountBagTotalTests
        {
            [Fact]
            public void Should_count_target()
            {
                var rules = new[]
                {
                    "shiny gold bags contain no other bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.CountBagTotal(target);
                
                Assert.Equal(1, variations);
            }
            
            [Fact]
            public void Should_count_target_and_subbags_1()
            {
                var rules = new[]
                {
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.", //t + (1 +1(0)) + (2+ 2(0))
                    "dark olive bags contain no other bags.",
                    "vibrant plum bags contain no other bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.CountBagTotal(target);
                
                Assert.Equal(4, variations);
            }
            
            [Fact]
            public void Should_count_target_and_subbags_2()
            {
                var rules = new[]
                {
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.", //t + (1 +1(2)) + (2+ 2(0))
                    "dark olive bags contain 2 vibrant plum bags.",
                    "vibrant plum bags contain no other bags."
                };
                
                var target = "shiny gold";

                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var variations = calc.CountBagTotal(target);
                
                Assert.Equal(6, variations);
            }

            [Fact]
            public void Should_match_example_1()
            {
                var rules = new[]
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.",
                    "dotted black bags contain no other bags."
                };
                
                var target = "shiny gold";
                
                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var count = calc.CountBagTotal(target)-1;
                Assert.Equal(32, count);
            }

            [Fact]
            public void Should_match_example_2()
            {
                var rules = new[]
                {
                    "shiny gold bags contain 2 dark red bags.",
                    "dark red bags contain 2 dark orange bags.",
                    "dark orange bags contain 2 dark yellow bags.",
                    "dark yellow bags contain 2 dark green bags.",
                    "dark green bags contain 2 dark blue bags.",
                    "dark blue bags contain 2 dark violet bags.",
                    "dark violet bags contain no other bags."
                };
                
                var target = "shiny gold";
                
                var calc = new LuggageWrapperCalculator
                {
                    Rules = rules
                };
                
                var count = calc.CountBagTotal(target)-1;
                Assert.Equal(126, count);
            }
            
            [Fact]
            public void Puzzle_7B()
            {
                var target = "shiny gold";
                
                var calc = new LuggageWrapperCalculator
                {
                    Rules = PuzzleInputs.Puzzle7
                };

                AssertAsync.CompletesIn(10, () =>
                {
                    var count = calc.CountBagTotal(target)-1;
                    Assert.True(5636 > count, "Should be less than 5636");
                    Assert.Equal(5635, count);
                });
            }
        }
    }
}