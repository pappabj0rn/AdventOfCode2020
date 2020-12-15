using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class BusCalculator
    {
        public (int id, int waitTime) CalculateWaitTimeForNextBus(int arrivalTime, int[] buses)
        {
            var id = 0;
            var minWait = int.MaxValue;

            foreach (var bus in buses)
            {
                var wait = (arrivalTime / bus + 1) * bus - arrivalTime;
                if (wait >= minWait) 
                    continue;
                
                minWait = wait;
                id = bus;
            }

            return (id, minWait);
        }
        
        public long FindEarliestTimestampForSubsequentDepartures(string[] buses, long searchStart = 1)
        {
            var offsetList = new List<(int bus, int offset)>();
            
            BuildBusOffsetList(buses, offsetList);

            var baseBus = offsetList.First().bus;
            offsetList = offsetList
                .Skip(1) // skip base
                .OrderByDescending(x => x.bus)
                .ToList();
            
            var baseMultiplier = Math.Max(searchStart, offsetList[0].bus) / offsetList[0].bus;
            var step = 1L;

            var busIndex = -1;
            while (true)
            {
                var @base = baseBus * baseMultiplier;

                if (offsetList.All(busOffset => IsOkBase(busOffset, @base)))
                    return @base;

                if(busIndex < offsetList.Count-1)
                {
                    var next = busIndex + 1;
                    if (IsOkBase(offsetList[next], @base))
                    {
                        step *= offsetList[next].bus;
                        busIndex++;
                    }
                }

                baseMultiplier += step;
            }
        }

        private static void BuildBusOffsetList(string[] buses, List<(int bus, int offset)> offsetList)
        {
            for (var i = 0; i < buses.Length; i++)
            {
                var busString = buses[i];
                if (busString == "x")
                    continue;

                var bus = int.Parse(busString);

                offsetList.Add((bus, i));
            }
        }

        private bool IsOkBase((int bus, int offset) busOffset, in long @base)
        {
            return (@base + busOffset.offset) % busOffset.bus == 0;
        }
    }
}