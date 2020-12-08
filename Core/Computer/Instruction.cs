namespace Core.Computer
{
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