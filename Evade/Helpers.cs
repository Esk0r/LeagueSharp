using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Evade
{
    static class Helpers
    {
        public static bool IsSpellShielded(this Obj_AI_Hero unit)
        {
            if (ObjectManager.Player.HasBuffOfType(BuffType.SpellShield))
            {
                return true;
            }

            if (ObjectManager.Player.HasBuffOfType(BuffType.SpellImmunity))
            {
                return true;
            }

            //Sivir E
            if (unit.LastCastedSpellName() == "SivirE" && (LeagueSharp.Common.Utils.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }

            //Morganas E
            if (unit.LastCastedSpellName() == "BlackShield" && (LeagueSharp.Common.Utils.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }

            //Nocturnes E
            if (unit.LastCastedSpellName() == "NocturneShit" && (LeagueSharp.Common.Utils.TickCount - unit.LastCastedSpellT()) < 300)
            {
                return true;
            }

            return false;
        }
    }
}
