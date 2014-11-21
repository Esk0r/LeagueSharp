#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace Marksman
{
    internal class Kalista : Champion
    {
        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;

        private static int kalistaMarkerCD;
        
        public Obj_AI_Hero CoopStrikeAlly;
        public float CoopStrikeAllyRange = 1250f;
        public Dictionary<String, int> MarkedChampions = new Dictionary<String, int>();

        public static Items.Item Dfg = new Items.Item(3128, 750);
        public Kalista()
        {
            Utils.PrintMessage("Kalista loaded.");

            Q = new Spell(SpellSlot.Q, 1450);
            W = new Spell(SpellSlot.W, 5500);
            E = new Spell(SpellSlot.E, 950);
            R = new Spell(SpellSlot.R, 1250);

            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);
        }

        public override void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W, E, R };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            var drawConn = GetValue<Circle>("DrawConnMax");
            if (drawConn.Active)
                Utility.DrawCircle(ObjectManager.Player.Position, CoopStrikeAllyRange, drawConn.Color);
        }

        public int KalistaMarkerCount
        {
            get
            {
                var xbuffCount = 0;
                foreach (
                    var buff in from enemy in ObjectManager.Get<Obj_AI_Hero>().Where(tx => tx.IsEnemy && !tx.IsDead)
                        where ObjectManager.Player.Distance(enemy) < E.Range
                        from buff in enemy.Buffs
                        where buff.Name.Contains("kalistaexpungemarker")
                        select buff) 
                {
                    xbuffCount = buff.Count;
                }
                return xbuffCount;
            }
        }

        public static double GetEDamage(Obj_AI_Base t)
        {
            var buff = t.Buffs.FirstOrDefault(xBuff => xBuff.DisplayName.ToLower() == "kalistaexpungemarker");
            if (buff != null)
            {
                double damage = ObjectManager.Player.FlatPhysicalDamageMod + ObjectManager.Player.BaseAttackDamage;
                double eDmg = damage * 0.60 + new double[] { 0, 20, 30, 40, 50, 60 }[E.Level];
                damage += buff.Count * (0.004* damage) + eDmg;
                return ObjectManager.Player.CalcDamage(t, Damage.DamageType.Physical, damage);
            }
            return 0;
        }
        
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (CoopStrikeAlly == null)
            {
                foreach (
                    var ally in
                        from ally in ObjectManager.Get<Obj_AI_Hero>().Where(tx => tx.IsAlly && !tx.IsDead && !tx.IsMe)
                        where ObjectManager.Player.Distance(ally) <= CoopStrikeAllyRange
                        from buff in ally.Buffs
                        where buff.Name.Contains("kalistacoopstrikeally")
                        select ally) 
                {
                    CoopStrikeAlly = ally;
                }
                if (W.Level != 0)
                {
                    Drawing.DrawText(Drawing.Width*0.44f, Drawing.Height*0.80f, Color.Red, "Searching Your Friend...");
                }
            }
            else
            {
                var drawConnText = GetValue<Circle>("DrawConnText");
                if (drawConnText.Active)
                {
                    Drawing.DrawText(Drawing.Width*0.44f, Drawing.Height*0.80f, drawConnText.Color,
                        "You Connected with " + CoopStrikeAlly.ChampionName);
                }

                var drawConnSignal = GetValue<bool>("DrawConnSignal");
                if (drawConnSignal)
                {
                    if (ObjectManager.Player.Distance(CoopStrikeAlly) > 800 &&
                        ObjectManager.Player.Distance(CoopStrikeAlly) < CoopStrikeAllyRange)
                    {
                        Drawing.DrawText(Drawing.Width*0.45f, Drawing.Height*0.82f, Color.Gold,
                            "Connection Signal: Low");
                    }
                    else if (ObjectManager.Player.Distance(CoopStrikeAlly) < 800)
                    {
                        Drawing.DrawText(Drawing.Width*0.45f, Drawing.Height*0.82f, Color.GreenYellow,
                            "Connection Signal: Good");

                    }
                    else if (ObjectManager.Player.Distance(CoopStrikeAlly) > CoopStrikeAllyRange)
                    {
                        Drawing.DrawText(Drawing.Width*0.45f, Drawing.Height*0.82f, Color.Red,
                            "Connection Signal: None");
                    }
                }
            }
            
            var drawEStackCount = GetValue<Circle>("DrawEStackCount");
            if (drawEStackCount.Active)
            {

                MarkedChampions.Clear();
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(tx => tx.IsEnemy && !tx.IsDead && ObjectManager.Player.Distance(tx) <= E.Range))
                {
                    foreach (var buff in enemy.Buffs.Where(buff => buff.Name.Contains("kalistaexpungemarker")))
                    {
                        MarkedChampions.Add(enemy.ChampionName, buff.Count);
                    }
                }

                foreach (var markedEnemies in MarkedChampions)
                {
                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (enemy.IsEnemy && !enemy.IsDead && ObjectManager.Player.Distance(enemy) <= E.Range &&
                            enemy.ChampionName == markedEnemies.Key)
                        {
                            var display = string.Format("E:{0}", markedEnemies.Value);
                            Drawing.DrawText(enemy.HPBarPosition.X + 145, enemy.HPBarPosition.Y + 20, drawEStackCount.Color,
                                display);
                        }
                    }
                }
            }
    
            Obj_AI_Hero t;

            if (Q.IsReady() && GetValue<KeyBind>("UseQTH").Active)
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (t != null)
                    Q.Cast(t);
            }

            if (ComboActive || HarassActive)
            {
                var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
                var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));

                if (Orbwalking.CanMove(100))
                {
                    if (Q.IsReady() && useQ)
                    {
                        t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                        if (t != null)
                            Q.Cast(t);
                    }
                    
                    if (E.IsReady() && useE)
                    {
                        t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                        if (t != null)
                        {
                            if (t.Health <= GetEDamage(t))
                            {
                                E.Cast();
                            }
                        }
                    }
                }
            }

            if (!R.IsReady() || !GetValue<KeyBind>("CastR").Active) return;
            t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
            if (t != null)
                R.Cast(t);
        }

        public override bool ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            return true;
        }

        public override bool HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(
                new MenuItem("UseQTH" + Id, "Use Q (Toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            return true;
        }

        public override bool MiscMenu(Menu config)
        {

            return true;
        }

        public override bool DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(new MenuItem("Dx" + Id, ""));
            config.AddItem(
                new MenuItem("DrawConnMax" + Id, "Connection range").SetValue(new Circle(false,
                    Color.FromArgb(100, 255, 255, 255))));
            config.AddItem(
                new MenuItem("DrawConnText" + Id, "Connection Text").SetValue(new Circle(false, Color.GreenYellow)));
            config.AddItem(
                new MenuItem("DrawConnSignal" + Id, "Connection Signal").SetValue(true));
            config.AddItem(new MenuItem("Dx" + Id, ""));
            config.AddItem(
                new MenuItem("DrawEStackCount" + Id, "E Stack Count").SetValue(new Circle(false, Color.Firebrick)));
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {

            return true;
        }

    }
}
