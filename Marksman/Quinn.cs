  public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETC" + Id, "Do not Under Turret E").SetValue(true));
            config.AddItem(new MenuItem("UseETK" + Id, "Use E Under Turret If Enemy Killable")
                .SetValue(true));
        }
        
        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseETH" + Id, "Do not Under Turret E").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
        }
        
        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, 
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, 
                    System.Drawing.Color.FromArgb(100, 255, 255, 255))));
        }
    }
}
