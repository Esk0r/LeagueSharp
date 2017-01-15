#region
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using TwistedFate.Common;
using Color = System.Drawing.Color;
#endregion

namespace TwistedFate
{
    internal class Program
    {
        public static Menu Config;

        private static Spell Q;
        private static readonly float Qangle = 28*(float) Math.PI/180;
        private static Orbwalking.Orbwalker SOW;
        private static Vector2 PingLocation;
        private static int LastPingT = 0;
        private static Obj_AI_Hero Player;
        private static int CastQTick;
        public static int PickTick = 0;
        public static Menu MenuTools { get; private set; }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Ping(Vector2 position)
        {
            if (Utils.TickCount - LastPingT < 30*1000) 
            {
                return;
            }
            
            LastPingT = Utils.TickCount;
            PingLocation = position;
            SimplePing();

            Utility.DelayAction.Add(150, SimplePing);
            Utility.DelayAction.Add(300, SimplePing);
            Utility.DelayAction.Add(400, SimplePing);
            Utility.DelayAction.Add(800, SimplePing);
        }

        private static void SimplePing()
        {
            Game.ShowPing(PingCategory.Fallback, PingLocation, true);
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "TwistedFate") return;
            Player = ObjectManager.Player;
            Q = new Spell(SpellSlot.Q, 1450);
            Q.SetSkillshot(0.25f, 40f, 1000f, false, SkillshotType.SkillshotLine);

            //Make the menu
            Config = new Menu("Twisted Fate", "TwistedFate", true);

            MenuTools = new Menu("Tools", "Tools");
            Config.AddSubMenu(MenuTools);
            

            var SowMenu = new Menu("Orbwalking", "Orbwalking");
            SOW = new Orbwalking.Orbwalker(SowMenu);
            MenuTools.AddSubMenu(SowMenu);

