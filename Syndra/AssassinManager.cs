using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace Syndra
{
    using SharpDX;
    using SharpDX.Direct3D9;
    using Syndra.Properties;
    using Color = System.Drawing.Color;

    internal enum TargetSelect
    {
        Syndra,

        LeagueSharp
    }

    internal class AssassinManager
    {
        public Menu Config;

        public static Font Text;

        private const string Tab = "    ";

        private TargetSelect Selector
        {
            get
            {
                return this.Config.Item("TS").GetValue<StringList>().SelectedIndex == 0
                           ? TargetSelect.Syndra
                           : TargetSelect.LeagueSharp;
            }
        }

        public void Initialize()
        {
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                    {
                        FaceName = "Malgun Gothic", Height = 21, OutputPrecision = FontPrecision.Default,
                        Weight = FontWeight.Bold, Quality = FontQuality.ClearTypeNatural
                    });

            this.Config = new Menu("Target Selector", "AssassinTargetSelector").SetFontStyle(
                FontStyle.Regular,
                SharpDX.Color.Cyan);

            var menuTargetSelector = new Menu("Target Selector", "TargetSelector");
            {
                TargetSelector.AddToMenu(menuTargetSelector);
            }

            this.Config.AddItem(
                new MenuItem("TS", "Active Target Selector:").SetValue(
                    new StringList(new[] { "Syndra Target Selector", "L# Target Selector" })))
                .SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow)
                .ValueChanged += (sender, args) =>
                    {
                        this.Config.Items.ForEach(
                            i =>
                                {
                                    i.Show();
                                    switch (args.GetNewValue<StringList>().SelectedIndex)
                                    {
                                        case 0:
                                            if (i.Tag == 22) i.Show(false);
                                            break;
                                        case 1:
                                            if (i.Tag == 11 || i.Tag == 12) i.Show(false);
                                            break;
                                    }
                                });
                    };

            menuTargetSelector.Items.ForEach(
                i =>
                    {
                        this.Config.AddItem(i);
                        i.SetTag(22);
                    });

            this.Config.AddItem(
                new MenuItem("Set", "Target Select Mode:").SetValue(
                    new StringList(new[] { "Single Target Select", "Multi Target Select" })))
                .SetFontStyle(FontStyle.Regular, SharpDX.Color.LightCoral)
                .SetTag(11);
            this.Config.AddItem(new MenuItem("Range", "Range (Recommend: 1150):"))
                .SetValue(new Slider(1150, (int)Program.Q.Range, (int)Program.Q.Range * 2))
                .SetTag(11);

            this.Config.AddItem(
                new MenuItem("Targets", "Targets:").SetFontStyle(FontStyle.Regular, SharpDX.Color.Aqua).SetTag(11));
            foreach (var e in HeroManager.Enemies)
            {
                this.Config.AddItem(
                    new MenuItem("enemy." + e.ChampionName, string.Format("{0}Focus {1}", Tab, e.ChampionName)).SetValue
                        (false)).SetTag(12);

            }

            this.Config.AddItem(
                new MenuItem("Draw.Title", "Drawings").SetFontStyle(FontStyle.Regular, SharpDX.Color.Aqua).SetTag(11));
            this.Config.AddItem(
                new MenuItem("Draw.Range", Tab + "Range").SetValue(new Circle(true, Color.Gray)).SetTag(11));
            this.Config.AddItem(
                new MenuItem("Draw.Enemy", Tab + "Active Enemy").SetValue(new Circle(true, Color.GreenYellow))
                    .SetTag(11));
            this.Config.AddItem(
                new MenuItem("Draw.Status", Tab + "Show Enemy:").SetValue(
                    new StringList(new[] { "Off", "Notification Text", "Sprite", "Both" }, 3)));
            Program.Config.AddSubMenu(this.Config);

            Sprite.Initialize();

            Game.OnWndProc += this.Game_OnWndProc;
            Drawing.OnDraw += this.Drawing_OnDraw;
            Drawing.OnPreReset += this.Drawing_OnPreReset;
            Drawing.OnPostReset += this.Drawing_OnPostReset;
            AppDomain.CurrentDomain.DomainUnload += this.CurrentDomain_DomainUnload;
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Text.OnLostDevice();
            Text.Dispose();
        }

        void Drawing_OnPostReset(EventArgs args)
        {
            Text.OnResetDevice();
        }

        void Drawing_OnPreReset(EventArgs args)
        {
            Text.OnLostDevice();
        }

        private void RefreshMenuItemsStatus()
        {

            this.Config.Items.ForEach(
                i =>
                    {
                        i.Show();
                        switch (this.Selector)
                        {
                            case TargetSelect.Syndra:
                                if (i.Tag == 22)
                                {
                                    i.Show(false);
                                }
                                break;
                            case TargetSelect.LeagueSharp:
                                if (i.Tag == 11)
                                {
                                    i.Show(false);
                                }
                                break;
                        }
                    });
        }

        public void ClearAssassinList()
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsEnemy))
            {
                this.Config.Item("enemy." + enemy.ChampionName).SetValue(false);
            }
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            if (this.Selector != TargetSelect.Syndra)
            {
                return;
            }

            if (args.Msg == 0x201)
            {
                foreach (var objAiHero in from hero in HeroManager.Enemies
                                          where
                                              hero.Distance(Game.CursorPos) < 150f && hero != null && hero.IsVisible
                                              && !hero.IsDead
                                          orderby hero.Distance(Game.CursorPos) descending
                                          select hero)
                {
                    if (objAiHero != null && objAiHero.IsVisible && !objAiHero.IsDead)
                    {
                        var xSelect = Program.Config.Item("Set").GetValue<StringList>().SelectedIndex;

                        switch (xSelect)
                        {
                            case 0:
                                this.ClearAssassinList();
                                Program.Config.Item("enemy." + objAiHero.ChampionName).SetValue(true);
                                break;
                            case 1:
                                var menuStatus = Program.Config.Item("enemy." + objAiHero.ChampionName).GetValue<bool>();
                                Program.Config.Item("enemy." + objAiHero.ChampionName).SetValue(!menuStatus);
                                break;
                        }
                    }
                }
            }
        }

        public Obj_AI_Hero GetTarget(
            float vDefaultRange = 0,
            TargetSelector.DamageType vDefaultDamageType = TargetSelector.DamageType.Physical)
        {
            if (this.Selector != TargetSelect.Syndra)
            {
                return TargetSelector.GetTarget(vDefaultRange, vDefaultDamageType);
            }

            vDefaultRange = Math.Abs(vDefaultRange) < 0.00001
                                ? Program.Eq.Range
                                : this.Config.Item("Range").GetValue<Slider>().Value;

            var vEnemy =
                HeroManager.Enemies.Where(e => e.IsValidTarget(vDefaultRange) && !e.IsZombie)
                    .Where(e => this.Config.Item("enemy." + e.ChampionName) != null)
                    .Where(e => this.Config.Item("enemy." + e.ChampionName).GetValue<bool>());

            if (this.Config.Item("Set").GetValue<StringList>().SelectedIndex == 1)
            {
                vEnemy = (from vEn in vEnemy select vEn).OrderByDescending(vEn => vEn.MaxHealth);
            }

            var objAiHeroes = vEnemy as Obj_AI_Hero[] ?? vEnemy.ToArray();

            var t = !objAiHeroes.Any() ? TargetSelector.GetTarget(vDefaultRange, vDefaultDamageType) : objAiHeroes[0];

            return t;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            var drawEnemy = this.Config.Item("Draw.Enemy").GetValue<Circle>();
            if (drawEnemy.Active)
            {
                var t = this.GetTarget(Program.E.Range, TargetSelector.DamageType.Physical);
                if (t.IsValidTarget())
                {
                    Render.Circle.DrawCircle(t.Position, (float)(t.BoundingRadius * 1.5), drawEnemy.Color);
                }
            }

            if (this.Selector != TargetSelect.Syndra)
            {
                return;
            }

            var rangeColor = this.Config.Item("Draw.Range").GetValue<Circle>();
            var range = this.Config.Item("Range").GetValue<Slider>().Value;
            if (rangeColor.Active)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, range, rangeColor.Color);
            }
            var drawStatus = this.Config.Item("Draw.Status").GetValue<StringList>().SelectedIndex;
            if (drawStatus == 1 || drawStatus == 3)
            {
                foreach (var e in
                    HeroManager.Enemies.Where(
                        e =>
                        e.IsVisible && !e.IsDead && this.Config.Item("enemy." + e.ChampionName) != null
                        && this.Config.Item("enemy." + e.ChampionName).GetValue<bool>()))
                {
                    DrawText(
                        Text,
                        "1st Priority Target",
                        e.HPBarPosition.X + e.BoundingRadius / 2f - (e.CharData.BaseSkinName.Length / 2f) - 27,
                        e.HPBarPosition.Y - 23,
                        SharpDX.Color.Black);

                    DrawText(
                        Text,
                        "1st Priority Target",
                        e.HPBarPosition.X + e.BoundingRadius / 2f - (e.CharData.BaseSkinName.Length / 2f) - 29,
                        e.HPBarPosition.Y - 25,
                        SharpDX.Color.IndianRed);
                }
            }
        }

        public static void DrawText(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int)vPosX, (int)vPosY, vColor);
        }

    }

    internal class Sprite
    {
        private static Vector2 DrawPosition
        {
            get
            {
                if (Program.AssassinManager.Config.Item("TS").GetValue<StringList>().SelectedIndex == 0)
                {
                    return new Vector2(0f, 0f);
                }

                var drawStatus = Program.AssassinManager.Config.Item("Draw.Status").GetValue<StringList>().SelectedIndex;
                if (KillableEnemy == null || (drawStatus != 2 && drawStatus != 3))
                {
                    return new Vector2(0f, 0f);
                }

                return new Vector2(
                    KillableEnemy.HPBarPosition.X + KillableEnemy.BoundingRadius / 2f,
                    KillableEnemy.HPBarPosition.Y - 70);
            }
        }

        private static bool DrawSprite
        {
            get
            {
                return true;
            }
        }

        private static Obj_AI_Hero KillableEnemy
        {
            get
            {
                var t = Program.AssassinManager.GetTarget(Program.E.Range);

                if (t.IsValidTarget()) return t;

                return null;
            }
        }

        internal static void Initialize()
        {
            new Render.Sprite(Resources.selectedchampion, new Vector2())
                {
                    PositionUpdate = () => DrawPosition, Scale = new Vector2(1f, 1f),
                    VisibleCondition = sender => DrawSprite
                }.Add();
        }
    }
}