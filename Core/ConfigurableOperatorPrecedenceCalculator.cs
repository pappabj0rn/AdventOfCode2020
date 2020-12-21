using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ConfigurableOperatorPrecedenceCalculator
    {
        public long Evaluate(string expression)
        {
            var operators = new Dictionary<char, Operator>
            {
                {'+', new Operator{Evaluate = (a,b) => a.Value + b.Value}},
                {'-', new Operator{Evaluate = (a,b) => a.Value - b.Value}},
                {'*', new Operator{Evaluate = (a,b) => a.Value * b.Value}}
            };
            
            var wipTerm = new List<char>();
            Operator op = null;
            for (int i = 0; i < expression.Length; i++)
            {
                var c = expression[i];

                if (c == ')')
                {
                    break;
                }
                
                if (c == '(')
                {
                    var counter = 1;
                    for (var j = 1; j < expression.Length; j++)
                    {
                        var c2 = expression[i + j];
                        switch (c2)
                        {
                            case '(':
                                counter++;
                                break;
                            case ')':
                                counter--;
                                break;
                        }

                        if (counter != 0) continue;
                        counter = j;
                        break;
                    }
                    wipTerm = Evaluate(expression.Substring(i+1,counter-1)).ToString().ToList();
                    i += counter;
                }
                
                if (char.IsDigit(c))
                {
                    wipTerm.Add(c);
                }

                if (operators.Keys.Contains(c))
                {
                    if (op != null)
                    {
                        op.TermB = new ValueTerm{Value = long.Parse(new string(wipTerm.ToArray()))};
                        wipTerm.Clear();
                        wipTerm = op.Value.ToString().ToList();
                    }
                    
                    op = operators[c];
                    op.TermA = new ValueTerm{Value = long.Parse(new string(wipTerm.ToArray()))};
                    wipTerm.Clear();
                }
            }

            if (wipTerm.Any())
            {
                var term = new ValueTerm {Value = long.Parse(new string(wipTerm.ToArray()))};
                if (op == null)
                    return term.Value;
                
                op.TermB = term;
            }
            
            return op.Value;
        }
        
    }

    public interface ITerm
    {
        long Value { get; }
    }

    public class ValueTerm : ITerm
    {
        public long Value { get; set; }
    }

    public class Operator : ITerm
    {
        public ITerm TermA { get; set; }
        public ITerm TermB { get; set; }
        public Func<ITerm,ITerm,long> Evaluate { get; set; }
        
        public long Value => Evaluate(TermA, TermB);
    }

    public class Add : ITerm
    {
        private readonly ITerm _a;
        private readonly ITerm _b;

        public Add(ITerm a, ITerm b)
        {
            _a = a;
            _b = b;
        }

        public long Value => _a.Value + _b.Value;
    }
}