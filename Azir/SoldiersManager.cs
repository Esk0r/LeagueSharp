using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Azir
{
    internal static class SoldiersManager
    {
        private static List<Obj_AI_Minion> _soldiers = new List<Obj_AI_Minion>();
        private static Dictionary<int, string> Animations = new Dictionary<int, string>();
        private const bool DrawSoldiers = true;

        public static List<Obj_AI_Minion> ActiveSoldiers
        {
            get { return _soldiers.Where(s => s.IsValid && !s.IsDead && !s.IsMoving && (!Animations.ContainsKey(s.NetworkId) || Animations[s.NetworkId] != "Inactive")).ToList(); }
        }

        public static List<Obj_AI_Minion> AllSoldiers2
        {
            get { return _soldiers.Where(s => s.IsValid && !s.IsDead).ToList(); }
        }

        public static List<Obj_AI_Minion> AllSoldiers
        {
            get { return _soldiers.Where(s => s.IsValid && !s.IsDead && !s.IsMoving).ToList(); }
        }

        static SoldiersManager()
        {
            Obj_AI_Minion.OnCreate += Obj_AI_Minion_OnCreate;
            Obj_AI_Minion.OnDelete += Obj_AI_Minion_OnDelete;
            Obj_AI_Minion.OnPlayAnimation += Obj_AI_Minion_OnPlayAnimation;

            if(DrawSoldiers)
            {
                Drawing.OnDraw += Drawing_OnDraw;
            }
        }

        static void Obj_AI_Minion_OnPlayAnimation(GameObject sender, GameObjectPlayAnimationEventArgs args)
        {
            if(sender is Obj_AI_Minion && ((Obj_AI_Minion)sender).IsSoldier())
            {
                Animations[sender.NetworkId] = args.Animation;
            }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var soldier in ActiveSoldiers)
            {
                Render.Circle.DrawCircle(soldier.Position, 320, System.Drawing.Color.FromArgb(150, System.Drawing.Color.Yellow));
            }
        }

        private static bool IsSoldier(this Obj_AI_Minion soldier)
        {
            return soldier.IsAlly && String.Equals(soldier.BaseSkinName, "azirsoldier", StringComparison.InvariantCultureIgnoreCase);
        }

        static void Obj_AI_Minion_OnCreate(GameObject sender, EventArgs args)
        {
            if(sender is Obj_AI_Minion && ((Obj_AI_Minion)sender).IsSoldier())
            {
                _soldiers.Add((Obj_AI_Minion)sender);
            }
        }

        static void Obj_AI_Minion_OnDelete(GameObject sender, EventArgs args)
        {
            _soldiers.RemoveAll(s => s.NetworkId == sender.NetworkId);
            Animations.Remove(sender.NetworkId);
        }
    }
}
