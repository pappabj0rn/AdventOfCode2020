using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ConfigurableOperatorPrecedenceCalculator
    {
        private static readonly Dictionary<char, Func<long,long,long>> Operators;

        static ConfigurableOperatorPrecedenceCalculator()
        {
            Operators = new Dictionary<char, Func<long,long,long>>
            {
                {'+', (a,b) => a+b},
                {'*', (a,b) => a*b}
            };
        }

        public bool ObserveOperatorPrecedence { get; set; }
        
        public long Evaluate(string expression)
        {
            expression = ResolveParentheses(expression);

            while (expression.Any(c=>Operators.Keys.Contains(c)))
            {
                expression = ObserveOperatorPrecedence
                    ? ResolveOperatorsByPrecedence(expression)
                    : ResolveOperatorsLeftToRight(expression);
            }

            return long.Parse(expression);
        }

        private string ResolveOperatorsByPrecedence(string expression)
        {
            foreach (var op in Operators)     
            {
                while (expression.Contains(op.Key))
                {
                    var opIndex = expression.IndexOf(op.Key);
                    var (sA, _, a) = FindPrecedingTermInfo(opIndex, expression);
                    var (_, eB, b) = FindFollowingTermInfo(opIndex, expression);
                    var result = op.Value(a, b);
                    expression = expression.Substring(0, sA) 
                                 + result 
                                 + expression.Substring(eB);
                }
            }
            return expression;
        }

        private (int start, int end, long value) FindPrecedingTermInfo(in int opIndex, string expression)
        {
            var start = opIndex;
            for (var i = opIndex-1; i >= 0; i--)
            {
                var c = expression[i];
                if (start < opIndex && c == ' ')
                {
                    var end = i + 1;
                    return (end, start, long.Parse(expression[end..start]));
                }

                if (char.IsDigit(c) && start == opIndex)
                    start = i+1;
            }
            
            return start <= opIndex 
                ? (0, start, long.Parse(expression[0..start]))
                : (-1, 0, 0);
        }

        private (int start, int end, long value) FindFollowingTermInfo(in int opIndex, string expression)
        {
            var start = 0;
            for (var i = opIndex+1; i < expression.Length; i++)
            {
                var c = expression[i];
                if (start > 0 && c == ' ')
                    return (start, i, long.Parse(expression[start..i]));

                if (char.IsDigit(c) && start == 0)
                    start = i;
            }
            
            return start > 0  
                ? (start, expression.Length, long.Parse(expression[start..]))
                : (-1, 0, 0);
        }

        private string ResolveOperatorsLeftToRight(string expression)
        {
            while (expression.Any(c => Operators.Keys.Contains(c)))
            {
                var opIndex = Operators
                    .Keys
                    .Select(o => expression.IndexOf(o))
                    .Where(x=> x > 0)
                    .Min();
                var op = Operators[expression[opIndex]];
                
                var (sA, _, a) = FindPrecedingTermInfo(opIndex, expression);
                var (_, eB, b) = FindFollowingTermInfo(opIndex, expression);
                var result = op(a, b);
                expression = expression.Substring(0, sA) 
                             + result 
                             + expression.Substring(eB);
            }
            
            return expression;
        }

        private string ResolveParentheses(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                var c = expression[i];

                if (c != '(') continue;
                
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
                var result = Evaluate(expression.Substring(i+1,counter-1)).ToString();
                expression = expression.Substring(0, i) + result + expression.Substring(i+1 + counter);
            }

            return expression;
        }
    }
}