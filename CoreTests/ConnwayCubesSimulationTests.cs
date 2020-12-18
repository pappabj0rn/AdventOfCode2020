using Core;
using Xunit;

namespace CoreTests
{
    public class Conway4dCubesSimulationTests
    {
        [Fact]
        public void Example_1()
        {
            var initialState = new[]
            {
                ".#.",
                "..#",
                "###"
            };

            var simulation = new Conway4dCubesSimulation(initialState);

            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();

            Assert.Equal(848, simulation.ActiveCubes.Count);
        }

        [Fact (Skip = "done")]
        public void Puzzle_17B()
        {
            var initialState = new[]
            {
                "..##.#.#",
                ".#####..",
                "#.....##",
                "##.##.#.",
                "..#...#.",
                ".#..##..",
                ".#...#.#",
                "#..##.##"
            };

            var simulation = new Conway4dCubesSimulation(initialState);

            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            Assert.Equal(1624, simulation.ActiveCubes.Count);
        }
    }
    public class Conway3dCubesSimulationTests
    {
        [Fact]
        public void Example_1()
        {
            var initialState = new[]
            {
                ".#.",
                "..#",
                "###"
            };

            var simulation = new Conway3dCubesSimulation(initialState);

            simulation.Simulate();
            var expZ0AtC1 = new[]
            {
                "#.#",
                ".##",
                ".#."
            };
            Assert.Equal(expZ0AtC1, simulation.GetZImage(0));
            Assert.Equal(11, simulation.ActiveCubes.Count);

            simulation.Simulate();
            Assert.Equal(21, simulation.ActiveCubes.Count);

            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();

            Assert.Equal(112, simulation.ActiveCubes.Count);
        }

        [Fact]
        public void Simplified_test_1()
        {
            var initialState = new[]
            {
                "...",
                "###",
                "..."
            };

            var simulation = new Conway3dCubesSimulation(initialState);

            simulation.Simulate();
            var expZ0AtC1 = new[]
            {
                "#",
                "#",
                "#"
            };
            Assert.Equal(expZ0AtC1, simulation.GetZImage(0));
        }

        [Fact (Skip = "done")]
        public void Puzzle_17A()
        {
            var initialState = new[]
            {
                "..##.#.#",
                ".#####..",
                "#.....##",
                "##.##.#.",
                "..#...#.",
                ".#..##..",
                ".#...#.#",
                "#..##.##"
            };

            var simulation = new Conway3dCubesSimulation(initialState);

            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            simulation.Simulate();
            Assert.Equal(213, simulation.ActiveCubes.Count);
        }

        [Fact]
        public void GetZImage_should_return_initial_state_given_to_constructor_after_constructing_simulation()
        {
            var initialState = new[]
            {
                ".#.",
                "..#",
                "###"
            };

            var simulation = new Conway3dCubesSimulation(initialState);

            var zImage0 = simulation.GetZImage(0);

            Assert.Equal(initialState, zImage0);
        }
    }
}