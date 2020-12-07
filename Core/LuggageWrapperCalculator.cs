using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class LuggageWrapperCalculator
    {
        public IEnumerable<string> Rules { get; set; }

        private List<string> _usedTargets;
        
        public int FindWrapperVariationsFor(string target)
        {
            _usedTargets = new List<string>();

            IEnumerable<string> wrappers = FindWrapperVariationsFor(new[] {target}).ToList();

            while (wrappers.Any())
            {
                wrappers = FindWrapperVariationsFor(
                    wrappers.Select(ContainerName))
                    .Distinct()
                    .ToList();
            }

            return _usedTargets.Count()-1;
        }

        private static string ContainerName(string r)
        {
            return r.Substring(0, r.IndexOf(" bags", StringComparison.Ordinal));
        }

        private IEnumerable<string> FindWrapperVariationsFor(IEnumerable<string> targets)
        {
            var containers = new List<string>();
            var filteredTargets = targets
                .Where(t => !_usedTargets.Contains(t))
                .ToList();
                
            foreach (var target in filteredTargets)
            {
                containers.AddRange(Rules.Where(r => r.Contains($" {target}") 
                                                     && !_usedTargets.Contains(ContainerName(r))));
            }
            
            _usedTargets.AddRange(filteredTargets);

            return containers;
        }

        public int CountBagTotal(string target)
        {
            var container = new Container(target);

            FillContainer(container);

            return container.Count();
        }
        
        private void FillContainer(Container container)
        {
            var rule = Rules.Single(r => r.StartsWith(container.Name));
            var parts = rule.Split(" bags contain ");
            //0 name
            //1 "no other bags." || 1 dark olive bag, 2 vibrant plum bags.
            //                        CSV
            //{shiny gold} bags contain
            //    {1 mirrored bronze} bag,
            //    {4 dull aqua} bags,
            //    {2 dotted indigo bags},
            //    {1 light indigo} bag.
            
            if(parts[1] == "no other bags.")
            {
                return;
            }

            var subReqs = parts[1].Split(',');
            //"1 dark olive bag"
            //" 2 vibrant plum bags."
            foreach (var subReq in subReqs.Select(s=>s.Trim()))
            {
                //"1 dark olive bag"
                //"2 vibrant plum bags."
                var req = subReq.Substring(0, subReq.IndexOf(" bag", StringComparison.Ordinal));
                //"1 dark olive"
                //"2 vibrant plum"
                var reqParts = req.Split(' ');
                //"1"
                //"dark"
                //"olive"
                
                //"2"
                //"vibrant"
                //"plum"
                var count = int.Parse(reqParts[0]);
                for (int i = 0; i < count; i++)
                {
                    var subContainer = new Container($"{reqParts[1]} {reqParts[2]}");
                    FillContainer(subContainer);
                    container.SubContainers.Add(subContainer);
                }
            }
        }
    }

    internal class Container
    {
        public List<Container> SubContainers { get; set; } = new List<Container>();
        public string Name { get; set; }

        public Container(string name)
        {
            Name = name;
        }
        
        public int Count()
        {
            return SubContainers.Sum(c => c.Count()) + 1;
        }
    }
}