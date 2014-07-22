using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Evade
{
    class EvadeSpellDatabase
    {
        public static List<EvadeSpellData> Spells = new List<EvadeSpellData>();

        static EvadeSpellDatabase()
        {
            //Add available evading spells to the database. SORTED BY PRIORITY.
            EvadeSpellData spell;

            //SpellShields
            if (ObjectManager.Player.BaseSkinName == "Sivir")
            {
                spell = new ShieldData("Sivir E", SpellSlot.E, 0, 1, true);
                Spells.Add(spell);
            }

            //Walking.
            spell = new EvadeSpellData("Walking", 1);
            Spells.Add(spell);

            //Dashes.

            #region Champion Dashes

            if (ObjectManager.Player.BaseSkinName == "Vayne")
            { 
                spell = new DashData("Vayne Q", SpellSlot.Q, 300, true, 250, 900, 2);
                Spells.Add(spell);
            }

            #endregion

            #region Champion Blinks
            if (ObjectManager.Player.BaseSkinName == "Ezreal")
            {
                spell = new BlinkData("Ezreal E", SpellSlot.E, 450, 250, 3);
                Spells.Add(spell);
            }
            #endregion

            //Flash
            if (ObjectManager.Player.GetSpellSlot("SummonerFlash") != SpellSlot.Unknown)
            {
                spell = new BlinkData("Flash", ObjectManager.Player.GetSpellSlot("SummonerFlash"), 400, 100, 5, true);
                Spells.Add(spell);
            }

            //Zhonyas
            spell = new EvadeSpellData("Zhonyas", 5);
            Spells.Add(spell);

        }

        public static EvadeSpellData GetByName(string Name)
        {
            Name = Name.ToLower();
            foreach (var evadeSpellData in Spells)
            {
                if (evadeSpellData.Name.ToLower() == Name)
                    return evadeSpellData;
            }

            return null;
        }
    }
}
