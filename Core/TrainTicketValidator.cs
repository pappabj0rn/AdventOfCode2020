using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class TrainTicketValidator
    {
        public Field[] Fields { get; set; }

        private IEnumerable<int> _invalidValues;
        private int _min = int.MaxValue;
        private int _max;
       
        public bool ValidateTicketValue(int ticketValue)
        {
            if (_invalidValues is null)
                FillValidValuesFromFieldRanges();
            
            return ticketValue >= _min && ticketValue <= _max && !_invalidValues.Contains(ticketValue);
        }

        private void FillValidValuesFromFieldRanges()
        {
            var validValues = Fields
                .SelectMany(x => x.Ranges)
                .SelectMany(x=>x.ToList())
                .Distinct()
                .OrderBy(x=>x)
                .ToList();
            
            _min = validValues.First();
            _max = validValues.Last();
            
            _invalidValues = Enumerable.Range(_min, _max-_min+1).Except(validValues);
        }

        public void DetermineFieldOrder(List<int[]> validTickets)
        {
            var fieldCandidates = new List<Field[]>(Fields.Length);
            for (var i = 0; i < Fields.Length; i++)
            {
                var i1 = i;
                var col = validTickets.Select(t => t[i1]);
                fieldCandidates.Add(Fields.Where(f => col.All(f.Allows)).ToArray());
            }

            ReduceFieldCandidates(fieldCandidates);

            Fields = fieldCandidates.SelectMany(x => x).ToArray();
        }

        private static void ReduceFieldCandidates(List<Field[]> fieldCandidates)
        {
            while (fieldCandidates.Any(x => x.Length > 1))
            {
                var fixedFields = fieldCandidates
                    .Where(x => x.Length == 1)
                    .SelectMany(x => x)
                    .ToList();

                for (var i = 0; i < fieldCandidates.Count; i++)
                {
                    var curCandidates = fieldCandidates[i];
                    if (curCandidates.Length > 1
                        && curCandidates.Any(x => fixedFields.Contains(x)))
                    {
                        fieldCandidates[i] = curCandidates.Except(fixedFields).ToArray();
                    }
                }
            }
        }
    }

    [DebuggerDisplay("{Name}")]
    public struct Field
    {
        public string Name { get; set; }
        public Range[] Ranges { get; set; }

        public bool Allows(int val)
        {
            return Ranges[0].Allows(val) || Ranges[1].Allows(val);
        }
    }

    public readonly struct Range : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _end;

        public Range(int start, int end)
        {
            _start = start;
            _end = end;
        }
        
        public bool Allows(int val)
        {
            return val >= _start && val <= _end;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return Enumerable.Range(_start, _end +1 - _start).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}