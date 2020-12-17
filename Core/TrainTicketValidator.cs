using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class TrainTicketValidator
    {
        public Field[] Fields { get; set; }

        private IEnumerable<int> _validValues;

       
        public bool ValidateTicketValue(int ticketValue)
        {
            if (_validValues is null)
                FillValidValuesFromFieldRanges();
            
            return _validValues.Contains(ticketValue);
        }

        private void FillValidValuesFromFieldRanges()
        {
            _validValues = Fields
                .SelectMany(x => x.Ranges)
                .SelectMany(x=>x.ToList())
                .Distinct()
                .OrderBy(x=>x);
        }

        public void DetermineFieldOrder(List<int[]> validTickets)
        {
            var fieldCandidates = new List<Field[]>(Fields.Length);
            for (var i = 0; i < Fields.Length; i++)
            {
                var i1 = i;
                var col = validTickets.Select(t => t[i1]);
                fieldCandidates.Add(Fields.Where(f => f.Allows(col)).ToArray());
            }

            while (fieldCandidates.Any(x=>x.Length > 1))
            {
                var fixedFields = fieldCandidates
                    .Where(x => x.Length == 1)
                    .SelectMany(x=>x)
                    .ToList();

                for (int i = 0; i < fieldCandidates.Count; i++)
                {
                    var curCandidates = fieldCandidates[i];
                    if (curCandidates.Length > 1
                        && curCandidates.Any(x => fixedFields.Contains(x)))
                    {
                        fieldCandidates[i] = curCandidates.Except(fixedFields).ToArray();
                    }
                }
            }

            Fields = fieldCandidates.SelectMany(x => x).ToArray();
        }
    }

    [DebuggerDisplay("{Name}")]
    public struct Field
    {
        public string Name { get; set; }
        public Range[] Ranges { get; set; }

        public bool Allows(IEnumerable<int> col)
        {
            var validValues = Ranges
                .SelectMany(x => x.ToList())
                .Distinct()
                .OrderBy(x => x);

            return col.All(x => validValues.Contains(x));
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