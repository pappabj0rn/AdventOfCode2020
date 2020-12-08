using System;

namespace Core.Computer
{
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
}