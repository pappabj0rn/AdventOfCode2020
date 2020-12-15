using Core;
using Xunit;

namespace CoreTests
{
    public class DockingProgramInitializerV1Tests
    {
        [Fact]
        public void Example_1()
        {
            var program = new[]
            {
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                "mem[8] = 11",
                "mem[7] = 101",
                "mem[8] = 0"
            };

            var init = new DockingProgramInitializerV1
            {
                Program = program
            };
                
            init.Execute();
                
            Assert.Equal(165, init.SumNonZeroMemory());
        }

        [Fact]
        public void Puzzle_14A()
        {
            var init = new DockingProgramInitializerV1
            {
                Program = PuzzleInputs.Puzzle14
            };
                
            init.Execute();
                
            Assert.Equal(6559449933360, init.SumNonZeroMemory());
        }
    }
    
    public class DockingProgramInitializerV2Tests
    {
        [Fact]
        public void Example_2()
        {
            var program = new[]
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };

            var init = new DockingProgramInitializerV2
            {
                Program = program
            };
                
            init.Execute();
                
            Assert.Equal(208, init.SumNonZeroMemory());
        }
        
        [Fact]
        public void Example_2b()
        {
            var program = new[]
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100"
            };

            var init = new DockingProgramInitializerV2
            {
                Program = program
            };
                
            init.Execute();

            var memAddress = init.Memory.Keys;
            Assert.Contains(26, memAddress);
            Assert.Contains(27, memAddress);
            Assert.Contains(58, memAddress);
            Assert.Contains(59, memAddress);
            foreach (var addr in memAddress)
            {
                Assert.Equal(100, init.Memory[addr]);
            }
        }
        
        [Fact]
        public void Example_2c()
        {
            var program = new[]
            {
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };

            var init = new DockingProgramInitializerV2
            {
                Program = program
            };
                
            init.Execute();

            var memAddress = init.Memory.Keys;
            Assert.Contains(16, memAddress);
            Assert.Contains(17, memAddress);
            Assert.Contains(18, memAddress);
            Assert.Contains(19, memAddress);
            Assert.Contains(24, memAddress);
            Assert.Contains(25, memAddress);
            Assert.Contains(26, memAddress);
            Assert.Contains(27, memAddress);
            foreach (var addr in memAddress)
            {
                Assert.Equal(1, init.Memory[addr]);
            }
        }
        
        [Fact]
        public void Example_2d()
        {
            var program = new[]
            {
                "mask = X00000000000000000000000000000000000",
                "mem[1] = 1"
            };

            var init = new DockingProgramInitializerV2
            {
                Program = program
            };
                
            init.Execute();

            var memAddress = init.Memory.Keys;
            Assert.Contains(1, memAddress);
            Assert.Contains(34_359_738_369, memAddress);
            foreach (var addr in memAddress)
            {
                Assert.Equal(1, init.Memory[addr]);
            }
        }

        [Fact]
        public void Puzzle_14B()
        {
            var init = new DockingProgramInitializerV2
            {
                Program = PuzzleInputs.Puzzle14
            };
                
            init.Execute();
                
            Assert.True(init.SumNonZeroMemory() > 2090634305407, "too low");
            Assert.Equal(3369767240513, init.SumNonZeroMemory());
        }
    }
}