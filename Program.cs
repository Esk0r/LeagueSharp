#region
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace Marksman
{
    internal class Program
    {
        public static Menu Config;
        public static Menu QuickSilverMenu;
//        public static Menu MenuInterruptableSpell;
        public static Champion CClass;
        public static Activator AActivator;
        public static double ActivatorTime;
        private static Obj_AI_Hero xSelectedTarget;        
        
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Marksman", "Marksman", true);
            CClass = new Champion();
            AActivator = new Activator();
            
            var BaseType = CClass.GetType();

            /* Update this with Activator.CreateInstance or Invoke
               http://stackoverflow.com/questions/801070/dynamically-invoking-any-function-by-passing-function-name-as-string 
               For now stays cancer.
             */
            var championName = ObjectManager.Player.ChampionName.ToLowerInvariant();

            switch (championName)
            {
                case "ashe":
                    CClass = new Ashe();
                    break;
                case "caitlyn":
                    CClass = new Caitlyn();
                    break;
                case "corki":
                    CClass = new Corki();
                    break;
                case "draven":
                    CClass = new Draven();
                    break;
                case "ezreal":
                    CClass = new Ezreal();
                    break;
                case "graves":
                    CClass = new Graves();
                    break;
                case "gnar":
                    CClass = new Gnar();
                    break;
                case "jinx":
                    CClass = new Jinx();
                    break;
                case "kalista":
                    CClass = new Kalista();
                    break;
                case "kogmaw":
                    CClass = new Kogmaw();
                    break;
                case "lucian":
                    CClass = new Lucian();
                    break;
                case "missfortune":
                    CClass = new MissFortune();
                    break;   
                case "quinn":
                    CClass = new Quinn();
                    break;
                case "sivir":
                    CClass = new Sivir();
                    break;
                case "teemo":
                    CClass = new Teemo();
                    break;
                case "tristana":
                    CClass = new Tristana();
                    break;
                case "twitch":
                    CClass = new Twitch();
                    break;
                case "urgot":
                    CClass = new Urgot();
                    break;
                case "vayne":
                    CClass = new Vayne();
                    break;
                case "varus":
                    CClass = new Varus();
                    break;
            }


            CClass.Id = ObjectManager.Player.BaseSkinName;
            CClass.Config = Config;

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            var orbwalking = Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            CClass.Orbwalker = new Orbwalking.Orbwalker(orbwalking);

            /* Menu Summoners */
            var summoners = Config.AddSubMenu(new Menu("Summoners", "Summoners"));
            var summonersHeal = summoners.AddSubMenu(new Menu("Heal", "Heal"));
            {
                summonersHeal.AddItem(new MenuItem("SUMHEALENABLE", "Enable").SetValue(true));
                summonersHeal.AddItem(new MenuItem("SUMHEALSLIDER", "Min. Heal Per.").SetValue(new Slider(20, 99, 1)));
            }

            var summonersBarrier = summoners.AddSubMenu(new Menu("Barrier", "Barrier"));
            {
                summonersBarrier.AddItem(new MenuItem("SUMBARRIERENABLE", "Enable").SetValue(true));
                summonersBarrier.AddItem(
                    new MenuItem("SUMBARRIERSLIDER", "Min. Heal Per.").SetValue(new Slider(20, 99, 1)));
            }

            var summonersIgnite = summoners.AddSubMenu(new Menu("Ignite", "Ignite"));
            {
                summonersIgnite.AddItem(new MenuItem("SUMIGNITEENABLE", "Enable").SetValue(true));
            }
            /* Menu Items */            
            var items = Config.AddSubMenu(new Menu("Items", "Items"));
            items.AddItem(new MenuItem("BOTRK", "BOTRK").SetValue(true));
            items.AddItem(new MenuItem("GHOSTBLADE", "Ghostblade").SetValue(true));
            items.AddItem(new MenuItem("SWORD", "Sword of the Divine").SetValue(true));
            items.AddItem(new MenuItem("MURAMANA", "Muramana").SetValue(true));
            QuickSilverMenu = new Menu("QSS", "QuickSilverSash");
            items.AddSubMenu(QuickSilverMenu);
            QuickSilverMenu.AddItem(new MenuItem("AnyStun", "Any Stun").SetValue(true));
            QuickSilverMenu.AddItem(new MenuItem("AnySlow", "Any Slow").SetValue(true));
            QuickSilverMenu.AddItem(new MenuItem("AnySnare", "Any Snare").SetValue(true));
            QuickSilverMenu.AddItem(new MenuItem("AnyTaunt", "Any Taunt").SetValue(true));
            foreach (var t in AActivator.BuffList)
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
                {
                    if (t.ChampionName == enemy.ChampionName)
                        QuickSilverMenu.AddItem(new MenuItem(t.BuffName, t.DisplayName).SetValue(t.DefaultValue));
                }
            }
            items.AddItem(
                new MenuItem("UseItemsMode", "Use items on").SetValue(
                    new StringList(new[] {"No", "Mixed mode", "Combo mode", "Both"}, 2)));

            
            //var Extras = Config.AddSubMenu(new Menu("Extras", "Extras"));
            //new PotionManager(Extras);

            // If Champion is supported draw the extra menus
            if (BaseType != CClass.GetType())
            {
                var combo = new Menu("Combo", "Combo");
                if (CClass.ComboMenu(combo))
                {
                    Config.AddSubMenu(combo);
                }

                var harass = new Menu("Harass", "Harass");
                if (CClass.HarassMenu(harass))
                {
                    harass.AddItem(new MenuItem("HarassMana", "Min. Mana Percent").SetValue(new Slider(50, 100, 0)));
                    Config.AddSubMenu(harass);
                }

                var laneclear = new Menu("LaneClear", "LaneClear");
                if (CClass.LaneClearMenu(laneclear))
                {
                    laneclear.AddItem(
                        new MenuItem("LaneClearMana", "Min. Mana Percent").SetValue(new Slider(50, 100, 0)));
                    Config.AddSubMenu(laneclear);
                }

                var misc = new Menu("Misc", "Misc");
                if (CClass.MiscMenu(misc))
                {
                    Config.AddSubMenu(misc);
                }
                /*
                if (championName != "caitlyn" || championName != "jinx")
                {
                    MenuInterruptableSpell = new Menu("Interruptable Spell",
                        "Interrupt with " + championName == "caitlyn" ? "Caitlyn's W" : "Jinx's E");

                    MenuInterruptableSpell.AddItem(new MenuItem("InterruptSpells", "Active").SetValue(true));

                    foreach (var xSpell in Interrupter.Spells)
                    {
                        MenuInterruptableSpell.AddItem(
                            new MenuItem("IntNode" + xSpell.BuffName, xSpell.ChampionName + " | " + xSpell.Slot)
                                .SetValue(true));
                    }
                    Config.AddSubMenu(MenuInterruptableSpell);
                }
                */
                var extras = new Menu("Extras", "Extras");
                if (CClass.ExtrasMenu(extras))
                {
                    new PotionManager(extras);
                    Config.AddSubMenu(extras);
                }

                var drawing = new Menu("Drawings", "Drawings");
                if (CClass.DrawingMenu(drawing))
                {
                    drawing.AddItem(
                        new MenuItem("drawMinionLastHit", "Minion Last Hit").SetValue(new Circle(false,
                            System.Drawing.Color.GreenYellow)));
                    drawing.AddItem(
                        new MenuItem("drawMinionNearKill", "Minion Near Kill").SetValue(new Circle(false,
                            System.Drawing.Color.Gray)));
                    drawing.AddItem(new MenuItem("drawJunglePosition", "JunglePosition").SetValue(true));

                    Config.AddSubMenu(drawing);
                }

            }


            CClass.MainMenu(Config);

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            //Interrupter.OnPossibleToInterrupt += Interrupter_OnPosibleToInterrupt;
            //Game.OnWndProc += Game_OnWndProc;
        }
        
        /*
        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base t, InterruptableSpell args)
        {
            if (!Config.Item("InterruptSpells").GetValue<KeyBind>().Active) 
                return;

            if (ObjectManager.Player.ChampionName != "Caitlyn" || ObjectManager.Player.ChampionName != "Jinx")
                return;

            Spell xSpellSlot = null;

            if (ObjectManager.Player.ChampionName == "Caitlyn")
            {
                xSpellSlot = new Spell(SpellSlot.W);
                xSpellSlot.Range = 800f;
            }

            if (ObjectManager.Player.ChampionName == "Jinx")
            {
                xSpellSlot = new Spell(SpellSlot.E);
                xSpellSlot.Range = 900f;
            }
            if (xSpellSlot == null)
                return;

            if (ObjectManager.Player.Distance(t) < xSpellSlot.Range)
                xSpellSlot.Cast(t);
        }
        */
        private static void Game_OnWndProc(WndEventArgs args)
        {
            
            if (args.Msg != 0x201)
                return;

                foreach (var objAiHero in from hero in ObjectManager.Get<Obj_AI_Hero>()
                                          where hero.IsValidTarget()
                                          select hero
                                              into h
                                              orderby h.Distance(Game.CursorPos) descending
                                              select h
                                                  into enemy
                                                  where enemy.Distance(Game.CursorPos) < 150f
                                                  select enemy)
                {
                    if (objAiHero != null && objAiHero != xSelectedTarget)
                    {
                        xSelectedTarget = objAiHero;
                        TargetSelector.SetTarget(objAiHero);
                        Utils.PrintMessage(string.Format("{0} selected.", objAiHero.BaseSkinName));
                    }
                }
          
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
           /* 
           if (xSelectedTarget != null && xSelectedTarget.IsValidTarget())
            {
                Render.Circle.DrawCircle(xSelectedTarget.Position, xSelectedTarget.BoundingRadius * 1.5f, System.Drawing.Color.Red);
            }
            */
            var drawJunglePosition = CClass.Config.SubMenu("Drawings").Item("drawJunglePosition").GetValue<bool>();
            {
                if (drawJunglePosition)
                    Utils.Jungle.DrawJunglePosition();
            }
            
            var drawMinionLastHit = CClass.Config.SubMenu("Drawings").Item("drawMinionLastHit").GetValue<Circle>();
            var drawMinionNearKill = CClass.Config.SubMenu("Drawings").Item("drawMinionNearKill").GetValue<Circle>();
            if (drawMinionLastHit.Active || drawMinionNearKill.Active)
            {
                var xMinions =
                    MinionManager.GetMinions(ObjectManager.Player.Position,
                        ObjectManager.Player.AttackRange + ObjectManager.Player.BoundingRadius + 300, MinionTypes.All,
                        MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                foreach (var xMinion in xMinions)
                {
                    if (drawMinionLastHit.Active && ObjectManager.Player.GetAutoAttackDamage(xMinion, true) >=
                        xMinion.Health)
                    {
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, drawMinionLastHit.Color);
                    }
                    else if (drawMinionNearKill.Active &&
                             ObjectManager.Player.GetAutoAttackDamage(xMinion, true) * 2 >= xMinion.Health) 
                    {
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, drawMinionNearKill.Color);
                    }
                }
            }

            if (CClass != null)
            {
                CClass.Drawing_OnDraw(args);
            }
            return;

            var y = 10;

            foreach (
                var t in
                    ObjectManager.Player.Buffs.Select(
                        b => b.DisplayName + " - " + b.IsActive + " - " + (b.EndTime > Game.Time) + " - " + b.IsPositive)
                )
            {
                Drawing.DrawText(0, y, System.Drawing.Color.Wheat, t);
                y = y + 16;
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            CheckChampionBuff();
            //Update the combo and harass values.
            CClass.ComboActive = CClass.Config.Item("Orbwalk").GetValue<KeyBind>().Active;
            
            var vHarassManaPer = Config.Item("HarassMana").GetValue<Slider>().Value;
            CClass.HarassActive = CClass.Config.Item("Farm").GetValue<KeyBind>().Active &&
                                  ObjectManager.Player.ManaPercentage() >= vHarassManaPer;

            CClass.ToggleActive = ObjectManager.Player.ManaPercentage() >= vHarassManaPer;

            var vLaneClearManaPer = Config.Item("LaneClearMana").GetValue<Slider>().Value;
            CClass.LaneClearActive = CClass.Config.Item("LaneClear").GetValue<KeyBind>().Active &
                                     ObjectManager.Player.ManaPercentage() >= vLaneClearManaPer;

            CClass.Game_OnGameUpdate(args);
            
            UseSummoners();
            var useItemModes = Config.Item("UseItemsMode").GetValue<StringList>().SelectedIndex;

            //Items
            if (
                !((CClass.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                   (useItemModes == 2 || useItemModes == 3))
                  ||
                  (CClass.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed &&
                   (useItemModes == 1 || useItemModes == 3))))
                return;

            var botrk = Config.Item("BOTRK").GetValue<bool>();
            var ghostblade = Config.Item("GHOSTBLADE").GetValue<bool>();
            var sword = Config.Item("SWORD").GetValue<bool>();
            var muramana = Config.Item("MURAMANA").GetValue<bool>();
            var target = CClass.Orbwalker.GetTarget() as Obj_AI_Base;

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
                        if (hasCutGlass || ObjectManager.Player.Health + damage < ObjectManager.Player.MaxHealth)
                            Items.UseItem(itemId, target);
                    }
                }
            }

            if (ghostblade && target != null && target.Type == ObjectManager.Player.Type &&
                 !ObjectManager.Player.HasBuff("ItemSoTD", true) /*if Sword of the divine is not active */ 
                 && Orbwalking.InAutoAttackRange(target)) 
                Items.UseItem(3142);

            if (sword && target != null && target.Type == ObjectManager.Player.Type &&
                !ObjectManager.Player.HasBuff("spectralfury", true) /*if ghostblade is not active*/ 
                && Orbwalking.InAutoAttackRange(target)) 
                Items.UseItem(3131);

            if (muramana && Items.HasItem(3042))
            {
                if (target != null && CClass.ComboActive &&
                    target.Position.Distance(ObjectManager.Player.Position) < 1200) 
                {
                    if (!ObjectManager.Player.HasBuff("Muramana", true))
                    {
                        Items.UseItem(3042);
                    }
                }
                else
                {
                    if (ObjectManager.Player.HasBuff("Muramana", true))
                    {
                        Items.UseItem(3042);
                    }
                }
            }
        }
        
        public static void UseSummoners()
        {
            if (ObjectManager.Player.IsDead)
                return;
                
            const int xDangerousRange = 1100;

            if (Config.Item("SUMHEALENABLE").GetValue<bool>())
            {
                var xSlot = ObjectManager.Player.GetSpellSlot("summonerheal");
                var xCanUse = ObjectManager.Player.Health <=
                              ObjectManager.Player.MaxHealth/100*Config.Item("SUMHEALSLIDER").GetValue<Slider>().Value;

                if (xCanUse && !ObjectManager.Player.InShop() && 
                    (xSlot != SpellSlot.Unknown || ObjectManager.Player.Spellbook.CanUseSpell(xSlot) == SpellState.Ready) 
                    && ObjectManager.Player.CountEnemysInRange(xDangerousRange) > 0) 
                {
                    ObjectManager.Player.Spellbook.CastSpell(xSlot);
                }
            }
            
            if (Config.Item("SUMBARRIERENABLE").GetValue<bool>())
            {
                var xSlot = ObjectManager.Player.GetSpellSlot("summonerbarrier");
                var xCanUse = ObjectManager.Player.Health <=
                              ObjectManager.Player.MaxHealth/100*Config.Item("SUMBARRIERSLIDER").GetValue<Slider>().Value;

                if (xCanUse && !ObjectManager.Player.InShop() && 
                    (xSlot != SpellSlot.Unknown || ObjectManager.Player.Spellbook.CanUseSpell(xSlot) == SpellState.Ready) 
                    && ObjectManager.Player.CountEnemysInRange(xDangerousRange) > 0) 
                {
                    ObjectManager.Player.Spellbook.CastSpell(xSlot);
                }
            }
            
            if (Config.Item("SUMIGNITEENABLE").GetValue<bool>())
            {
                var xSlot = ObjectManager.Player.GetSpellSlot("summonerdot");
                var t = CClass.Orbwalker.GetTarget() as Obj_AI_Hero;
                
                if (t != null && xSlot != SpellSlot.Unknown &&
                    ObjectManager.Player.Spellbook.CanUseSpell(xSlot) == SpellState.Ready)
                {
                    if (ObjectManager.Player.Distance(t) < 650 &&
                        ObjectManager.Player.GetSummonerSpellDamage(t, Damage.SummonerSpell.Ignite) >=
                        t.Health)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(xSlot, t);
                    }
                }
            }
        }
        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            CClass.Orbwalking_AfterAttack(unit, target);
        }

        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            CClass.Orbwalking_BeforeAttack(args);
        }

        private static void CheckChampionBuff()
        {
            foreach (var t1 in ObjectManager.Player.Buffs)
            {
                foreach (var t in QuickSilverMenu.Items)
                {
                    if (QuickSilverMenu.Item(t.Name).GetValue<bool>())
                    {
                        if (t1.Name.ToLower().Contains(t.Name.ToLower()))
                        {
                            foreach (var bx in AActivator.BuffList.Where(bx => bx.BuffName == t1.Name))
                            {
                                if (bx.Delay > 0)
                                {
                                    if (ActivatorTime + bx.Delay < (int) Game.Time)
                                        ActivatorTime = (int) Game.Time;

                                    if (ActivatorTime + bx.Delay <= (int) Game.Time)
                                    {
                                        if (Items.HasItem(3139)) Items.UseItem(3139);
                                        if (Items.HasItem(3140)) Items.UseItem(3140);
                                        ActivatorTime = (int) Game.Time;
                                    }
                                }
                                else
                                {
                                    if (Items.HasItem(3139)) Items.UseItem(3139);
                                    if (Items.HasItem(3140)) Items.UseItem(3140);
                                }

                            }
                        }
                    }

                    if (QuickSilverMenu.Item("AnySlow").GetValue<bool>() &&
                        ObjectManager.Player.HasBuffOfType(BuffType.Slow))
                    {
                        if (Items.HasItem(3139)) Items.UseItem(3139);
                        if (Items.HasItem(3140)) Items.UseItem(3140);
                    }
                    if (QuickSilverMenu.Item("AnySnare").GetValue<bool>() &&
                        ObjectManager.Player.HasBuffOfType(BuffType.Snare))
                    {
                        if (Items.HasItem(3139)) Items.UseItem(3139);
                        if (Items.HasItem(3140)) Items.UseItem(3140);
                    }
                    if (QuickSilverMenu.Item("AnyStun").GetValue<bool>() &&
                        ObjectManager.Player.HasBuffOfType(BuffType.Stun))
                    {
                        if (Items.HasItem(3139)) Items.UseItem(3139);
                        if (Items.HasItem(3140)) Items.UseItem(3140);
                    }
                    if (QuickSilverMenu.Item("AnyTaunt").GetValue<bool>() &&
                        ObjectManager.Player.HasBuffOfType(BuffType.Taunt))
                    {
                        if (Items.HasItem(3139)) Items.UseItem(3139);
                        if (Items.HasItem(3140)) Items.UseItem(3140);
                    }
                }
            }           
        }
    }
}
