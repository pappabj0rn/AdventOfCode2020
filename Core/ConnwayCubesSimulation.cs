using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Conway3dCubesSimulation
    {
        private const char Empty = '.';
        private const char Active = '#';
        
        public Dictionary<IntVector3, Cube> ActiveCubes { get; private set; } = new Dictionary<IntVector3, Cube>();
        
        public Conway3dCubesSimulation(IList<string> initialState)
        {
            var xSize = initialState[0].Length;
            var ySize = initialState.Count;
            initialState = initialState.Reverse().ToList();

            for (var y = 0; y < ySize; y++)
            {
                var row = initialState[y];
                for (var x = 0; x < xSize; x++)
                {
                    var c = row[x];
                    if (c != Active) 
                        continue;
                    
                    ActiveCubes.Add(new IntVector3(x,y,0),new Cube{Active = true});
                }
            }
        }

        public void Simulate()
        {
            var cubes = new Dictionary<IntVector3, Cube>(ActiveCubes);

            //Reset
            for (var i = 0; i < cubes.Keys.Count; i++)
            {
                var v = cubes.Keys.Skip(i).First();
                var value = cubes[v];
                value.ActiveNeighbours = 0;
                cubes[v] = value;
            }
            
            //Update
            foreach (var (origin, _) in ActiveCubes)
            {
                var neighbourVectors = GetNeighbourVectors(origin);
                
                foreach (var n in neighbourVectors)
                {
                    if (!cubes.ContainsKey(n))
                    {
                        cubes.Add(n, new Cube {ActiveNeighbours = 1});
                    }
                    else
                    {
                        var cube = cubes[n];
                        cube.ActiveNeighbours++;
                        cubes[n] = cube;
                    }
                }
            }

            //activate/de-active
            for (var i = 0; i < cubes.Keys.Count; i++)
            {
                var v = cubes.Keys.Skip(i).First();
                var cube = cubes[v];
                if (cube.Active)
                {
                    cube.Active = cube.ActiveNeighbours >= 2 && cube.ActiveNeighbours <= 3;
                }
                else
                {
                    cube.Active = cube.ActiveNeighbours == 3;
                }
                cubes[v] = cube;
            }

            ActiveCubes = cubes.Where(x=>x.Value.Active).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public string[] GetZImage(int z)
        {
            var activeVectorsAtZ = ActiveCubes
                .Where(x => x.Key.Z == z)
                .Select(x => x.Key)
                .ToList();

            var yMin = activeVectorsAtZ.OrderBy(x => x.Y).First().Y;
            var yMax = activeVectorsAtZ.OrderByDescending(x => x.Y).First().Y;
            var xMin = activeVectorsAtZ.OrderBy(x => x.X).First().X;
            var xMax = activeVectorsAtZ.OrderByDescending(x => x.X).First().X;

            var img = new List<string>();
            
            for (var y = yMax; y >= yMin; y--)
            {
                var chars = new List<char>();
                for (var x = xMin; x <= xMax; x++)
                {
                    chars.Add(ActiveCubes.ContainsKey(new IntVector3(x, y, z))
                        ? Active
                        : Empty);
                }
                img.Add(new string(chars.ToArray()));
            }
            
            return img.ToArray();
        }

        private IEnumerable<IntVector3> GetNeighbourVectors(IntVector3 origin)
        {
            for (var z = -1; z <= 1; z++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        if(x==0 && y==0 && z==0)
                            continue;
                            
                        yield return new IntVector3(x, y, z) + origin;
                    }
                }                
            }
        }
    }
    
    public class Conway4dCubesSimulation
    {
        private const char Empty = '.';
        private const char Active = '#';
        
        public Dictionary<IntVector4, Cube> ActiveCubes { get; private set; } = new Dictionary<IntVector4, Cube>();
        
        public Conway4dCubesSimulation(IList<string> initialState)
        {
            var xSize = initialState[0].Length;
            var ySize = initialState.Count;
            initialState = initialState.Reverse().ToList();

            for (var y = 0; y < ySize; y++)
            {
                var row = initialState[y];
                for (var x = 0; x < xSize; x++)
                {
                    var c = row[x];
                    if (c != Active) 
                        continue;
                    
                    ActiveCubes.Add(new IntVector4(x,y,0,0),new Cube{Active = true});
                }
            }
        }

        public void Simulate()
        {
            var cubes = new Dictionary<IntVector4, Cube>(ActiveCubes);

            //Reset
            for (var i = 0; i < cubes.Keys.Count; i++)
            {
                var v = cubes.Keys.Skip(i).First();
                var value = cubes[v];
                value.ActiveNeighbours = 0;
                cubes[v] = value;
            }
            
            //Update
            foreach (var (origin, _) in ActiveCubes)
            {
                var neighbourVectors = GetNeighbourVectors(origin);
                
                foreach (var n in neighbourVectors)
                {
                    if (!cubes.ContainsKey(n))
                    {
                        cubes.Add(n, new Cube {ActiveNeighbours = 1});
                    }
                    else
                    {
                        var cube = cubes[n];
                        cube.ActiveNeighbours++;
                        cubes[n] = cube;
                    }
                }
            }

            //activate/de-active
            for (var i = 0; i < cubes.Keys.Count; i++)
            {
                var v = cubes.Keys.Skip(i).First();
                var cube = cubes[v];
                if (cube.Active)
                {
                    cube.Active = cube.ActiveNeighbours >= 2 && cube.ActiveNeighbours <= 3;
                }
                else
                {
                    cube.Active = cube.ActiveNeighbours == 3;
                }
                cubes[v] = cube;
            }

            ActiveCubes = cubes.Where(x=>x.Value.Active).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private IEnumerable<IntVector4> GetNeighbourVectors(IntVector4 origin)
        {
            for (var w = -1; w <= 1; w++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        for (var x = -1; x <= 1; x++)
                        {
                            if(x==0 && y==0 && z==0 && w==0)
                                continue;
                            
                            yield return new IntVector4(x, y, z, w) + origin;
                        }
                    }                
                }
            }
        }
    }

    public struct Cube
    {
        public bool Active { get; set; }
        public int ActiveNeighbours { get; set; }
    }
}