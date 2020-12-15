using System.Collections.Generic;

namespace Core
{
    public class ElfMemoryGame
    {
        public int Turn { get; private set; }
        public int LastNumber { get; private set; } = -1;
        public int[] StartingNumbers { get; set; } = new int[0];

        private readonly Dictionary<int, int> _numberLastUsage = new Dictionary<int, int>();
        private int _lastUseDelta;
        private bool _lastNumberWasNew;
        
        public void Speak()
        {
            Turn++;
            
            if (HandleStartingNumbers()) return;

            SetLastNumber(_lastNumberWasNew ? 0 : _lastUseDelta);
        }

        private bool HandleStartingNumbers()
        {
            if (StartingNumbers.Length <= 0) 
                return false;
            
            SetLastNumber(StartingNumbers[0]);
            StartingNumbers = StartingNumbers[1..];
            return true;
        }

        private void SetLastNumber(int number)
        {
            LastNumber = number;
            
            if (_numberLastUsage.ContainsKey(LastNumber))
            {
                _lastNumberWasNew = false;
                _lastUseDelta = Turn - _numberLastUsage[LastNumber];
                _numberLastUsage[number] = Turn;
            }
            else
            {
                _lastNumberWasNew = true;
                _numberLastUsage.Add(number, Turn);
            }
        }
    }
}