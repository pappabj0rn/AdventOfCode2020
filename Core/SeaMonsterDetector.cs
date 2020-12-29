using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class SeaMonsterDetector
    {
        public class RegexInfo
        {
            public string Input { get; set; }
            public string Regex { get; set; }
        }
        
        public Dictionary<int, RegexInfo> Rules { get; }
            = new Dictionary<int, RegexInfo>();
            
        public void BuildRules(string[] rulesInput)
        {
            foreach (var input in rulesInput)
            {
                var parts = input.Split(':');
                var ruleId = int.Parse(parts[0]);
                Rules.Add(
                    ruleId,
                    new RegexInfo
                    {
                        Input = parts[1].Trim(),
                        Regex = parts[1].Contains('"')
                            ? parts[1].Replace("\"", "").Trim()
                            : ""
                    });
            }

            while (Rules.Values.Any(x => x.Regex.IsNullOrEmpty()))
            {
                var (key, ri) = Rules.First(x => x.Value.Regex.IsNullOrEmpty());
                Rules[key].Regex = ParseRule(ri.Input);
            }
        }

        private string ParseRule(string input)
        {
            var sb = new StringBuilder();
            
            var parts = input.Split(' ');
            foreach (var part in parts)
            {
                if (int.TryParse(part, out int v))
                {
                    if (Rules[v].Regex == "")
                    {
                        Rules[v].Regex = ParseRule(Rules[v].Input);
                    }

                    sb.Append($"({Rules[v].Regex})");
                }
                else
                {
                    sb.Append(part);
                }
            }
            
            return sb.ToString();
        }

        public bool TestStringWithRule(string s, int key)
        {
            var regex = new Regex($"^{Rules[key].Regex}$");
            return regex.IsMatch(s);
        }
    }
}