using System;
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

        public static int NextGameId = 0;
        private int GameId;
        public Dictionary<string, bool> PrevD1 = new Dictionary<string, bool>();
        public Dictionary<string, bool> PrevD2 = new Dictionary<string, bool>();
        public void BattleRecursive(Queue<int> d1, Queue<int> d2)
        {
            if (GameId == 0)
            {
                GameId = ++NextGameId;
                Console.WriteLine("Game "+GameId);
            }
            while (d1.Any() && d2.Any())
            {
                RoundsPlayed++;
                //Console.WriteLine("Round "+RoundsPlayed);
                var d1Key = CalcDeckKey(d1);
                var d2Key = CalcDeckKey(d2);
                if (PrevD1.ContainsKey(d1Key) && PrevD2.ContainsKey(d2Key))
                {
                    P1Score = CalculateScore(d1);
                    P2Score = 0;
                    return;
                }

                if(!PrevD1.ContainsKey(d1Key))
                    PrevD1.Add(d1Key,false);
                if(!PrevD2.ContainsKey(d2Key))
                    PrevD2.Add(d2Key,false);
                
                var c1 = d1.Dequeue();
                var c2 = d2.Dequeue();

                var p1Wins = c1 > c2;
                
                if (d1.Count >= c1 && d2.Count >= c2)
                {
                    var cc = new CrabCombat
                    {
                        PrevD1 = new Dictionary<string, bool>(PrevD1),
                        PrevD2 = new Dictionary<string, bool>(PrevD2)
                    };
                    cc.BattleRecursive(new Queue<int>(d1.ToArray().Take(c1)), new Queue<int>(d2.ToArray().Take(c2)));
                    Console.WriteLine("Back to "+GameId);

                    p1Wins = cc.P1Score > 0;
                }
                
                if (p1Wins)
                {
                    //Console.WriteLine($"P1 Wins {c1} {c2}");
                    d1.Enqueue(c1);
                    d1.Enqueue(c2);
                }
                else
                {
                    //Console.WriteLine($"P2 Wins {c1} {c2}");
                    d2.Enqueue(c2);
                    d2.Enqueue(c1);
                }
            }
            
            P1Score = CalculateScore(d1);
            P2Score = CalculateScore(d2);
        }

        private string CalcDeckKey(Queue<int> d1)
        {
            return d1.ToArray().Aggregate("", (c, n) => $"{c}{n},");
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