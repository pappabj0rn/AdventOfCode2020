using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core
{
    public class PassportValidator
    {
        public string Input { get; set; }
        
        public int CountValidPassports()
        {
            void ParsePassport(List<string> list, List<Passport> passports)
            {
                if (!list.Any()) 
                    return;
                
                var passport = new Passport();
                passport.Parse(list);
                passports.Add(passport);
                list.Clear();
            }

            var parsedPassports = new List<Passport>();

            var lines = Input.Split(Environment.NewLine);

            var wipData = new List<string>();
            foreach (var line in lines)
            {
                var emptyLine = line.IsNullOrEmpty();
                if(emptyLine)
                {
                    ParsePassport(wipData, parsedPassports);
                    continue;
                }

                var parts = line.Split(' ');
                wipData.AddRange(parts);
            }

            ParsePassport(wipData, parsedPassports);
            
            
            return parsedPassports.Count(x=>x.Validate());
        }
    }
    
    internal class Passport
    {
        public int? Byr { get; set; }
        public int? Iyr { get; set; }
        public int? Eyr { get; set; }
        public string Pid { get; set; }
        public int? Cid { get; set; }
        public string Hgt { get; set; }
        public string Hcl { get; set; }
        public string Ecl { get; set; }
            
        public void Parse(IEnumerable<string> input)
        {
            foreach (var str in input)
            {
                var pair = str.Split(':');
                Regex regex;
                Match match;
                switch (pair[0].ToLower())
                {
                    case "byr":
                        if (int.TryParse(pair[1], out var byr) && byr>=1920 && byr<=2002)
                        {
                            Byr = byr;
                        }
                        break;
                    case "iyr":
                        if (int.TryParse(pair[1], out var iyr) && iyr>=2010 && iyr<=2020)
                        {
                            Iyr = iyr;
                        }
                        break;
                    case "eyr":
                        if (int.TryParse(pair[1], out var eyr) && eyr>=2020 && eyr<=2030)
                        {
                            Eyr = eyr;
                        }
                        break;
                    case "hgt":
                        regex = new Regex("(\\d*)(cm|in)");
                        match = regex.Match(pair[1]);
                        if (match.Success && int.TryParse(match.Groups[1].Value, out var value))
                        {
                            var unit = match.Groups[2].Value.ToLower();
                            if (unit == "cm" 
                                    && value >= 150 
                                    && value <= 193
                                || unit == "in" 
                                    && value >= 59 
                                    && value <= 76)
                            {
                                Hgt = pair[1];
                            }
                        }
                        break;
                    case "hcl":
                        regex = new Regex("^#[0-9a-f]{6}$");
                        match = regex.Match(pair[1]);
                        if (match.Success)
                        {
                            Hcl = pair[1];
                        }
                        break;
                    case "ecl":
                        var validEclValues = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
                        if (validEclValues.Contains(pair[1]))
                        {
                            Ecl = pair[1];
                        }
                        break;
                    case "pid":
                        regex = new Regex("^[0-9]{9}$");
                        match = regex.Match(pair[1]);
                        if (match.Success)
                        {
                            Pid = pair[1];
                        }
                        break;
                    case "cid":
                        Cid = int.Parse(pair[1]);
                        break;
                }
            } 
        }
            
        public bool Validate()
        {
            return Byr.HasValue
                && Iyr.HasValue
                && Eyr.HasValue
                && !Pid.IsNullOrEmpty()
                && !Hgt.IsNullOrEmpty()
                && !Hcl.IsNullOrEmpty()
                && !Ecl.IsNullOrEmpty();
        }
    }
}