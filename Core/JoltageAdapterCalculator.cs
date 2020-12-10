using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class JoltageAdapterCalculator
    {
        public int[] Adapters { get; set; }

        public (int ones, int threes) FindDiffs()
        {
            var sorted = Adapters.OrderBy(x => x).ToArray();

            var ones = sorted[0] == 1 ? 1 : 0;
            var threes = sorted[0] == 3 ? 2 : 1;

            for (var i = 0; i < sorted.Count() -1; i++)
            {
                switch (sorted[i+1] - sorted[i])
                {
                    case 1:
                        ones++;
                        break;
                    case 3:
                        threes++;
                        break;
                }
            }
            
            return (ones, threes);
        }

        public long FindVariations()
        {
            var node = BuildNodeFor(0);
            return node.Branches;
        }

        readonly Dictionary<int, Node> _nodeCache = new Dictionary<int, Node>();
        private Node BuildNodeFor(in int i)
        {
            if (_nodeCache.ContainsKey(i))
            {
                return _nodeCache[i];
            }
            
            var node = new Node(BuildNodesFor(i));
            
            _nodeCache.Add(i, node);
            return node;
        }

        private IEnumerable<Node> BuildNodesFor(in int i)
        {
            var n = i;
            var connections = Adapters.Where(x => x > n && x < n + 4);
            
            return connections.Select(x => BuildNodeFor(x));
        }
    }

    public class Node
    {
        public long Branches { get; }
        
        public Node(IEnumerable<Node> nodes)
        {
            Branches = nodes.Any()
                ? nodes.Sum(x => x.Branches)
                : 1;
        }
    }
}