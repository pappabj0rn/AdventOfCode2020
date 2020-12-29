using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core
{
    public class JigsawAssembler
    {
        public List<Tile> Tiles { get; } 
            = new List<Tile>();
        
        public Dictionary<int, List<TileEdge>> TileEdgeIndex { get; } 
            = new Dictionary<int, List<TileEdge>>();

        public Tile[,] Layout { get; set; }
        
        public void LoadTiles(string[] data)
        {
            Tile temp = null;
            foreach (var line in data)
            {
                if (line.StartsWith("Tile"))
                {
                    if(temp != null)
                        Tiles.Add(temp);
                    
                    temp = new Tile
                    {
                        Id = int.Parse(line[5..^1])
                    };
                    continue;
                }

                if (!line.IsNullOrEmpty())
                {
                    temp.Data.Add(line.ToCharArray());
                }
            }
            
            if(temp != null)
                Tiles.Add(temp);
        }

        public void IndexTileEdges()
        {
            foreach (var tile in Tiles)
            {
                IndexTileEdges(tile);
            }
        }

        private void IndexTileEdges(Tile tile)
        {
            for (var e = 0; e < 4; e++)
            {
                for (var f = 0; f < 2; f++)
                {
                    var flipped = f == 1;
                    var edge = (Edge) e;

                    var n = tile.GetEdgeValue(edge, flipped);

                    if (!TileEdgeIndex.ContainsKey(n))
                        TileEdgeIndex.Add(n, new List<TileEdge>());

                    TileEdgeIndex[n].Add(new TileEdge
                    {
                        Edge = edge,
                        Flipped = flipped,
                        Tile = tile
                    });
                }
            }
        }

        private void ReindexTilesEdges(Tile tile)
        {
            foreach (var list in TileEdgeIndex.Values.Where(x => x.Any(y => y.Tile == tile)))
            {
                list.RemoveAll(x => x.Tile == tile);
            }

            IndexTileEdges(tile);
        }

        public void ArrangeTiles()
        {
            var firstCornerId = TileEdgeIndex
                .Where(x => x.Value.Count() == 1)
                .Select(x => x.Value.First().Tile.Id)
                .GroupBy(x=>x)
                .Where(g=>g.Count() == 4)
                .Select(g=>g.Key)
                .First();

            var outsideEdges = TileEdgeIndex
                .Where(x => x.Value.Count() == 1 
                            && x.Value.Any(y=>y.Tile.Id == firstCornerId))
                .Select(x => x.Value.First().Edge)
                .Distinct()
                .ToArray();
            
            var curTile = Tiles.First(x => x.Id == firstCornerId);

            if (!outsideEdges.Contains(Edge.Top))
            {
                curTile.FlipY();
            }

            if (!outsideEdges.Contains(Edge.Left))
            {
                curTile.FlipX();
            }
            
            ReindexTilesEdges(curTile);

            var length = (int)Math.Sqrt(Tiles.Count);
            Layout = new Tile[length, length];

            Layout[0, 0] = curTile;

            var y = 0;
            while (true)
            {
                var x = 0;
                
                if (curTile == null)
                {
                    var prevTile = Layout[y - 1, x];
                    var matchingEdges = new[] {
                        prevTile.GetEdgeValue(Edge.Bottom, false),
                        prevTile.GetEdgeValue(Edge.Bottom, true)
                    };
                    
                    var nextTileEdge = TileEdgeIndex
                        .Where(_ => matchingEdges.Contains(_.Key))
                        .SelectMany(_ => _.Value)
                        .FirstOrDefault(_ => _.Tile != prevTile);

                    if (nextTileEdge == null)
                    {
                        break;
                    }
                    
                    switch (nextTileEdge.Edge)
                    {
                        case Edge.Right:
                            nextTileEdge.Tile.Rotate(-1);
                            break;
                        case Edge.Bottom:
                            nextTileEdge.Tile.FlipY();
                            break;
                        case Edge.Left:
                            nextTileEdge.Tile.Rotate(1);
                            break;
                    }

                    if (prevTile.GetEdgeValue(Edge.Bottom, false) != nextTileEdge.Tile.GetEdgeValue(Edge.Top, false))
                    {
                        nextTileEdge.Tile.FlipX();
                    }
                    
                    ReindexTilesEdges(nextTileEdge.Tile);

                    curTile = nextTileEdge.Tile;
                    Layout[y,x] = curTile;
                }

                while (true)
                {
                    var matchingEdges = new[] {
                        curTile.GetEdgeValue(Edge.Right, false),
                        curTile.GetEdgeValue(Edge.Right, true)
                    };

                    var nextTileEdge = TileEdgeIndex
                        .Where(_ => matchingEdges.Contains(_.Key))
                        .SelectMany(_ => _.Value)
                        .FirstOrDefault(_ => _.Tile != curTile);

                    if (nextTileEdge == null)
                    {
                        curTile = null;
                        y++;
                        break;
                    }

                    switch (nextTileEdge.Edge)
                    {
                        case Edge.Top:
                            nextTileEdge.Tile.Rotate(-1);
                            break;
                        case Edge.Right:
                            nextTileEdge.Tile.FlipX();
                            break;
                        case Edge.Bottom:
                            nextTileEdge.Tile.Rotate(1);
                            break;
                    }

                    if (curTile.GetEdgeValue(Edge.Right, false) != nextTileEdge.Tile.GetEdgeValue(Edge.Left, false))
                    {
                        nextTileEdge.Tile.FlipY();
                    }
                    
                    ReindexTilesEdges(nextTileEdge.Tile);

                    x++;
                    curTile = nextTileEdge.Tile;
                    Layout[y,x] = curTile;
                }
            }
        }

        public char[,] GenerateImage()
        {
            var layoutLength = (int)Math.Sqrt(Layout.Length);
            var imgLength = layoutLength * (Tiles.First().Data.Count - 2);
            var img = new char[imgLength,imgLength];
            
            for (var y = 0; y < layoutLength; y++)
            {
                for (var x = 0; x < layoutLength; x++)
                {
                    for (var i = 0; i < Layout[y, x].Img.Count; i++)
                    {
                        var row = Layout[y, x].Img[i];
                        for (var j = 0; j < row.Length; j++)
                        {
                            img[y * row.Length + i, x * row.Length + j] = row[j];
                        }
                    }
                }
            }

            return img;
        }
    }

    public static class TileReplacer
    {
        public static int ReplaceWithCount(Tile pattern, char replacementChar, char[,] image)
        {
            var rows = image.GetLength(0);
            var cols = image.GetLength(1);

            var hits = 0;
            
            var y = 0;
            while (y + pattern.Data.Count - 1 < cols)
            {
                var x = 0;
                while (x + pattern.Data[0].Length - 1 < rows)
                {
                    var reqCoords = GetRequiredCoords(pattern);

                    if (!IsMatch(x, y, reqCoords, image))
                    {
                        x++;
                        continue;
                    }
                    hits++;
                    Replace(x, y, reqCoords, replacementChar, image);
                    x++;
                }

                y++;
            }

            return hits;
        }

        private static void Replace(
            in int x, 
            in int y, 
            IEnumerable<IntVector2> reqCoords, 
            in char replacementChar, 
            char[,] image)
        {
            foreach (var coord in reqCoords)
            {
                image[y + coord.Y, x + coord.X] = replacementChar;
            }
        }

        private static bool IsMatch(
            in int x, 
            in int y, 
            IEnumerable<IntVector2> reqCoords, 
            char[,] image)
        {
            var iY = y;
            var iX = x;
            return reqCoords.All(coord => image[iY + coord.Y, iX + coord.X] == '#');
        }

        private static List<IntVector2> GetRequiredCoords(Tile pattern)
        {
            var coords = new List<IntVector2>();

            for (var y = 0; y < pattern.Data.Count; y++)
            {
                for (var x = 0; x < pattern.Data[y].Length; x++)
                {
                    if(pattern.Data[y][x] != '#') continue;

                    coords.Add(new IntVector2(x, y));
                }
            }

            return coords;
        }
    }

    [DebuggerDisplay("{Tile.Id} {Edge},{Flipped}")]
    public class TileEdge
    {
        public Tile Tile { get; set; }
        public Edge Edge { get; set; }
        public bool Flipped { get; set; }
    }

    public enum Edge
    {
        Top,
        Right,
        Bottom,
        Left
    }
    
    [DebuggerDisplay("{Id}")]
    public class Tile
    {
        public int Id { get; set; }
        public List<char[]> Data { get; private set; } = new List<char[]>();

        public List<char[]> Img => Data
            .Skip(1)
            .Take(Data.Count - 2)
            .Select(row => row[1..^1])
            .ToList();

        public void FlipX()
        {
            for (var i = 0; i < Data.Count; i++)
            {
                Data[i] = Data[i].Reverse().ToArray();
            }
        }

        public void FlipY()
        {
            Data.Reverse();
        }

        public void Rotate(int rotation)
        {
            var newData = new List<char[]>();

            if (rotation > 0)
            {
                RotateRight(newData);
   
                rotation--;
            }
            else if (rotation < 0)
            {
                RotateLeft(newData);
                
                rotation++;
            }
            
            Data = newData;

            if (rotation != 0)
            {
                Rotate(rotation);
            }
        }

        private void RotateLeft(List<char[]> newData)
        {
            for (var i = Data[0].Length - 1; i > -1; i--)
            {
                var newRow = Data.Select(row => row[i]).ToList();
                newData.Add(newRow.ToArray());
            }
        }

        private void RotateRight(List<char[]> newData)
        {
            for (var i = 0; i < Data[0].Length; i++)
            {
                var newRow = Data.Select(row => row[i]).ToList();
                newRow.Reverse();
                newData.Add(newRow.ToArray());
            }
        }

        public int GetEdgeValue(Edge edge, bool flip)
        {
            var chars = edge switch
            {
                Edge.Top => Data.First(),
                Edge.Right => Data.Select(d => d.Last()),
                Edge.Bottom => Data.Last(),
                Edge.Left => Data.Select(d => d.First()),
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };

            chars = chars.Select(c => c == '#' ? '1' : '0');

            if (flip)
                chars = chars.Reverse();

            return Convert.ToInt32(new string(chars.ToArray()), 2);
        }
    }
}