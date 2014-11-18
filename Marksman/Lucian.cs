#region
using System;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Security.Cryptography;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Lucian : Champion
    {
        public static Spell Q, Q2, W, E;

        public static bool DoubleHit = false;

        private static readonly string[] BeCareful =
        {
            "Amumu", "Annie", "Cassiopeia", "Darius", "Diana", "Draven", "Fiddlestick", "Fizz", "Gragas", "Irelia", "JarvanIV", "Jax", "Jayce", "Kassadin", "Katarina", "Khazix", "LeeSin", "Leona", "Lissandra"
            , "Malphite", "Malzahar", "Maokai", "MasterYi", "Morgana", "Nocturne", "Nunu", "Olaf", "Orianna", "Pantheon"
        };

        public Lucian()
        {
            Utils.PrintMessage("Lucian loaded.");

            Q = new Spell(SpellSlot.Q, 675);
            Q.SetSkillshot(0.25f, 65f, 1100f, false, SkillshotType.SkillshotLine);
            
            Q2 = new Spell(SpellSlot.Q, 1100);
            
            W = new Spell(SpellSlot.W, 1000);
            W.SetSkillshot(0.30f, 80f, 1600f, true, SkillshotType.SkillshotLine); 
            
            E = new Spell(SpellSlot.E, 475);

            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;
        }

        public bool LucianHasPassive
        {
            get
            {
                DoubleHit = ObjectManager.Player.Buffs.Any(buff => buff.Name == "lucianpassivebuff");
                return DoubleHit;
            }
        }


        public bool LucianHasPassive2()
        {
            return Config.Item("Passive" + Id).GetValue<bool>() && DoubleHit;
        }

        public static Obj_AI_Base QMinion
        {
            get
            {
                var vTarget = SimpleTs.GetTarget(Q2.Range, SimpleTs.DamageType.Physical);
                var vMinions = MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.NotAlly,
                    MinionOrderTypes.None);

                return (from vMinion in vMinions.Where(vMinion => vMinion.IsValidTarget(Q.Range))
                    let endPoint =
                        vMinion.ServerPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(), -Q2.Range)
                            .To3D()
                    where
                        vMinion.Distance(vTarget) <= vTarget.Distance(ObjectManager.Player) &&
                        Intersection(ObjectManager.Player.ServerPosition.To2D(), endPoint.To2D(),
                            vTarget.ServerPosition.To2D(), vTarget.BoundingRadius + Q.Width / 4)
                    select vMinion).FirstOrDefault();
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, Q2, W, E };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (!menuItem.Active || spell.Level < 0) return;

                Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public static bool Intersection(Vector2 p1, Vector2 p2, Vector2 pC, float radius)
        {
            var p3 = new Vector2(pC.X + radius, pC.Y + radius);

            var m = ((p2.Y - p1.Y) / (p2.X - p1.X));
            var constant = (m * p1.X) - p1.Y;
            var b = -(2f * ((m * constant) + p3.X + (m * p3.Y)));
            var a = (1 + (m * m));
            var c = ((p3.X * p3.X) + (p3.Y * p3.Y) - (radius * radius) + (2f * constant * p3.Y) + (constant * constant));
            var d = ((b * b) - (4f * a * c));

            return d > 0;
        }

        public static bool IsPositionSafeForE(Obj_AI_Hero target, Spell spell)
        {
            var predPos = spell.GetPrediction(target).UnitPosition.To2D();
            var myPos = ObjectManager.Player.Position.To2D();
            var newPos = (target.Position.To2D() - myPos);
            newPos.Normalize();

            var checkPos = predPos + newPos * (spell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;

            foreach (var tower in ObjectManager.Get<Obj_Turret>()
                .Where(tower => tower.IsValid && !tower.IsDead && Math.Abs(tower.Health) > float.Epsilon)
                .Where(tower => Vector3.Distance(tower.Position, ObjectManager.Player.Position) < 1450))
            {
                closestTower = tower;
            }

            if (closestTower == null)
                return true;

            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
                return false;

            return true;
        }

        public static void WriteLowEnemy()
        {
        }
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
                return;

            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;

                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget() && !LucianHasPassive)
                {
                    Q.CastOnUnit(t);
                }
            }

            if ((!ComboActive && !HarassActive)) return;

            var useQ = Config.Item("UseQ" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useE = Config.Item("UseE" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useW = Config.Item("UseW" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();
            var useQExtended = Config.Item("UseQExtended" + (ComboActive ? "C" : "H") + Id).GetValue<bool>();

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Level > 0)
                Config.Item("GHOSTBLADE")
                    .SetValue(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LucianR");

            if (useQExtended && Q.IsReady())
            {
                var t = SimpleTs.GetTarget(Q2.Range, SimpleTs.DamageType.Physical);

                if (t != null && QMinion.IsValidTarget() && ObjectManager.Player.Distance(t) > Q.Range)
                {
                    if (ObjectManager.Player.Distance(t) > Orbwalking.GetRealAutoAttackRange(t) + 40)
                        Q.CastOnUnit(QMinion);
                }
            }

            if (useQ && Q.IsReady())
            {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget() && !LucianHasPassive)
                {
                    Q.CastOnUnit(t);
                }
            }

            if (useW && W.IsReady())
            {
                var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    if (ObjectManager.Player.Distance(t) <= Orbwalking.GetRealAutoAttackRange(t))
                    {
                        if (!LucianHasPassive)
                            W.Cast(t);
                    }
                    else
                        W.Cast(t);
                }
            }

            if (useE && E.IsReady())
            {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t != null)
                {
                    if (!LucianHasPassive)
                        E.Cast(Game.CursorPos);
                }
            }
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedC" + Id, "Use Extended Q").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0],
                    KeyBindType.Toggle)));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseQExtendedH" + Id, "Use Extended Q").SetValue(true));
            config.AddItem(
                new MenuItem("UseQExtendedTH" + Id, "Use Ext. Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("Passive" + Id, "Take in consideration Passive").SetValue(true));

            return true;
        }
       
        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(new MenuItem("DrawQ2" + Id, "Extended Q range").SetValue(new Circle(true, Color.CornflowerBlue)));
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.CornflowerBlue)));
            config.AddItem(new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.CornflowerBlue)));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

    }
}