            /* Q */
            var q = new Menu("Q - Wildcards", "Q");
            {
                q.AddItem(new MenuItem("AutoQI", "Auto-Q immobile").SetValue(true));
                q.AddItem(new MenuItem("AutoQD", "Auto-Q dashing").SetValue(true));
                q.AddItem(
                    new MenuItem("CastQ", "Cast Q (tap)").SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Press)));
                Config.AddSubMenu(q);
            }

            /* W */
            var w = new Menu("W - Pick a card", "W");
            {
                w.AddItem(
                    new MenuItem("SelectYellow", "Select Yellow").SetValue(new KeyBind("W".ToCharArray()[0],
                        KeyBindType.Press)));
                w.AddItem(
                    new MenuItem("SelectBlue", "Select Blue").SetValue(new KeyBind("E".ToCharArray()[0],
                        KeyBindType.Press)));
                w.AddItem(
                    new MenuItem("SelectRed", "Select Red").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
                Config.AddSubMenu(w);
            }

            var menuLane = new Menu("Lane", "Lane");
            {
                menuLane.AddItem(new MenuItem("Lane.BlueCard.MinMana", "Keep up mana > % [0 = Off]").SetValue(new Slider(50, 0, 100)));
                Config.AddSubMenu(menuLane);
            }

            var menuItems = new Menu("Items", "Items");
            {
                menuItems.AddItem(new MenuItem("itemBotrk", "Botrk").SetValue(true));
                menuItems.AddItem(new MenuItem("itemYoumuu", "Youmuu").SetValue(true));
                menuItems.AddItem(
                    new MenuItem("itemMode", "Use items on").SetValue(
                        new StringList(new[] {"No", "Mixed mode", "Combo mode", "Both"}, 2)));
                Config.AddSubMenu(menuItems);
            }

            var r = new Menu("R - Destiny", "R");
            {
                r.AddItem(new MenuItem("AutoY", "Select yellow card after R").SetValue(true));
                Config.AddSubMenu(r);
            }

            var misc = new Menu("Misc", "Misc");
            {
                misc.AddItem(new MenuItem("PingLH", "Ping low health enemies (Only local)").SetValue(true));
                misc.AddItem(new MenuItem("Misc.InstantSelection", "Anti-Cheat: Never select card instantly").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Toggle))).SetFontStyle(FontStyle.Regular, SharpDX.Color.Aqua).Permashow();
                misc.AddItem(new MenuItem("Misc.SelectRandomCard", "Anti-Cheat: Select a random card for killable enemy").SetValue(false)).SetFontStyle(FontStyle.Regular, SharpDX.Color.Aqua).Permashow();
                misc.AddItem(new MenuItem("Misc.SelectGoldCardInRisk", "Anti-Cheat: Select instantly Gold Card if I'm in risk").SetValue(false)).SetFontStyle(FontStyle.Regular, SharpDX.Color.Aqua).Permashow();
                Config.AddSubMenu(misc);
            }

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit = ComboDamage;
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            /*Drawing*/
            var drawings = new Menu("Drawings", "Drawings");
            {
                drawings.AddItem(
                    new MenuItem("Qcircle", "Q Range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("Rcircle", "R Range").SetValue(new Circle(true, Color.FromArgb(100, 255, 255, 255))));
                drawings.AddItem(
                    new MenuItem("Rcircle2", "R Range (minimap)").SetValue(new Circle(true,
                        Color.FromArgb(255, 255, 255, 255))));
                drawings.AddItem(dmgAfterComboItem);
                Config.AddSubMenu(drawings);
            }

            Config.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddToMainMenu();

            var x = new CommonBuffManager();
            CommonGeometry.Init();


            CommonAutoLevel.Init(MenuTools);
            CommonSkins.Init(MenuTools);

            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnDraw += Drawing_OnDraw_PassiveTimes;
            Drawing.OnEndScene += DrawingOnOnEndScene;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;
        }

        private static void Drawing_OnDraw_PassiveTimes(EventArgs args)
        {
            var passiveBuffs =
                     (from b in ObjectManager.Player.Buffs
                      join b1 in CommonBuffManager.PassiveBuffs on b.DisplayName equals b1.BuffName
                      select new { b, b1 }).Distinct();

            foreach (var buffName in passiveBuffs)
            {
                //Game.PrintChat(buffName.b1.BuffName);
                for (int i = 0; i < passiveBuffs.Count(); i++)
                {
                    if (buffName.b.EndTime >= Game.Time)
                    {
                        CommonGeometry.DrawBox(new Vector2(ObjectManager.Player.HPBarPosition.X + 10, (i * 8) + ObjectManager.Player.HPBarPosition.Y + 32), 130, 6, Color.FromArgb(100, 255, 200, 37), 1, Color.Black);

                        var buffTime = buffName.b.EndTime - buffName.b.StartTime;
                        CommonGeometry.DrawBox(new Vector2(ObjectManager.Player.HPBarPosition.X + 11, (i * 8) + ObjectManager.Player.HPBarPosition.Y + 33), (130 / buffTime) * (buffName.b.EndTime - Game.Time), 4, buffName.b1.Color, 1, buffName.b1.Color);

                        TimeSpan timeSpan = TimeSpan.FromSeconds(buffName.b.EndTime - Game.Time);
                        var timer = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                        CommonGeometry.DrawText(CommonGeometry.TextPassive, timer, ObjectManager.Player.HPBarPosition.X + 142, (i * 8) + ObjectManager.Player.HPBarPosition.Y + 29, SharpDX.Color.Wheat);
                    }
                }
            }

            foreach (var hero in HeroManager.AllHeroes)
            {
                var jungleBuffs = (from b in hero.Buffs join b1 in CommonBuffManager.JungleBuffs on b.DisplayName equals b1.BuffName select new {b, b1}).Distinct();

                foreach (var buffName in jungleBuffs.ToList())
                {
                    var nDiff1 = buffName.b.EndTime - buffName.b.StartTime < 10 ? (Game.Time - buffName.b.StartTime) * 10 : Game.Time - buffName.b.StartTime;
                    var nDiff2 = buffName.b.EndTime - buffName.b.StartTime < 10 ? (buffName.b.EndTime - buffName.b.StartTime) * 10 : buffName.b.EndTime - buffName.b.StartTime;
                    var circle1 = new CommonGeometry.Circle2(new Vector2(hero.Position.X + 3, hero.Position.Y - 3), 140 + (buffName.b1.Number*20), nDiff1, nDiff2).ToPolygon();
                    circle1.Draw(Color.Black, 3);

                    var circle = new CommonGeometry.Circle2(hero.Position.To2D(), 140 + (buffName.b1.Number*20), nDiff1, nDiff2).ToPolygon(); circle.Draw(buffName.b1.Color, 3);
                }
            }
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
                args.Process = CardSelector.Status != SelectStatus.Selecting &&
                               Utils.TickCount - CardSelector.LastWSent > 300;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.Equals("Gate", StringComparison.InvariantCultureIgnoreCase) && Config.Item("AutoY").GetValue<bool>())
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
        }

        private static void DrawingOnOnEndScene(EventArgs args)
        {
            var rCircle2 = Config.Item("Rcircle2").GetValue<Circle>();
            if (rCircle2.Active)
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 5500, rCircle2.Color, 1, 23, true);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qCircle = Config.Item("Qcircle").GetValue<Circle>();
            var rCircle = Config.Item("Rcircle").GetValue<Circle>();

            if (qCircle.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, qCircle.Color);
            }

            if (rCircle.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, 5500, rCircle.Color);
            }
        }


        private static int CountHits(Vector2 position, List<Vector2> points, List<int> hitBoxes)
        {
            var result = 0;

            var startPoint = ObjectManager.Player.ServerPosition.To2D();
            var originalDirection = Q.Range*(position - startPoint).Normalized();
            var originalEndPoint = startPoint + originalDirection;

            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];

                for (var k = 0; k < 3; k++)
                {
                    var endPoint = new Vector2();
                    if (k == 0) endPoint = originalEndPoint;
                    if (k == 1) endPoint = startPoint + originalDirection.Rotated(Qangle);
                    if (k == 2) endPoint = startPoint + originalDirection.Rotated(-Qangle);

                    if (point.Distance(startPoint, endPoint, true, true) <
                        (Q.Width + hitBoxes[i])*(Q.Width + hitBoxes[i]))
                    {
                        result++;
                        break;
                    }
                }
            }

            return result;
        }

        private static void CastQ(Obj_AI_Base unit, Vector2 unitPosition, int minTargets = 0)
        {
            var points = new List<Vector2>();
            var hitBoxes = new List<int>();

            var startPoint = ObjectManager.Player.ServerPosition.To2D();
            var originalDirection = Q.Range*(unitPosition - startPoint).Normalized();

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsValidTarget() && enemy.NetworkId != unit.NetworkId)
                {
                    var pos = Q.GetPrediction(enemy);
                    if (pos.Hitchance >= HitChance.Medium)
                    {
                        points.Add(pos.UnitPosition.To2D());
                        hitBoxes.Add((int) enemy.BoundingRadius);
                    }
                }
            }

            var posiblePositions = new List<Vector2>();

            for (var i = 0; i < 3; i++)
            {
                if (i == 0) posiblePositions.Add(unitPosition + originalDirection.Rotated(0));
                if (i == 1) posiblePositions.Add(startPoint + originalDirection.Rotated(Qangle));
                if (i == 2) posiblePositions.Add(startPoint + originalDirection.Rotated(-Qangle));
            }


            if (startPoint.Distance(unitPosition) < 900)
            {
                for (var i = 0; i < 3; i++)
                {
                    var pos = posiblePositions[i];
                    var direction = (pos - startPoint).Normalized().Perpendicular();
                    var k = (2/3*(unit.BoundingRadius + Q.Width));
                    posiblePositions.Add(startPoint - k*direction);
                    posiblePositions.Add(startPoint + k*direction);
                }
            }

            var bestPosition = new Vector2();
            var bestHit = -1;

            foreach (var position in posiblePositions)
            {
                var hits = CountHits(position, points, hitBoxes);
                if (hits > bestHit)
                {
                    bestPosition = position;
                    bestHit = hits;
                }
            }

            if (bestHit + 1 <= minTargets)
                return;

            Q.Cast(bestPosition.To3D(), true);
        }

        private static float ComboDamage(Obj_AI_Hero hero)
        {
            var dmg = 0d;
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q)*2;
            dmg += Player.GetSpellDamage(hero, SpellSlot.W);
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q);

            if (ObjectManager.Player.GetSpellSlot("SummonerIgnite") != SpellSlot.Unknown)
            {
                dmg += ObjectManager.Player.GetSummonerSpellDamage(hero, Damage.SummonerSpell.Ignite);
            }

            return (float) dmg;
        }

        static double GetCardDamage(int slot)
        {
            var dmg = 0d;
            int[] blue = new[] {40, 60, 80, 100, 120};
            int[] red = new[] { 30, 45, 60, 75, 90 };
            double[] gold = new[] { 15, 22.5, 30, 37.5, 45 };


            var dmg1 = ObjectManager.Player.TotalAttackDamage + (ObjectManager.Player.TotalMagicalDamage / 2);

            if (slot == 0)
            {
                dmg = blue[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level - 1] + dmg1;
            }
            else if (slot == 1)
            {
                dmg = red[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level - 1] + dmg1;
            }
            else if (slot == 2)
            {
                dmg = gold[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Level - 1] + dmg1;
            }

            return dmg;
        }


        static void SelectACard(Cards aCard)
        {
            if (PickTick == 0)
            {
                PickTick = Utils.TickCount;
            }

            CardSelector.StartSelecting(aCard);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("PingLH").GetValue<bool>())
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready &&
                                    h.IsValidTarget() && ComboDamage(h) > h.Health))
                {
                    Ping(enemy.Position.To2D());
                }

            if (Config.Item("CastQ").GetValue<KeyBind>().Active)
            {
                CastQTick = Utils.TickCount;
            }

            if (Utils.TickCount - CastQTick < 500)
            {
                var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (qTarget != null)
                {
                    Q.Cast(qTarget);
                }
            }

            var combo = Config.Item("Combo").GetValue<KeyBind>().Active;

            //Choose a random card if enemy killable
            if (combo && Config.Item("Misc.SelectRandomCard").GetValue<bool>())
            {
                var wName = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name;
                var t = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(null) + 65, TargetSelector.DamageType.Magical);
                if (t.IsValidTarget() && ObjectManager.Player.CanAttack)
                {
                    if (
                        (wName.Equals("Bluecardlock", StringComparison.InvariantCultureIgnoreCase) && t.Health <= GetCardDamage(0))
                        || (wName.Equals("Redcardlock", StringComparison.InvariantCultureIgnoreCase) && t.Health <= GetCardDamage(1))
                        || (wName.Equals("Goldcardlock", StringComparison.InvariantCultureIgnoreCase) && t.Health <= GetCardDamage(2))
                       )
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
                        Program.PickTick = 0;
                    }
                }
            }

            if (SOW.ActiveMode != Orbwalking.OrbwalkingMode.Combo && SOW.ActiveMode != Orbwalking.OrbwalkingMode.None)
            {
                if (Config.Item("Lane.BlueCard.MinMana").GetValue<Slider>().Value > 0 && Player.ManaPercent < Config.Item("Lane.BlueCard.MinMana").GetValue<Slider>().Value)
                {
                    var minions = MinionManager.GetMinions(Orbwalking.GetRealAutoAttackRange(null) + 165);
                    {
                        if (minions.Count > 0)
                        {
                            SelectACard(Cards.Blue);
                        }
                    }
                }

            }

            //Select cards.
            if (Config.Item("SelectYellow").GetValue<KeyBind>().Active || combo)
            {
                SelectACard(Cards.Yellow);
            }

            if (Config.Item("SelectBlue").GetValue<KeyBind>().Active)
            {
                SelectACard(Cards.Blue);
            }

            if (Config.Item("SelectRed").GetValue<KeyBind>().Active)
            {
                SelectACard(Cards.Red);
            }
