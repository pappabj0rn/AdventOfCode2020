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
}