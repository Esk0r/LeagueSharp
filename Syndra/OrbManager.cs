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

        public static bool ActiveRecv = false;
        public static Byte Activebyte = 0x00;

        static OrbManager()
        {
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "SyndraQ")
            {
                tmpQOrbT = Environment.TickCount;
                tmpQOrbPos = args.End;
            }

            if (sender.IsMe && WObject(true) != null && (args.SData.Name == "SyndraW" || args.SData.Name == "syndraw2"))
            {
                tmpWOrbT = Environment.TickCount + 250;
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

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0x8E)
            {
                var packet = new GamePacket(args.PacketData);
                packet.Position = 2;
                var networkId = packet.ReadInteger();
                WObjectNetworkId = networkId;
            }
        }

        public static List<Vector3> GetOrbs(bool toGrab = false)
        {
            var result = new List<Vector3>();
            foreach (
                var obj in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(obj => obj.IsValid && obj.Team == ObjectManager.Player.Team && obj.Name == "Seed"))
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

            if (Environment.TickCount - tmpQOrbT < 400)
            {
                result.Add(tmpQOrbPos);
            }

            if (Environment.TickCount - tmpWOrbT < 400 && Environment.TickCount - tmpWOrbT > 0)
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