using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core
{
    public class DockingProgramInitializerV1
    {
        public string[] Program { get; set; }
        
        private long _filterMask;
        private long _filterValue;

        private readonly Dictionary<int,long> _memory = new Dictionary<int, long>();

        private readonly Regex _memRegex = new Regex(@"mem\[(\d+)\] = (\d+)");
        
        public void Execute()
        {
            foreach (var line in Program)
            {
                if (line.StartsWith("mask"))
                {
                    SetMasks(line[7..]);
                }
                else
                {
                    var match = _memRegex.Match(line);
                    var address = int.Parse(match.Groups[1].Value);
                    var value = int.Parse(match.Groups[2].Value);
                    SetMemory(address, value);
                }
            }
        }

        private void SetMemory(in int address, in int value)
        {
            var filtered = (value & ~_filterMask) | _filterValue;
            
            if(!_memory.ContainsKey(address))
                _memory.Add(address,0);
            
            _memory[address] = filtered;
        }

        private void SetMasks(string mask)
        {
            _filterMask = Convert.ToInt64(mask.Replace("0", "1").Replace("X", "0"), 2);
            _filterValue = Convert.ToInt64(mask.Replace("X", "0"), 2);
        }

        public long SumNonZeroMemory()
        {
            return _memory.Where(m=>m.Value != 0).Sum(m=>m.Value);
        }
    }
    
    public class DockingProgramInitializerV2
    {
        public string[] Program { get; set; }
        
        private long _filterMask;
        private long _fixedAddressBits;
        private readonly List<long> _floatingAddressBitValues = new List<long>();

        public readonly Dictionary<long,long> Memory = new Dictionary<long, long>();

        private readonly Regex _memRegex = new Regex(@"mem\[(\d+)\] = (\d+)");
        
        public void Execute()
        {
            foreach (var line in Program)
            {
                if (line.StartsWith("mask"))
                {
                    SetMasks(line[7..]);
                }
                else
                {
                    var match = _memRegex.Match(line);
                    var address = long.Parse(match.Groups[1].Value);
                    var value = int.Parse(match.Groups[2].Value);
                    SetMemory(address, value);
                }
            }
        }

        private void SetMemory(in long address, in int value)
        {
            var baseAddress = (address & ~_filterMask) | _fixedAddressBits;

            var variations = (int) Math.Pow(2, _floatingAddressBitValues.Count);
            var addressModifiers = new long[variations];
            for (var i = 1; i < variations; i++)
            {
                addressModifiers[i] = GetFloatingValueIndices(i).Sum(f => _floatingAddressBitValues[f]);
            }

            foreach (var addressModifier in addressModifiers)
            {
                var addr = baseAddress + addressModifier;
                
                if(!Memory.ContainsKey(addr))
                    Memory.Add(addr,0);

                Memory[addr] = value;
            }
        }

        private IEnumerable<int> GetFloatingValueIndices(in int val)
        {
            var components = new List<int>();
            var f = 1;
            var i = 0;
            while (f <= val)
            {
                if((val & f) > 0)
                    components.Add(i);
                f <<= 1;
                i++;
            }
            
            return components;
        }

        private void SetMasks(string mask)
        {
            _filterMask = Convert.ToInt64(mask.Replace("X", "1"), 2);
            _fixedAddressBits = Convert.ToInt64(mask.Replace("X", "0"), 2);

            var n = 0;
            _floatingAddressBitValues.Clear();
            foreach (var c in mask.Reverse())
            {
                if (c == 'X')
                {
                    _floatingAddressBitValues.Add((long) Math.Pow(2, n));
                }

                n++;
            }
        }

        public long SumNonZeroMemory()
        {
            return Memory.Where(m=>m.Value != 0).Sum(m=>m.Value);
        }
    }
}