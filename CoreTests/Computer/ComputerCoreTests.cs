using System;
using System.Collections.Generic;
using Core.Computer;
using Xunit;

namespace CoreTests.Computer
{
    public abstract class ComputerCoreTests
    {
        public class CpuTests
        {
            [Fact]
            public void Run_will_do_nothing_when_there_is_no_program()
            {
                var cpu = new CPU();

                cpu.Run();
                
                Assert.Equal(0, cpu.ProgramCounter);
            }

            [Theory]
            [InlineData(0,7)]
            [InlineData(1,6)]
            [InlineData(2,4)]
            public void Run_will_execute_program_starting_at_program_counter(int pc, int expected)
            {
                var cpu = new CPU
                {
                    ProgramCounter = pc,
                    Program = new List<Instruction>
                    {
                        new Accumulate(1),
                        new Accumulate(2),
                        new Accumulate(4)
                    }
                };
                
                cpu.Run();
                
                Assert.Equal(expected, cpu.Accumulator);
            }

            [Fact]
            public void Run_will_halt_when_before_executing_an_instruction_a_second_time()
            {
                var cpu = new CPU
                {
                    Program = new List<Instruction>
                    {
                        new Accumulate(1),
                        new Jump(-1)
                    }
                };
                
                AssertAsync.CompletesIn(1, () =>
                {
                    cpu.Run();                    
                });

                Assert.Equal(1, cpu.Accumulator);
            }
        }
        
        public class AccTests
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(10)]
            [InlineData(-5)]
            public void Should_increase_global_accumulator_with_parameter_value(int parameter)
            {
                var cpu = new CPU();
                var acc = new Accumulate
                {
                    Arguments = new[]{parameter}
                };
                
                acc.Execute(cpu);
                
                Assert.Equal(parameter, cpu.Accumulator);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(11)]
            public void Should_increase_program_counter_by_one(int pc)
            {
                var cpu = new CPU
                {
                    ProgramCounter = pc
                };
                var acc = new Accumulate
                {
                    Arguments = new[]{0}
                };
                
                acc.Execute(cpu);
                
                Assert.Equal(pc+1, cpu.ProgramCounter);
            }
        }

        public class JumpTests
        {
            [Theory]
            [InlineData(0, 1)]
            [InlineData(1, 1)]
            [InlineData(0, 2)]
            [InlineData(-5, 2)]
            [InlineData(1, 10)]
            public void Should_jump_forward_relative_it_self_given_a_positive_parameter(int pc, int parameter)
            {
                var cpu = new CPU
                {
                    ProgramCounter = pc
                };
                var jmp = new Jump
                {
                    Arguments = new[]{parameter}
                };
                
                jmp.Execute(cpu);
                
                Assert.Equal(cpu.ProgramCounter, pc + parameter);
            }
            
            [Theory]
            [InlineData(0, -1)]
            [InlineData(1, -1)]
            [InlineData(0, -2)]
            [InlineData(-5, -2)]
            [InlineData(1, -10)]
            public void Should_jump_backwards_relative_it_self_given_a_negative_parameter(int pc, int parameter)
            {
                var cpu = new CPU
                {
                    ProgramCounter = pc
                };
                var jmp = new Jump
                {
                    Arguments = new[]{parameter}
                };
                
                jmp.Execute(cpu);
                
                Assert.Equal(cpu.ProgramCounter, pc + parameter);
            }
        }
        
        public class NopTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(11)]
            public void Should_increase_program_counter_by_one(int pc)
            {
                var cpu = new CPU
                {
                    ProgramCounter = pc
                };
                var nop = new NoOperation();
                
                nop.Execute(cpu);
                
                Assert.Equal(pc+1, cpu.ProgramCounter);
            }
        }

        public class ProgramLoaderTests
        {
            [Fact]
            public void Should_parse_data_to_program()
            {
                var cpu = new CPU();
            
                var loader = new ProgramLoader();

                var data = new[]
                {
                    "nop +5",
                    "acc -4",
                    "jmp 0"
                };
                
                loader.Load(data, cpu);
                
                Assert.Equal(3, cpu.Program.Count);
                
                Assert.IsType<NoOperation>(cpu.Program[0]);
                Assert.Equal(5, cpu.Program[0].Arguments[0]);
                
                Assert.IsType<Accumulate>(cpu.Program[1]);
                Assert.Equal(-4, cpu.Program[1].Arguments[0]);
                
                Assert.IsType<Jump>(cpu.Program[2]);
                Assert.Equal(0, cpu.Program[2].Arguments[0]);
            }
        }
    }

    public class ProgramTests
    {
        [Fact]
        public void Example_program_1()
        {
            var cpu = new CPU
            {
                Program = new List<Instruction>
                {
                    new NoOperation(0),
                    new Accumulate(1),
                    new Jump(4),
                    new Accumulate(3),
                    new Jump(-3),
                    new Accumulate(-99),
                    new Accumulate(1),
                    new Jump(-4),
                    new Accumulate(6)
                }
            };

            AssertAsync.CompletesIn(3, () =>
            {
                cpu.Run(); 
            });
            
            Assert.Equal(5, cpu.Accumulator);
        }

        [Fact(Skip="done")]
        public void Puzzle_8A()
        {
            var cpu = new CPU();
            new ProgramLoader().Load(PuzzleInputs.Puzzle8, cpu);

            AssertAsync.CompletesIn(1, () =>
            {
                cpu.Run(); 
            });
            
            Assert.Equal(1451, cpu.Accumulator);
        }

        [Fact(Skip = "done")]
        public void Puzzle_8B()
        {
            for (var i = 0; i < 625; i++)
            {
                var cpu = new CPU();
                new ProgramLoader().Load(PuzzleInputs.Puzzle8, cpu);
                
                var run = false;
                
                var instruction = cpu.Program[i];
                if (instruction.GetType() == typeof(NoOperation))
                {
                    cpu.Program[i] = new Jump(instruction.Arguments[0]);
                    run = true;
                }
                else if (instruction.GetType() == typeof(Jump))
                {
                    cpu.Program[i] = new NoOperation(instruction.Arguments[0]);
                    run = true;
                }

                if (run)
                {
                    cpu.Run();
                    if (cpu.ProgramCounter == cpu.Program.Count)
                    {
                        throw new Exception($"instruction[{i}] should change to {instruction.GetType().Name} to get acc {cpu.Accumulator}");
                    }
                }
            }
        }
    }
}