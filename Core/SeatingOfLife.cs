using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SimulationRules
    {
        public int CrowdedLimit { get; set; } = 4;
        public bool ExtendedReach { get; set; }
    }
    
    public class SeatingOfLife
    {
        private const char Occupied = '#';
        private const char Free = 'L';
        private const char Floor = '.';
        private const char Border = 'X';

        public char[][] Field { get; set; }
        public SimulationRules Rules { get; set; } = new SimulationRules();
        
        public bool Simulate()
        {
            var changed = false;

            var changes = new List<(int x, int y, char state)>();
            
            for (var y = 0; y < Field.Length; y++)
            {
                for (var x = 0; x < Field[y].Length; x++)
                {
                    var curSpot = Field[y][x];
                    
                    if(curSpot == Floor)
                        continue;
                    
                    var neighbours = GetNeighbours(x, y);
                    var occupiedNeighbours = neighbours.Count(c => c == Occupied);
                    
                    switch (curSpot)
                    {
                        case Free when occupiedNeighbours == 0:
                            changes.Add((x,y,Occupied));
                            changed = true;
                            break;
                        case Occupied when occupiedNeighbours >= Rules.CrowdedLimit:
                            changes.Add((x,y,Free));
                            changed = true;
                            break;
                    }
                }
            }

            if (!changed) 
                return false;
            
            foreach (var change in changes)
            {
                Field[change.y][change.x] = change.state;
            }

            return true;
        }

        private char[] GetNeighbours(in int coordX, in int coordY)
        {
            var directions = new List<IntVector2>
            {
                new IntVector2(-1,-1),
                new IntVector2(0,-1),
                new IntVector2(1,-1),
                
                new IntVector2(-1,0),
                new IntVector2(1,0),
                
                new IntVector2(-1,1),
                new IntVector2(0,1),
                new IntVector2(1,1)
            };
            
            var neighbours = directions
                .ToDictionary<IntVector2, IntVector2, char?>(direction => direction, direction => null);

            var amp = 0;
            while (neighbours.Values.Contains(null))
            {
                amp++;
                foreach (var dir in neighbours.Where(n=>n.Value == null).Select(n=>n.Key).ToList())
                {
                    var x = coordX + dir.X * amp;
                    var y = coordY + dir.Y * amp;
                    
                    try
                    {
                        var c = Field[y][x];
                        if(Rules.ExtendedReach 
                           && c == Floor)
                            continue;
                        
                        neighbours[dir] = c;
                    }
                    catch (Exception)
                    {
                        neighbours[dir] = Border;
                    }
                }
            }

            return neighbours.Values.Select(c=>c.Value).ToArray();
        }
    }
}