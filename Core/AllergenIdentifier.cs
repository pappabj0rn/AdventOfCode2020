using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class AllergenIdentifier
    {
        public List<Set> Sets { get; set; }
        public List<string> AllergenFreeIngredients { get; private set; } = new List<string>();

        public void Analyze()
        {
            var allergens = Sets
                .SelectMany(x=>x.Allergens)
                .Distinct()
                .ToList();

            var allergenDictionary = allergens
                .ToDictionary(
                    allergen => allergen, 
                    allergen => new List<string>());

            while (Sets.SelectMany(x=>x.Allergens).Any())
            {
                foreach (var allergen in allergens)
                {
                    if (allergenDictionary[allergen].Count == 1) continue;

                    var candidates = Sets
                        .Where(x => x.Allergens.Contains(allergen))
                        .Select(x => x.Ingredients)
                        .Aggregate((prev, next) => prev.Intersect(next).ToList());

                    allergenDictionary[allergen] = candidates;

                    if (candidates.Count != 1) continue;

                    var candidate = candidates.First();
                    foreach (var set in Sets.Where(x => x.Ingredients.Contains(candidate)))
                    {
                        set.Allergens.Remove(allergen);
                        set.DangerousIngredients.Add(candidate, allergen);
                        set.Ingredients.Remove(candidate);
                    }
                }
            }
            
            AllergenFreeIngredients = Sets
                .SelectMany(x=>x.Ingredients)
                .Distinct()
                .ToList();
        }

        public int CountAllergenFreeIngredientsOccurrences()
        {
            return Sets.SelectMany(x => x.Ingredients).Count();
        }
    }

    public class Set
    {
        public List<string> Ingredients { get; } = new List<string>();
        public Dictionary<string,string> DangerousIngredients { get; set; } = new Dictionary<string,string>();
        public List<string> Allergens { get; } = new List<string>();
        
        public Set(string source)
        {
            var parts = source
                .Replace(")", "")
                .Replace(",", "")
                .Split(" (contains ");
            
            Ingredients.AddRange(parts[0].Split(" "));
            Allergens.AddRange(parts[1].Split(" "));
        }
    }
}