using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Drawing;
using Color = System.Drawing.Color;

namespace Azir
{
    class Program
    {
        public static Obj_AI_Hero Player;
        public static Menu Menu;
        public static AzirWalker AzirWalker;

        public static Spell Q;
        public static Spell Qline;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        public static SpellSlot IgniteSlot;

        private static int _allinT = 0;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.ChampionName != "Azir") return;

            #region Spells
            Q = new Spell(SpellSlot.Q, 825);
            Qline = new Spell(SpellSlot.Q, 825);

            W = new Spell(SpellSlot.W, 450);
            E = new Spell(SpellSlot.E, 1250);
            R = new Spell(SpellSlot.R, 450);

            Q.SetSkillshot(0, 70, 1600, false, SkillshotType.SkillshotCircle);
            Qline.SetSkillshot(0, 70, 1600, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0, 100, 1700, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.5f, 0, 1400, false, SkillshotType.SkillshotLine);

            IgniteSlot = Player.GetSpellSlot("SummonerDot");
            #endregion

            #region Menu
            Menu = new Menu("Azir", "Azir", true);

            TargetSelector.AddToMenu(Menu.SubMenu("Target Selector"));
            AzirWalker = new AzirWalker(Menu.SubMenu("Orbwalker"));

            Menu.SubMenu("Combo").AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseWC", "Use W").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("UseIgnite", "Use Ignite").SetValue(true));
            Menu.SubMenu("Combo").AddItem(new MenuItem("AllInKEK", "All-in (tap)!").SetValue(new KeyBind('G', KeyBindType.Press)));
            Menu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu.SubMenu("Harass").AddItem(new MenuItem("HarassMinMana", "Min mana %").SetValue(new Slider(20, 0, 100)));
            Menu.SubMenu("Harass").AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind('C', KeyBindType.Press)));

            Menu.SubMenu("LaneClear").AddItem(new MenuItem("UseQLC", "Use Q").SetValue(true));
            Menu.SubMenu("LaneClear").AddItem(new MenuItem("UseWLC", "Use W").SetValue(true));
            Menu.SubMenu("LaneClear").AddItem(new MenuItem("LaneClearActive", "LaneClear!").SetValue(new KeyBind('V', KeyBindType.Press)));

            Menu.SubMenu("Misc").AddItem(new MenuItem("Jump", "Jump towards cursor").SetValue(new KeyBind('E', KeyBindType.Press)));
            Menu.SubMenu("Misc").Item("Jump").ValueChanged += Program_ValueChanged;
            Menu.SubMenu("Misc").AddItem(new MenuItem("AutoEInterrupt", "Interrupt targets with E").SetValue(false));

            Menu.SubMenu("R").AddItem(new MenuItem("AutoRN", "Auto R if it will hit >=").SetValue(new Slider(3, 1, 6)));
            Menu.SubMenu("R").AddItem(new MenuItem("AutoRInterrupt", "Interrupt targets with R").SetValue(true));

            var dmgAfterComboItem = new MenuItem("DamageAfterR", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit += hero => GetComboDamage(hero);
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            Menu.SubMenu("Drawings").AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.Yellow))));
            Menu.SubMenu("Drawings").AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(150, Color.Yellow))));
            Menu.SubMenu("Drawings").AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.Yellow))));
            Menu.SubMenu("Drawings").AddItem(dmgAfterComboItem);

            Menu.AddToMainMenu();
            #endregion
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Program_ValueChanged(object sender, OnValueChangeEventArgs e)
        {
            if(e.GetNewValue<KeyBind>().Active)
            {
                Jumper.Jump();
            }
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if(args.DangerLevel != Interrupter2.DangerLevel.High)
            {
                return;
            }

            if(Menu.SubMenu("Misc").Item("AutoEInterrupt").GetValue<bool>() && E.IsReady())
            {
                foreach (var soldier in SoldiersManager.AllSoldiers.Where(s => Player.Distance(s, true) < E.RangeSqr))
                {
                    if (E.WillHit(sender, soldier.ServerPosition))
                    {
                        E.Cast(soldier.ServerPosition);
                        return;
                    }
                }
                return;
            }

            if (Menu.SubMenu("R").Item("AutoRInterrupt").GetValue<bool>() && R.IsReady())
            {
                var dist = Player.Distance(sender, true);

                if(dist < R.RangeSqr)
                {
                    R.Cast(sender, false, true);
                    return;
                }
                
                if(dist < Math.Pow(Math.Sqrt(R.RangeSqr + Math.Pow(R.Width + sender.BoundingRadius, 2)) , 2))
                {
                    var angle = (float) Math.Atan(R.Width + sender.BoundingRadius / R.Range);
                    var p = (sender.ServerPosition.To2D() - Player.ServerPosition.To2D()).Rotated(angle);
                    R.Cast(p);
                }
            }
        }

        static float GetComboDamage(Obj_AI_Base target)
        {
            var damage = 0d;
            if (Q.IsReady())
            {
                damage += Player.GetSpellDamage(target, SpellSlot.Q);
            }
            
            if (E.IsReady())
            {
                damage += Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (R.IsReady())
            {
                damage += Player.GetSpellDamage(target, SpellSlot.R);
            }

            if(IgniteSlot != SpellSlot.Unknown && Player.Spellbook.GetSpell(IgniteSlot).State == SpellState.Ready)
            {
                damage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
            }

            damage += SoldiersManager.ActiveSoldiers.Count * Player.GetSpellDamage(target, SpellSlot.W);

            return (float)damage;
        }

        static void LaneClear()
        {
            var useQ = Menu.SubMenu("LaneClear").Item("UseQLC").GetValue<bool>();
            var useW = Menu.SubMenu("LaneClear").Item("UseWLC").GetValue<bool>();

            var minions = MinionManager.GetMinions(Q.Range);
            if(minions.Count == 0)
            {
                minions = MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            }

            if (minions.Count > 0)
            {
                if (useW && W.Instance.Ammo > 0 && (minions.Count > 2 || minions[0].Team == GameObjectTeam.Neutral))
                {
                    var p = Player.Position.To2D().Extend(minions[0].Position.To2D(), W.Range);
                    W.Cast(p);
                    return;
                }

                if (useQ && Qline.IsReady() && (minions.Count >= 2 || minions[0].Team == GameObjectTeam.Neutral))
                {
                    var positions = new Dictionary<Vector3, int>();

                    foreach (var soldier in SoldiersManager.AllSoldiers)
                    {
                        Qline.UpdateSourcePosition(soldier.ServerPosition, ObjectManager.Player.ServerPosition);
                        foreach (var minion in minions)
                        {
                            var hits = Qline.CountHits(minions.Select(m => m.ServerPosition).ToList(), minion.ServerPosition);
                            if (hits >= 2 || minions[0].Team == GameObjectTeam.Neutral)
                            {
                                if(!positions.ContainsKey(minion.ServerPosition))
                                {
                                    positions.Add(minion.ServerPosition, hits);
                                }
                            }
                        }
                    }

                    if(positions.Count > 0)
                    {
                        Qline.Cast(positions.MaxOrDefault(k => k.Value).Key);
                    }
                }
                return;
            }
        }

        static void Harass()
        {
            var harassTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (harassTarget == null)
            {
                return;
            }

            if(W.Instance.Ammo > 0)
            {
                var p = Player.Position.To2D().Extend(harassTarget.Position.To2D(), W.Range);
                if(Q.IsReady() || HeroManager.Enemies.Any(h => h.IsValidTarget(W.Range + 200)))
                {
                    W.Cast(p);
                }
                return;
            }

            if(Q.IsReady())
            {
                var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if(qTarget != null)
                {
                    foreach (var soldier in SoldiersManager.AllSoldiers)
                    {
                        Q.UpdateSourcePosition(soldier.ServerPosition, ObjectManager.Player.ServerPosition);
                        Q.Cast(qTarget);
                    }
                }
            }
        }

        static void Combo()
        {
            var useQ = Menu.SubMenu("Combo").Item("UseQC").GetValue<bool>();
            var useW = Menu.SubMenu("Combo").Item("UseWC").GetValue<bool>();
            var useE = Menu.SubMenu("Combo").Item("UseEC").GetValue<bool>();
            var useR = (Utils.TickCount - _allinT < 4000) && Menu.SubMenu("Combo").Item("UseRC").GetValue<bool>();
            
            var qTarget = TargetSelector.GetTarget(Q.Range + 200, TargetSelector.DamageType.Magical);
            if (qTarget == null)
            {
                return;
            }

            if (useQ && Q.IsReady())
            {
                foreach (var soldier in SoldiersManager.AllSoldiers)
                {
                    Q.UpdateSourcePosition(soldier.ServerPosition, ObjectManager.Player.ServerPosition);
                    Q.Cast(qTarget);
                }
            }

            if (useW && W.Instance.Ammo > 0)
            {
                var p = Player.Distance(qTarget, true) > W.RangeSqr ? Player.Position.To2D().Extend(qTarget.Position.To2D(), W.Range) : qTarget.Position.To2D();
                W.Cast(p);
            }

            if (useE && ((Utils.TickCount - _allinT) < 4000 || (HeroManager.Enemies.Count(e => e.IsValidTarget(1000)) <= 2 && GetComboDamage(qTarget) > qTarget.Health)) && E.IsReady())
            {
                foreach (var soldier in SoldiersManager.AllSoldiers2.Where(s => Player.Distance(s, true) < E.RangeSqr))
                {
                    if(E.WillHit(qTarget, soldier.ServerPosition))
                    {
                        E.Cast(soldier.ServerPosition);
                        return;
                    }
                }
            }

            if (GetComboDamage(qTarget) > qTarget.Health)
            {
                if (useR && R.IsReady())
                {
                    R.Cast(qTarget, false, true);
                }

                if (Menu.SubMenu("Combo").Item("UseIgnite").GetValue<bool>() && IgniteSlot != SpellSlot.Unknown && Player.GetSpell(IgniteSlot).State == SpellState.Ready && Player.Distance(qTarget, true) < 600 * 600)
                {
                    Player.Spellbook.CastSpell(IgniteSlot, qTarget);
                }
            }
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            R.Width = 133 * (3 + R.Level);
            
            var minTargets = Menu.SubMenu("R").Item("AutoRN").GetValue<Slider>().Value;
            if(minTargets != 6)
            {
                R.CastIfWillHit(R.GetTarget(), minTargets);
            }

            if (Menu.SubMenu("Combo").Item("AllInKEK").GetValue<KeyBind>().Active)
            {
                _allinT = Utils.TickCount;
            }

            if (Menu.SubMenu("Harass").Item("HarassActive").GetValue<KeyBind>().Active && Player.ManaPercentage() > Menu.SubMenu("Harass").Item("HarassMinMana").GetValue<Slider>().Value)
            {
                Harass();
                return;
            }

            if (Menu.SubMenu("Combo").Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
                return;
            }

            if(Menu.SubMenu("LaneClear").Item("LaneClearActive").GetValue<KeyBind>().Active)
            {
                LaneClear();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qCircle = Menu.SubMenu("Drawings").Item("QRange").GetValue<Circle>();
            if (qCircle.Active)
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, qCircle.Color);
            }

            var wCircle = Menu.SubMenu("Drawings").Item("WRange").GetValue<Circle>();
            if (wCircle.Active)
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, wCircle.Color);
            }

            var rCircle = Menu.SubMenu("Drawings").Item("RRange").GetValue<Circle>();
            if (rCircle.Active)
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, rCircle.Color);
            }
        }
    }
}
