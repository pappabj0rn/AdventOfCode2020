using System;

namespace Core
{
    public class ShipNavigation
    {
        public IntVector2 Position { get; private set; }
        public Waypoint Waypoint { get; set; } = new Waypoint(10, -1);

        public int ManhattanDistance => 
            Math.Abs(Position.X) + Math.Abs(Position.Y);

        public void Move(string order)
        {
            switch (order[0..1])
            {
                case "N":
                case "E":
                case "S":
                case "W":
                    Waypoint.Move(order);
                    break;
                case "R":
                case "L":
                    Waypoint.Rotate(order);
                    break;
                case "F":
                    MoveForward(order);
                    break;
            }
        }

        private void MoveForward(string order)
        {
            var length = int.Parse(order[1..]);

            for (var i = 0; i < length; i++)
            {
                Position += Waypoint.Position;                
            }
        }

        private void MoveInDirection(string order)
        {
            var x = order[0..1] switch
            {
                "E" => int.Parse(order[1..]),
                "W" => -int.Parse(order[1..]),
                _ => 0
            };
            
            var y = order[0..1] switch
            {
                "N" => -int.Parse(order[1..]),
                "S" => int.Parse(order[1..]),
                _ => 0
            };

            Position += new IntVector2(x, y);
        }
    }

    public class Waypoint
    {
        public Waypoint(int x, int y)
        {
            Position = new IntVector2(x, y);
        }

        public IntVector2 Position { get; private set; }

        public void Move(string order)
        {
            var x = order[0..1] switch
            {
                "E" => int.Parse(order[1..]),
                "W" => -int.Parse(order[1..]),
                _ => 0
            };
            
            var y = order[0..1] switch
            {
                "N" => -int.Parse(order[1..]),
                "S" => int.Parse(order[1..]),
                _ => 0
            };

            Position += new IntVector2(x, y);
        }

        public void Rotate(string order)
        {
            var rotation = order switch
            {
                "R90" => 1,
                "R180" => 2,
                "R270" => 3,
                "L90" => 3,
                "L180" => 2,
                "L270" => 1,
                _ => 0
            };
            
            var switchComponents = rotation % 2 == 1;

            var components = CreateDirectionComponents();

            foreach (var c in components)
            {
                var dir = (int) c.Direction;
                dir += rotation;
                c.Direction = (Direction) (dir % 4);
            }

            Position = BuildVector(switchComponents, components);
        }

        private static IntVector2 BuildVector(bool switchComponents, DirectionComponent[] components)
        {
            var x = switchComponents
                ? components[1]
                : components[0];
            
            var y = switchComponents
                ? components[0]
                : components[1];
            
            return new IntVector2(
                x.Direction == Direction.East
                    ? x.Length
                    : -x.Length,
                y.Direction == Direction.South
                    ? y.Length
                    : -y.Length);
        }

        private DirectionComponent[] CreateDirectionComponents()
        {
            return new[]
            {
                new DirectionComponent(
                    Position.X > 0
                        ? Direction.East
                        : Direction.West,
                    Position.X),
                
                new DirectionComponent(
                    Position.Y > 0
                        ? Direction.South
                        : Direction.North,
                    Position.Y)
            };
        }
    }

    public class DirectionComponent
    {
        public Direction Direction { get; set; }
        public int Length { get; private set; }
        
        public DirectionComponent(
            in Direction direction, 
            in int length)
        {
            Direction = direction;
            Length = Math.Abs(length);
        }
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}