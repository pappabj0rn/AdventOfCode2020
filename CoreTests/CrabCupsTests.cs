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
                Assert.Equal(expectedEnd[i], cc.GetOrderFromOne()[i]);
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
    }
}