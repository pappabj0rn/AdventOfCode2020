using System;
using System.Collections.Generic;

namespace Core.Computer
{
    public class CPU
    {
        public int Accumulator;
        public int ProgramCounter;
        public List<Instruction> Program = new List<Instruction>();
        private List<int> _visitedInstructions = new List<int>();

        public void Run()
        {
            while (ProgramCounter < Program.Count)
            {
                if (_visitedInstructions.Contains(ProgramCounter))
                    break;
                
                _visitedInstructions.Add(ProgramCounter);
                Program[ProgramCounter].Execute(this);
            }
        }

        public void Reset()
        {
            ProgramCounter = 0;
            Accumulator = 0;
            _visitedInstructions = new List<int>();
        }
    }
    
    public class ProgramLoader
    {
        public void Load(string[] data, CPU cpu)
        {
            foreach (var line in data)
            {
                var parts = line.Split(' ');
                var arg = int.Parse(parts[1]);
                Instruction instruction = parts[0] switch
                {
                    "nop" => new NoOperation(arg),
                    "acc" => new Accumulate(arg),
                    "jmp" => new Jump(arg),
                    _ => throw new NotImplementedException()
                };
                
                cpu.Program.Add(instruction);
            }
        }
    }
    
    public abstract class Instruction
    {
        public int[] Arguments;

        public virtual void Execute(CPU cpu)
        {
            IncreaseProgramCounter(cpu);
        }

        private void IncreaseProgramCounter(CPU cpu)
        {
            cpu.ProgramCounter++;
        }
    }

    public class NoOperation : Instruction
    {
        public NoOperation()
        {
            
        }

        public NoOperation(int parameter)
        {
            Arguments = new[] {parameter};
        }
    }

    public class Accumulate : Instruction
    {
        public Accumulate()
        {
            
        }

        public Accumulate(int parameter)
        {
            Arguments = new[] {parameter};
        }
        
        public override void Execute(CPU cpu)
        {
            cpu.Accumulator += Arguments[0];
            base.Execute(cpu);
        }
    }

    public class Jump : Instruction
    {
        public Jump()
        {
            
        }

        public Jump(int parameter)
        {
            Arguments = new[] {parameter};
        }

        public override void Execute(CPU cpu)
        {
            cpu.ProgramCounter += Arguments[0];
        }
    }
}