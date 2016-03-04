#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Syndra
{
    public static class OrbManager
    {
        private static int _wobjectnetworkid = -1;

        public static int WObjectNetworkId
        {
            get
            {
                if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    return -1;

                return _wobjectnetworkid;
            }
            set
            {
                _wobjectnetworkid = value;
            }
        }

        public static int tmpQOrbT;
        public static Vector3 tmpQOrbPos = new Vector3();

        public static int tmpWOrbT;
        public static Vector3 tmpWOrbPos = new Vector3();

        static OrbManager()
        {
            Obj_AI_Base.OnPauseAnimation += Obj_AI_Base_OnPauseAnimation;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        static void Obj_AI_Base_OnPauseAnimation(Obj_AI_Base sender, Obj_AI_BasePauseAnimationEventArgs args)
        {
            if (sender is Obj_AI_Minion)
            {
                WObjectNetworkId = sender.NetworkId;  
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name.Equals("SyndraQ", StringComparison.InvariantCultureIgnoreCase))
            {
                tmpQOrbT = Utils.TickCount;
                tmpQOrbPos = args.End;
            }

            if (sender.IsMe && WObject(true) != null && (args.SData.Name.Equals("SyndraW", StringComparison.InvariantCultureIgnoreCase) || args.SData.Name.Equals("syndraw2", StringComparison.InvariantCultureIgnoreCase)))
            {
                tmpWOrbT = Utils.TickCount + 250;
                tmpWOrbPos = args.End;
            }
        }

        public static Obj_AI_Minion WObject(bool onlyOrb)
        {
            if (WObjectNetworkId == -1) return null;
            var obj = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(WObjectNetworkId);
            if (obj != null && obj.IsValid<Obj_AI_Minion>() && (obj.Name == "Seed" && onlyOrb || !onlyOrb)) return (Obj_AI_Minion)obj;
            return null;
        }

        public static List<Vector3> GetOrbs(bool toGrab = false)
        {
            var result = new List<Vector3>();
            foreach (
                var obj in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(obj => obj.IsValid && obj.Team == ObjectManager.Player.Team && !obj.IsDead && obj.Name == "Seed"))
            {
                
                var valid = false;
                if (obj.NetworkId != WObjectNetworkId)
                    if (
                        ObjectManager.Get<GameObject>()
                            .Any(
                                b =>
                                    b.IsValid && b.Name.Contains("_Q_") && b.Name.Contains("Syndra_") &&
                                    b.Name.Contains("idle") && obj.Position.Distance(b.Position) < 50))
                        valid = true;
               
                if (valid && (!toGrab || !obj.IsMoving))
                    result.Add(obj.ServerPosition);
            }

            if (Utils.TickCount - tmpQOrbT < 400)
            {
                result.Add(tmpQOrbPos);
            }

            if (Utils.TickCount - tmpWOrbT < 400 && Utils.TickCount - tmpWOrbT > 0)
            {
                result.Add(tmpWOrbPos);
            }

            return result;
        }

        public static Vector3 GetOrbToGrab(int range)
        {
            var list = GetOrbs(true).Where(orb => ObjectManager.Player.Distance(orb) < range).ToList();
            return list.Count > 0 ? list[0] : new Vector3();
        }
    }
}