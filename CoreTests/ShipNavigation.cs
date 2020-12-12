using Core;
using Xunit;

namespace CoreTests
{
    public abstract class WaypointTests
    {
        public class Move
        {
            [Theory]
            [InlineData("N10", 0, -10)]
            [InlineData("S10", 0, 10)]
            [InlineData("E10", 10, 0)]
            [InlineData("W10", -10, 0)]
            public void Should_move_in_specified_directions(
                string order, 
                int expectedX,
                int expectedY)
            {
                var wp = new Waypoint(0, 0);
                wp.Move(order);
                
                Assert.Equal(expectedX, wp.Position.X);
                Assert.Equal(expectedY, wp.Position.Y);
            }
        }

        public class Rotate
        {
            [Theory]
            [InlineData(new[]{"E3","N2"}, "R90", 2, 3)]
            [InlineData(new[]{"E3","N2"}, "R180", -3, 2)]
            [InlineData(new[]{"E3","N2"}, "R270", -2, -3)]
            [InlineData(new[]{"E3","N2"}, "L90", -2, -3)]
            [InlineData(new[]{"E3","N2"}, "L180", -3, 2)]
            [InlineData(new[]{"E3","N2"}, "L270", 2, 3)]
            public void Should_rotate_waypoint_around_origin(
                string[] moves,
                string rotation, 
                int expectedX,
                int expectedY)
            {
                var wp = new Waypoint(0,0);
                foreach (var move in moves)
                {
                    wp.Move(move);
                }
                
                wp.Rotate(rotation);
                
                Assert.Equal(expectedX, wp.Position.X);
                Assert.Equal(expectedY, wp.Position.Y);
            }
        }
    }
    public abstract class ShipNavigationTests
    {
        public class Move
        {
            [Theory]
            [InlineData("N0", 10, -1)]
            [InlineData("N10", 10, -11)]
            [InlineData("S10", 10, 9)]
            [InlineData("E10", 20, -1)]
            [InlineData("W10", 0, -1)]
            public void Should_move_waypoint_in_specified_directions(
                string order, 
                int expectedX,
                int expectedY)
            {
                var nav = new ShipNavigation();

                nav.Move(order);
                
                Assert.Equal(expectedX, nav.Waypoint.Position.X);
                Assert.Equal(expectedY, nav.Waypoint.Position.Y);
            }
            
            [Theory]
            [InlineData(new[]{"F1"}, 10, -1)]
            [InlineData(new[]{"F10"}, 100, -10)]
            [InlineData(new[]{"S1","F1"}, 10, 0)]
            [InlineData(new[]{"S1","F10"}, 100, 0)]
            public void Should_move_forward_according_to_current_waypoint_direction(
                string[] orders, 
                int expectedX, 
                int expectedY)
            {
                var nav = new ShipNavigation();

                foreach (var order in orders)
                {
                    nav.Move(order);
                }
                
                Assert.Equal(expectedX, nav.Position.X);
                Assert.Equal(expectedY, nav.Position.Y);
            }
            
            [Fact]
            public void Example_2()
            {
                var orders = new[]
                {
                    "F10", "N3", "F7", "R90", "F11"
                };
                
                var nav = new ShipNavigation();

                foreach (var order in orders)
                {
                    nav.Move(order);
                }
                
                Assert.Equal(286, nav.ManhattanDistance);
            }

            [Fact(Skip="Done")]
            public void Puzzle_12A()
            {
                var nav = new ShipNavigation();

                foreach (var order in PuzzleInputs.Puzzle12)
                {
                    nav.Move(order);
                }
                
                Assert.Equal(381, nav.ManhattanDistance);
            }
            
            [Fact(Skip="Done")]
            public void Puzzle_12B()
            {
                var nav = new ShipNavigation();

                foreach (var order in PuzzleInputs.Puzzle12)
                {
                    nav.Move(order);
                }
                
                Assert.Equal(28591, nav.ManhattanDistance);
            }
        }
    }
}