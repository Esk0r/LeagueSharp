using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marksman
{
    internal class BuffList
    {
        public string ChampionName { get; set; }
        public string DisplayName { get; set; }
        public string BuffName { get; set; }
    }

    internal class Activator
    {
        public string ChampionName { get; set; }
        public string DisplayName { get; set; }
        public string BuffName { get; set; }

        public readonly List<BuffList> BuffList = new List<BuffList>();

        public Activator()
        {
            BuffList.Add(new BuffList{ ChampionName = "Fizz", DisplayName = "Fizz (R)", BuffName = "FizzMarinerDoom" });
            BuffList.Add(new BuffList{ ChampionName = "Malzahar", DisplayName = "Malzahar (R)", BuffName = "AlZaharNetherGrasp" });
            BuffList.Add(new BuffList{ ChampionName = "Mordekaiser", DisplayName = "Mordekaiser (R)", BuffName = "MordekaiserChildrenOfTheGrave" });
            BuffList.Add(new BuffList{ ChampionName = "Nocturne", DisplayName = "Nocturne (R)", BuffName = "NocturneParanoia" });
            BuffList.Add(new BuffList{ ChampionName = "Darius", DisplayName = "Darius (W)", BuffName = "DariusNoxianTacticsONH" });
            BuffList.Add(new BuffList{ ChampionName = "Poppy", DisplayName = "Poppy (R)", BuffName = "PoppyDiplomaticImmunity" });
            BuffList.Add(new BuffList{ ChampionName = "TwistedFate", DisplayName = "Twisted Fate (R)", BuffName = "Destiny" });
            BuffList.Add(new BuffList{ ChampionName = "Urgot", DisplayName = "Urgot (R)", BuffName = "UrgotSwap2" });
            BuffList.Add(new BuffList{ ChampionName = "Skarner", DisplayName = "Skarner (R)", BuffName = "SkarnerImpale" });
            BuffList.Add(new BuffList{ ChampionName = "Vladimir", DisplayName = "Vladimir (R)", BuffName = "VladimirHemoplague" });
            BuffList.Add(new BuffList{ ChampionName = "Warwick", DisplayName = "Warwick (R)", BuffName = "InfiniteDuress" });
            BuffList.Add(new BuffList{ ChampionName = "Morgana", DisplayName = "Morgana (Q)", BuffName = "DarkBindingMissile" });
            BuffList.Add(new BuffList{ ChampionName = "Diana", DisplayName = "Diana (Q)", BuffName = "DianaArc" }); 
            BuffList.Add(new BuffList{ ChampionName = "Zilean", DisplayName = "Zilean (Q)", BuffName = "timebombenemybuff" });
            BuffList.Add(new BuffList{ ChampionName = "Zed", DisplayName = "Zed (R)", BuffName = "zedulttargetmark" });
            BuffList.Add(new BuffList{ ChampionName = "LeBlanc", DisplayName = "LeBlanc (E)", BuffName = "LeblancSoulShackle" });
        }
    }
    
}
