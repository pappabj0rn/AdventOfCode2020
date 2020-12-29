using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class CrabCombat
    {
        public Queue<int> P1 { get; set; }
        public Queue<int> P2 { get; set; }
        public int RoundsPlayed { get; set; }
        public int P1Score { get; set; }
        public int P2Score { get; set; }

        public void Battle()
        {
            while (P1.Any() && P2.Any())
            {
                RoundsPlayed++;
                
                var c1 = P1.Dequeue();
                var c2 = P2.Dequeue();
                
                if (c1 > c2)
                {
                    P1.Enqueue(c1);
                    P1.Enqueue(c2);
                }
                else
                {
                    P2.Enqueue(c2);
                    P2.Enqueue(c1);
                }
            }

            P1Score = CalculateScore(P1);
            P2Score = CalculateScore(P2);
        }

        private int CalculateScore(Queue<int> deck)
        {
            var score = 0;
            
            for (var i = deck.Count; i > 0; i--)
            {
                score += deck.Dequeue() * i;
            }

            return score;
        }
    }
}