/*
            if (CardSelector.Status == SelectStatus.Selected && combo)
            {
                var target = SOW.GetTarget();
                if (target.IsValidTarget() && target is Obj_AI_Hero && Items.HasItem("DeathfireGrasp") && ComboDamage((Obj_AI_Hero)target) >= target.Health)
                {
                    Items.UseItem("DeathfireGrasp", (Obj_AI_Hero) target);
                }
            }
*/

            //Auto Q
            var autoQI = Config.Item("AutoQI").GetValue<bool>();
            var autoQD = Config.Item("AutoQD").GetValue<bool>();

            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready && (autoQD || autoQI))
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget(Q.Range*2))
                    {
                        var pred = Q.GetPrediction(enemy);
                        if ((pred.Hitchance == HitChance.Immobile && autoQI) ||
                            (pred.Hitchance == HitChance.Dashing && autoQD))
                        {
                            CastQ(enemy, pred.UnitPosition.To2D());
                        }
                    }
                }


            var useItemModes = Config.Item("itemMode").GetValue<StringList>().SelectedIndex;
            if (
                !((SOW.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                   (useItemModes == 2 || useItemModes == 3))
                  ||
                  (SOW.ActiveMode == Orbwalking.OrbwalkingMode.Mixed &&
                   (useItemModes == 1 || useItemModes == 3))))
                return;

            var botrk = Config.Item("itemBotrk").GetValue<bool>();
            var youmuu = Config.Item("itemYoumuu").GetValue<bool>();
            var target = SOW.GetTarget() as Obj_AI_Base;

            if (botrk)
            {
                if (target != null && target.Type == ObjectManager.Player.Type &&
                    target.ServerPosition.Distance(ObjectManager.Player.ServerPosition) < 450)
                {
                    var hasCutGlass = Items.HasItem(3144);
                    var hasBotrk = Items.HasItem(3153);

                    if (hasBotrk || hasCutGlass)
                    {
                        var itemId = hasCutGlass ? 3144 : 3153;
                        var damage = ObjectManager.Player.GetItemDamage(target, Damage.DamageItems.Botrk);
                        if (hasCutGlass ||
                            ObjectManager.Player.Health + damage < ObjectManager.Player.MaxHealth &&
                            Items.CanUseItem(itemId))
                            Items.UseItem(itemId, target);
                    }
                }
            }

            if (youmuu && target != null && target.Type == ObjectManager.Player.Type &&
                Orbwalking.InAutoAttackRange(target) && Items.CanUseItem(3142))
                Items.UseItem(3142);
        }
    }
}
