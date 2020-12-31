using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core;
using Xunit;

namespace CoreTests
{
    public class CrabCupsTests
    {
        [Theory]
        [InlineData(new[]{3,8,9,1,2,5,4,6,7}, 10, new[]{9,2,6,5,8,3,7,4})]
        [InlineData(new[]{3,8,9,1,2,5,4,6,7}, 100, new[]{6,7,3,8,4,5,2,9})]
        public void Example_1(int[] start, int turns, int[] expectedEnd)
        {
            var cc = new CrabCups(start);
            for (var i = 0; i < turns; i++)
            {
                cc.Move();
            }

            for (var i = 0; i < expectedEnd.Length; i++)
            {
                Assert.Equal(expectedEnd[i], cc.GetOrderFromOne().Skip(i).First());
            }
        }

        [Fact]
        public void Puzzle_23A()
        {
            var cc = new CrabCups(new[] {6, 8, 5, 9, 7, 4, 2, 1, 3});
            for (var i = 0; i < 100; i++)
            {
                cc.Move();
            }

            var result = cc.GetOrderFromOne().Aggregate("", (c, n) => $"{c}{n}");
            
            Assert.Equal("82635947", result);
        }

        [Fact(Skip = "done")]
        public void Example_2()
        {
            var firstNumbers = new List<int> {3, 8, 9, 1, 2, 5, 4, 6, 7};
            var following = Enumerable.Range(firstNumbers.Max()+1, 1_000_000 - firstNumbers.Count);
            firstNumbers.AddRange(following);
            var cc = new CrabCups(firstNumbers);
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10_000_000; i++)
            {
                cc.Move();
            }
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds}ms");

            Assert.Equal(934001, cc.GetOrderFromOne().First());
            Assert.Equal(159792, cc.GetOrderFromOne().Skip(1).First());
            
            Assert.Equal(149245887792, (long)cc.GetOrderFromOne().First() * cc.GetOrderFromOne().Skip(1).First());
        }

        [Fact(Skip = "done")]
        public void Puzzle_23B()
        {
            var firstNumbers = new List<int> {6, 8, 5, 9, 7, 4, 2, 1, 3};
            var following = Enumerable.Range(firstNumbers.Max()+1, 1_000_000 - firstNumbers.Count);
            firstNumbers.AddRange(following);
            var cc = new CrabCups(firstNumbers);
            for (var i = 0; i < 10_000_000; i++)
            {
                cc.Move();
            }
            
            Assert.Equal(157047826689, (long)cc.GetOrderFromOne().First() * cc.GetOrderFromOne().Skip(1).First());
        }
    }
}