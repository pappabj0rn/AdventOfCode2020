using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public abstract class SeatingOfLifeTests
    {
        public class Simulate : SeatingCalculatorTests
        {
            [Theory]
            [InlineData(
                new[]
                {
                    "#.#L.L#.##",
                    "#LLL#LL.L#",
                    "L.#.L..#..",
                    "#L##.##.L#",
                    "#.#L.LL.LL",
                    "#.#L#L#.##",
                    "..L.L.....",
                    "#L#L##L#L#",
                    "#.LLLLLL.L",
                    "#.#L#L#.##"
                },
                false)]
            public void Should_return_false_when_simulating_a_stable_field(string[] input, bool _)
            {
                var sim = new SeatingOfLife {Field = input.Select(s => s.ToArray()).ToArray()};

                var changed = sim.Simulate();

                Assert.False(changed);
                var occupiedCount = sim.Field.SelectMany(x => x).Count(x => x == '#');
                Assert.Equal(37,occupiedCount);
            }

            [Theory]
            [InlineData(new[] {"L"}, true)]
            public void Should_return_true_when_simulation_changes_field(
                string[] input, bool expected)
            {
                var sim = new SeatingOfLife {Field = input.Select(s => s.ToArray()).ToArray()};

                var changed = sim.Simulate();

                Assert.Equal(expected, changed);
            }

            [Theory]
            [InlineData(new[] {"L"}, new[] {"#"})]
            [InlineData(new[] {"L."}, new[] {"#."})]
            [InlineData(new[]
            {
                "##.",
                "#..",
                "##."
            }, new[]
            {
                "##.",
                "L..",
                "##."
            })]
            [InlineData(new[]
            {
                "##.",
                "L..",
                "##."
            }, new[]
            {
                "##.",
                "L..",
                "##."
            })]
            public void Should_set_all_free_seats_having_less_than_4_occupied_adjacent_neighbours_to_occupied(
                string[] input,
                string[] expected)
            {
                var sim = new SeatingOfLife {Field = input.Select(s => s.ToArray()).ToArray()};
                sim.Simulate();

                var expectedField = expected.Select(s => s.ToArray()).ToArray();

                Assert.Equal(expectedField, sim.Field);
            }

            [Theory]
            [InlineData(
                new[]
                {
                    "L.LL.LL.LL",
                    "LLLLLLL.LL",
                    "L.L.L..L..",
                    "LLLL.LL.LL",
                    "L.LL.LL.LL",
                    "L.LLLLL.LL",
                    "..L.L.....",
                    "LLLLLLLLLL",
                    "L.LLLLLL.L",
                    "L.LLLLL.LL"
                },
                false)]
            public void Example_1(string[] input, bool _)
            {
                var sim = new SeatingOfLife {Field = input.Select(s => s.ToArray()).ToArray()};

                while (sim.Simulate()) { }

                var occupiedCount = sim.Field.SelectMany(x => x).Count(x => x == '#');
                Assert.Equal(37,occupiedCount);
            }

            [Fact(Skip = "done")]
            public void Puzzle_11A()
            {
                var sim = new SeatingOfLife {Field = PuzzleInputs.Puzzle11.Select(s => s.ToArray()).ToArray()};

                while (sim.Simulate()) { }

                var occupiedCount = sim.Field.SelectMany(x => x).Count(x => x == '#');
                Assert.Equal(2338,occupiedCount);
            }
            
            [Theory]
            [InlineData(
                    "L.LL.LL.LL",
                    "LLLLLLL.LL",
                    "L.L.L..L..",
                    "LLLL.LL.LL",
                    "L.LL.LL.LL",
                    "L.LLLLL.LL",
                    "..L.L.....",
                    "LLLLLLLLLL",
                    "L.LLLLLL.L",
                    "L.LLLLL.LL")]
            public void Example_2(params string[] input)
            {
                var sim = new SeatingOfLife
                {
                    Field = input.Select(s => s.ToArray()).ToArray(),
                    Rules =
                    {
                        CrowdedLimit = 5, 
                        ExtendedReach = true
                    }
                };

                while (sim.Simulate()) { }

                var occupiedCount = sim.Field.SelectMany(x => x).Count(x => x == '#');
                Assert.Equal(26,occupiedCount);
            }
            
            [Fact(Skip="Done")]
            public void Puzzle_11B()
            {
                var sim = new SeatingOfLife
                {
                    Field = PuzzleInputs.Puzzle11.Select(s => s.ToArray()).ToArray(),
                    Rules = {CrowdedLimit = 5, ExtendedReach = true}
                };

                while (sim.Simulate()) { }

                var occupiedCount = sim.Field.SelectMany(x => x).Count(x => x == '#');
                Assert.Equal(2134,occupiedCount);
            }
        }
    }
}