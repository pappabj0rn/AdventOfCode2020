using System.Linq;

namespace Core
{
    public class PasswordPolicy
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public char Char { get; set; }

        public bool Evaluate(string input)
        {
            var targetCharCount = input.Count(x => x == Char);
            return
                targetCharCount >= Min
                && targetCharCount <= Max;
        }
        
        public static PasswordPolicy Parse(string input)
        {
            var parts = input
                .Replace(' ', '-')
                .Split('-');
            
            return new PasswordPolicy
            {
                Min = int.Parse(parts[0]),
                Max = int.Parse(parts[1]),
                Char = parts[2][0]
            };
        }
    }
    
    public class PasswordPolicy2
    {
        public int Pos1 { get; set; }
        public int Pos2 { get; set; }
        public char Char { get; set; }

        public bool Evaluate(string input)
        {
            int Matches(int pos)
            {
                return input[pos] == Char ? 1 : 0;
            }

            var matches = Matches(Pos1) + Matches(Pos2);
            return matches == 1;
        }
        
        public static PasswordPolicy2 Parse(string input)
        {
            var parts = input
                .Replace(' ', '-')
                .Split('-');
            
            return new PasswordPolicy2
            {
                Pos1 = int.Parse(parts[0])-1,
                Pos2 = int.Parse(parts[1])-1,
                Char = parts[2][0]
            };
        }
    }
}