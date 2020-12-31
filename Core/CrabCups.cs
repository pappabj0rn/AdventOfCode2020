using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class CrabCups
    {
        private readonly LinkedList<int> _circle;
        private int _max = 0;

        private Stopwatch _sw = new Stopwatch();
        private LinkedListNode<int> _curentNode;
        private Dictionary<int, LinkedListNode<int>> _nodeIndex 
            = new Dictionary<int, LinkedListNode<int>>();

        public CrabCups(IEnumerable<int> circle)
        {
            _circle = new LinkedList<int>(circle);
            _curentNode = _circle.First;

            var next = _curentNode;
            while (next != null)
            {
                _nodeIndex.Add(next.Value, next);
                next = next.Next;
            }

            _max = _circle.Max();
        }
        
        public void Move()
        {
            var movingCups = GetMovingCups();

            var targetCup = CalculateTargetCup(movingCups);

            //var targetNode = _circle.Find(targetCup);
            var targetNode = _nodeIndex[targetCup];

            foreach (var movingCup in movingCups.Reverse())
            {
                _circle.AddAfter(targetNode, movingCup);
            }

            _curentNode = _curentNode.NextOrFirst();
        }

        private int CalculateTargetCup(LinkedListNode<int>[] movingCups)
        {
            var targetCup = _curentNode.Value - 1;

            var rerun = true;
            while (rerun)
            {
                rerun = false;

                if (movingCups.Select(x => x.Value).Contains(targetCup))
                {
                    targetCup--;
                    rerun = true;
                }

                if (targetCup >= 1) continue;

                targetCup = _max;
                rerun = true;
            }

            return targetCup;
        }

        private LinkedListNode<int>[] GetMovingCups()
        {
            var firstMovingNode = _curentNode.NextOrFirst();
            var secondMovingNode = firstMovingNode.NextOrFirst();
            var thirdMovingNode = secondMovingNode.NextOrFirst();
            
            _circle.Remove(firstMovingNode);
            _circle.Remove(secondMovingNode);
            _circle.Remove(thirdMovingNode);
            
            return new[]
            {
                firstMovingNode,
                secondMovingNode,
                thirdMovingNode
            };
        }

        public IEnumerable<int> GetOrderFromOne()
        {
            var startNode = _circle
                .Find(1);

            var first = true;
            
            while (first || startNode.NextOrFirst().Value != 1)
            {
                first = false;
                startNode = startNode.NextOrFirst();
                yield return startNode.Value;
            }
        }
    }
    
    static class CircularLinkedList {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }
    }
}