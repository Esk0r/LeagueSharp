#region

using System;
using System.IO;
using System.Xml;
using LeagueSharp;
using LeagueSharp.Common;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Marksman
{
    internal class EnemyMarker
    {
        public string ChampionName { get; set; }
        public double ExpireTime { get; set; }
        public int BuffCount { get; set; }
    }

    internal class Kalista : Champion
    {
        public Spell Q;
        public Spell W;
        public static Spell E;
        public Spell R;

        public static Dictionary<Vector3, Vector3> jumpPos;

        public Obj_AI_Hero CoopStrikeAlly;
        public float CoopStrikeAllyRange = 1250f;
        public Dictionary<String, int> MarkedChampions = new Dictionary<String, int>();
        private static readonly List<EnemyMarker> xEnemyMarker = new List<EnemyMarker>();

        public Kalista()
        {
            Utils.PrintMessage("Kalista loaded.");

            Q = new Spell(SpellSlot.Q, 1150);
            W = new Spell(SpellSlot.W, 5500);
            E = new Spell(SpellSlot.E, 950);
            R = new Spell(SpellSlot.R, 1250);

            Q.SetSkillshot(0.25f, 40f, 1300f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);
        }

        public override void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = {Q, W, E, R};
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active && spell.Level > 0)
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }

            var drawConn = GetValue<Circle>("DrawConnMax");
            if (drawConn.Active)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, CoopStrikeAllyRange, drawConn.Color);

            var drawJumpPos = GetValue<Circle>("DrawJumpPos");
            if (drawJumpPos.Active)
            {
                foreach (KeyValuePair<Vector3, Vector3> pos in jumpPos)
                {
                    if (ObjectManager.Player.Distance(pos.Key) <= 500f ||
                        ObjectManager.Player.Distance(pos.Value) <= 500f)

                    {
                        Drawing.DrawCircle(pos.Key, 75f, drawJumpPos.Color);
                        Drawing.DrawCircle(pos.Value, 75f, drawJumpPos.Color);
                    }
                    if (ObjectManager.Player.Distance(pos.Key) <= 35f ||
                        ObjectManager.Player.Distance(pos.Value) <= 35f)
                    {
                        Render.Circle.DrawCircle(pos.Key, 70f, Color.GreenYellow);
                        Render.Circle.DrawCircle(pos.Value, 70f, Color.GreenYellow);
                    }
                }
            }
        }

        public void JumpTo()
        {
            if (!Q.IsReady())
            {
                Drawing.DrawText(Drawing.Width*0.44f, Drawing.Height*0.80f, Color.Red,
                    "Q is not ready! You can not Jump!");
                return;
            }

            Drawing.DrawText(Drawing.Width*0.39f, Drawing.Height*0.80f, Color.White,
                "Jumping Mode is Active! Go to the nearest jump point!");

            foreach (var xTo in from pos in jumpPos
                where ObjectManager.Player.Distance(pos.Key) <= 35f ||
                      ObjectManager.Player.Distance(pos.Value) <= 35f
                let xTo = pos.Value
                select ObjectManager.Player.Distance(pos.Key) < ObjectManager.Player.Distance(pos.Value)
                    ? pos.Value
                    : pos.Key)
            {
                Q.Cast(new Vector2(xTo.X, xTo.Y), true);
                //Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(xTo.X, xTo.Y)).Send();
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, xTo);
            }
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

        private static float GetEDamage(Obj_AI_Base t)
        {
            if (!E.IsReady())
                return 0f;
            return (float) ObjectManager.Player.GetSpellDamage(t, SpellSlot.E);
            
            /* I think this calculation working good but i cant check now. after I'll do */ 
            var buff = t.Buffs.FirstOrDefault(xBuff => xBuff.DisplayName.ToLower() == "kalistaexpungemarker");
            if (buff.Count == 0) 
                return 0f;

            double damage = ObjectManager.Player.FlatPhysicalDamageMod + ObjectManager.Player.BaseAttackDamage;
            double eDmg = damage * 0.60 + new double[] {0, 20, 30, 40, 50, 60}[E.Level];
            
            if (buff.Count == 1) 
                return (float)eDmg;
            
            damage += buff.Count * 0.003 * damage + eDmg;
            return (float) ObjectManager.Player.CalcDamage(t, Damage.DamageType.Physical, damage);
        }
        
        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (GetValue<Circle>("DrawJumpPos").Active)
                fillPositions();

            if (GetValue<KeyBind>("JumpTo").Active)
            {
                JumpTo();
            }

            foreach (
                var myBoddy in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(
                            obj => obj.Name == "RobotBuddy" &&
                                   obj.IsAlly && ObjectManager.Player.Distance(obj) < 1500))

            {
                Render.Circle.DrawCircle(myBoddy.Position, 75f, Color.Red);
            }
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
                    Drawing.DrawText(Drawing.Width*0.44f, Drawing.Height*0.80f, Color.Red, "Searching Your Friend...");
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
                xEnemyMarker.Clear();
                foreach (
                    var xEnemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(tx => tx.IsEnemy && !tx.IsDead && ObjectManager.Player.Distance(tx) < E.Range))
                {
                    foreach (var buff in xEnemy.Buffs.Where(buff => buff.Name.Contains("kalistaexpungemarker")))
                    {
                        xEnemyMarker.Add(new EnemyMarker
                        {
                            ChampionName = xEnemy.ChampionName,
                            ExpireTime = Game.Time + 4,
                            BuffCount = buff.Count
                        });
                    }
                }

                foreach (var markedEnemies in xEnemyMarker)
                {
                    foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                    {
                        if (enemy.IsEnemy && !enemy.IsDead && ObjectManager.Player.Distance(enemy) <= E.Range &&
                            enemy.ChampionName == markedEnemies.ChampionName)
                        {
                            if (!(markedEnemies.ExpireTime > Game.Time)) continue;
                            var xCoolDown = TimeSpan.FromSeconds(markedEnemies.ExpireTime - Game.Time);
                            var display = string.Format("E:{0}", markedEnemies.BuffCount );
                            Drawing.DrawText(enemy.HPBarPosition.X + 145, enemy.HPBarPosition.Y + 20,
                                drawEStackCount.Color,
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
                t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
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
                        t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                        if (t != null)
                            Q.Cast(t);
                    }

                    if (E.IsReady() && useE)
                    {
                        t = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                        if (t != null)
                        {
                            if (t.Health < ObjectManager.Player.GetSpellDamage(t, SpellSlot.E))
                            {
                                E.Cast();
                            }
                        }
                    }
                }
            }

            if (!R.IsReady() || !GetValue<KeyBind>("CastR").Active) return;
            t = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
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
            config.AddItem(
                new MenuItem("JumpTo" + Id, "JumpTo").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
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
            config.AddItem(new MenuItem("Dx", ""));
            config.AddItem(
                new MenuItem("DrawJumpPos" + Id, "Jump Positions").SetValue(new Circle(false, Color.HotPink)));

            var damageAfterE = new MenuItem("DamageAfterE", "Damage After E").SetValue(true);
            config.AddItem(damageAfterE);

            Utility.HpBarDamageIndicator.DamageToUnit = GetEDamage;
            Utility.HpBarDamageIndicator.Enabled = damageAfterE.GetValue<bool>();
            damageAfterE.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };
            
            return true;
        }

        public override bool ExtrasMenu(Menu config)
        {
            return true;
        }


        public override bool LaneClearMenu(Menu config)
        {
            return true;
        }

        public static void fillPositions()
        {

        }
    }
}
