using LeagueSharp;
using System.Collections.Generic;

namespace Marksman
{
    public enum Skilltype
    {
        Unknown = 0,
        Line = 1,
        Circle = 2,
        Cone = 3
    }

    public class SpellList
    {
        public string HeroName { get; set; }
        public string SpellMenuName { get; set; }
        public SpellSlot Slot { get; set; }
        public Skilltype Type { get; set; }
        public float Radius { get; set; }
        public string SDataName { get; set; }
        public int DangerLevel { get; set; }

        public static List<SpellList> CCList = new List<SpellList>();
        public static List<SpellList> SList = new List<SpellList>();

        static SpellList()
        {
            #region CCList

            CCList.Add(
                new SpellList
                {
                    HeroName = "Aatorx",
                    SpellMenuName = "Dark Flight",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "AatroxQ",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Aatorx",
                    SpellMenuName = "Blades of Torment",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    SDataName = "AatroxE",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Ahri",
                    SpellMenuName = "Charm",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    SDataName = "AhriSeduce",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Alistar",
                    SpellMenuName = "Pulverize",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "Pulverize",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Alistar",
                    SpellMenuName = "Headbutt",
                    Slot = SpellSlot.W,
                    SDataName = "Headbutt",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Amumu",
                    SpellMenuName = "Bandage Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "BandageToss",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Amumu",
                    SpellMenuName = "Curse of the Sad Mummy",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    SDataName = "CurseoftheSadMummy",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Anivia",
                    SpellMenuName = "Flash Frost",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "FlashFrost",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Anivia",
                    SpellMenuName = "Glacial Storm",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "GlacialStorm",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Annie",
                    SpellMenuName = "Tibbers",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "InfernalGuardian",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Ashe",
                    SpellMenuName = "Crystal Arrow",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    SDataName = "EnchantedCrystalArrow",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Ashe",
                    SpellMenuName = "Volley",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Cone,
                    SDataName = "Volley",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Azir",
                    SpellMenuName = "ShiftingSands",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    SDataName = "AzirE",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Azir",
                    SpellMenuName = "Emperor's Divide",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    SDataName = "AzirR",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Rocket Grab",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "RocketGrab",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Power Fist",
                    Slot = SpellSlot.E,
                    SDataName = "PowerFist",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Brand",
                    SpellMenuName = "Sear",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "BrandBlazeMissile",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Bruam",
                    SpellMenuName = "Winter's Bite",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "BraumQ",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Bruam",
                    SpellMenuName = "Glacial Fissure",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    SDataName = "BraumR",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Caitlyn",
                    SpellMenuName = "90 Caliber Net",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "CaitlynEntrapment",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Cassiopeia",
                    SpellMenuName = "Petrifying Gaze",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Cone,
                    SDataName = "CassiopeiaPetrifyingGaze",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Cho'gath",
                    SpellMenuName = "Rupture",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "Rupture",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Darius",
                    SpellMenuName = "Aprehend",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    SDataName = "DariusAxeGrabCone",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Diana",
                    SpellMenuName = "Moonfall",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    SDataName = "DianaVortex",
                    DangerLevel = 3
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "DrMundo",
                    SpellMenuName = "Cleaver",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "InfectedCleaverMissileCast",
                    DangerLevel = 3
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Draven",
                    SpellMenuName = "Stand Aside",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    SDataName = "DravenDoubleShot",
                    DangerLevel = 3
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Elise",
                    SpellMenuName = "Cocoon",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    SDataName = "DravenDoubleShot",
                    DangerLevel = 3
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Evelynn",
                    SpellMenuName = "Agony's Embrace",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "EvelynnR",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Fizz",
                    SpellMenuName = "Chum the Waters",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "FizzMarinerDoomMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Fizz",
                    SpellMenuName = "Playful Trickster",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "FizzJump",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Galio",
                    SpellMenuName = "Resolute Smite",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "GalioResoluteSmite",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Galio",
                    SpellMenuName = "Idol Of Durand",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GalioIdolOfDurand",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Boomerang Throw",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Bouldar Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarBigQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Wallop",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarBigW",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Gnar",
                    SpellMenuName = "GNAR!",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GnarR",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Barrel Roll",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "GragasQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Body Slam",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GragasE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Explosive Cask",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GragasR",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Heimerdinger",
                    SpellMenuName = "Electron Storm Grenade",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "HeimerdingerE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Hecarim",
                    SpellMenuName = "Onslaught of Shadows",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "HecarimUlt",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Hecarim",
                    SpellMenuName = "Devestating Charge",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "HecarimRamp",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Janna",
                    SpellMenuName = "Howling Gale",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "HowlingGale",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Janna",
                    SpellMenuName = "Zephyr",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "ReapTheWhirlwind",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Jax",
                    SpellMenuName = "Counter Strike",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "JaxCounterStrike",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "JarvanIV",
                    SpellMenuName = "Dragon Strike",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JarvanIVDragonStrike",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Jayce",
                    SpellMenuName = "Thundering Blow",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "JayceThunderingBlow",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Jinx",
                    SpellMenuName = "Zap!",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JinxW",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Jinx",
                    SpellMenuName = "Chompers!",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 4,
                    SDataName = "JinxE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Karma",
                    SpellMenuName = "Inner Flame (Mantra)",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "KarmaQMantra",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Karma",
                    SpellMenuName = "Sprit Bond",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "KarmaQMantra",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Kassadin",
                    SpellMenuName = "Force Pulse",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    DangerLevel = 3,
                    SDataName = "ForcePulse",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Khazix",
                    SpellMenuName = "Void Spikes",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "KhazixW",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Kayle",
                    SpellMenuName = "Reckoning",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "JudicatorReckoning",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "KogMaw",
                    SpellMenuName = "Void Ooze",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "KogMawVoidOoze",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Leblanc",
                    SpellMenuName = "Soul Shackle",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LeblancSoulShackle",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Leblanc",
                    SpellMenuName = "Soul Shackle (Mimic)",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LeblancSoulShackleM",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "LeeSin",
                    SpellMenuName = "Dragon's Rage",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "BlindMonkRKick",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Leona",
                    SpellMenuName = "Zenith Blade",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LeonaZenithBlade",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Leona",
                    SpellMenuName = "Shield of Daybreak",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "LeonaShieldOfDaybreak",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Leona",
                    SpellMenuName = "Solar Flare",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "LeonaSolarFlare",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lissandra",
                    SpellMenuName = "Ice Shard",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "LissandraQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lissandra",
                    SpellMenuName = "Ring of Frost",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LissandraW",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Lulu",
                    SpellMenuName = "Glitterlance",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "LuluQ"
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lulu",
                    SpellMenuName = "Glitterlance: Extended",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LuluQMissileTwo"
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lux",
                    SpellMenuName = "Light Binding",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "LuxLightBinding",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lux",
                    SpellMenuName = "Lucent Singularity",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "LuxLightStrikeKugel",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Lux",
                    SpellMenuName = "Final Spark",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "LuxMaliceCannon",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Malphite",
                    SpellMenuName = "Unstoppable Force",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "UFSlash",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Malphite",
                    SpellMenuName = "Sismic Shard",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "SismicShard",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Malzahar",
                    SpellMenuName = "Nether Grasp",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "AlZaharNetherGrasp",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Maokai",
                    SpellMenuName = "Twisted Advance",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "MaokaiUnstableGrowth",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Maokai",
                    SpellMenuName = "Arcane Smash",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "MaokaiTrunkLine",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Morgana",
                    SpellMenuName = "Dark Binding",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "DarkBindingMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Mordekaiser",
                    SpellMenuName = "Children of the Grave",
                    Slot = SpellSlot.Q,
                    DangerLevel = 5,
                    SDataName = "MordekaiserChildrenOfTheGrave",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Wukong",
                    SpellMenuName = "Cyclone",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "MonkeyKingSpinToWin",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nami",
                    SpellMenuName = "Aqua Prision",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "NamiQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nasus",
                    SpellMenuName = "Wither",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "NasusW",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Karthus",
                    SpellMenuName = "Wall of Pain",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "KarthusWallOfPain",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nami",
                    SpellMenuName = "Tidal Wave",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "NamiR",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Dredge Line",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "NautilusAnchorDragMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Riptide",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "NautilusSplashZone",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Depth Charge",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "NautilusGrandLine",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Nidalee",
                    SpellMenuName = "Javelin Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JavelinToss",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Olaf",
                    SpellMenuName = "Undertow",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "OlafAxeThrowCast",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Orianna",
                    SpellMenuName = "Command: Dissonance ",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "OrianaDissonanceCommand",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Orianna",
                    SpellMenuName = "OrianaDetonateCommand",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "OrianaDetonateCommand",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Quinn",
                    SpellMenuName = "Blinding Assault",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "QuinnQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Rammus",
                    SpellMenuName = "Puncturing Taunt",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "PuncturingTaunt",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Rengar",
                    SpellMenuName = "Bola Strike (Emp)",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "RengarEFinal",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Fiddlesticks",
                    SpellMenuName = "Terrify",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "Terrify",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Renekton",
                    SpellMenuName = "Ruthless Predator",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "RenektonPreExecute",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Riven",
                    SpellMenuName = "Ki Burst",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "RivenMartyr"
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Rumble",
                    SpellMenuName = "RumbleGrenade",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "RumbleGrenade",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Rumble",
                    SpellMenuName = "RumbleCarpetBombM",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 4,
                    SDataName = "RumbleCarpetBombMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Ryze",
                    SpellMenuName = "Rune Prision",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "RunePrison",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Sejuani",
                    SpellMenuName = "Arctic Assault",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "SejuaniArcticAssault",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Sejuani",
                    SpellMenuName = "Glacial Prision",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SejuaniGlacialPrisonStart",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Singed",
                    SpellMenuName = "Mega Adhesive",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "MegaAdhesive",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Singed",
                    SpellMenuName = "Fling",
                    Slot = SpellSlot.E,
                    DangerLevel = 2,
                    SDataName = "Fling",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nocturne",
                    SpellMenuName = "Unspeakable Horror",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "NocturneUnspeakableHorror",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Shen",
                    SpellMenuName = "ShenShadowDash",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ShenShadowDash",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Shyvana",
                    SpellMenuName = "ShyvanaTransformCast",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ShyvanaTransformCast",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Skarner",
                    SpellMenuName = "Fracture",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "SkarnerFractureMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Skarner",
                    SpellMenuName = "Impale",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SkarnerFractureMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Pantheon",
                    SpellMenuName = "Aegis of Zeonia",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "PantheonW",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Pantheon",
                    SpellMenuName = "Heroic Charge",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "PoppyHeroicCharge",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Nunu",
                    SpellMenuName = "Ice Blast",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "Ice Blast",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Sona",
                    SpellMenuName = "Crescendo",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SonaCrescendo",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Swain",
                    SpellMenuName = "Nevermove",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "SwainShadowGrasp",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Syndra",
                    SpellMenuName = "Scatter the Weak",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    DangerLevel = 5,
                    SDataName = "SyndraE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Thresh",
                    SpellMenuName = "Death Sentence",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ThreshQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Thresh",
                    SpellMenuName = "Flay",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ThreshEFlay",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Tristana",
                    SpellMenuName = "Buster Shot",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "BusterShot",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Trundle",
                    SpellMenuName = "Pillar of Ice",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "TrundleCircle",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Trundle",
                    SpellMenuName = "Subjugate",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "TrundlePain",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Tryndamere",
                    SpellMenuName = "Mocking Shout",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "MockingShout",
                });

            CCList.Add(
                new SpellList
                {
                    HeroName = "Twitch",
                    SpellMenuName = "Venom Cask",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "TwitchVenomCaskMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Urgot",
                    SpellMenuName = "Corrosive Charge",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "UrgotPlasmaGrenadeBoom",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Varus",
                    SpellMenuName = "Hail of Arrowws",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "VarusE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Varus",
                    SpellMenuName = "Chain of Corruption",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "VarusR",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Veigar",
                    SpellMenuName = "Event Horizon",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "VeigarEventHorizon",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "VelkozQ",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "VelkozQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "Plasma Fission",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "VelkozQSplit",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "Tectonic Disruption",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "VelkozE",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Vi",
                    SpellMenuName = "Vault Breaker",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ViQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Vi",
                    SpellMenuName = "Assault and Battery",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ViR",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Viktor",
                    SpellMenuName = "Gravity Field",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "ViktorGravitonField",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Vayne",
                    SpellMenuName = "Condemn",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "Vayne Condemn",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Warwick",
                    SpellMenuName = "Infinite Duress",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "InfiniteDuress",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Xerath",
                    SpellMenuName = "Eye of Destruction",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "XerathArcaneBarrage2",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Xerath",
                    SpellMenuName = "Shocking Orb",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "XerathMageSpearMissile",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "XinZhao",
                    SpellMenuName = "Three Talon Strike",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "XenZhaoComboTarget",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "XinZhao",
                    SpellMenuName = "Audacious Charge",
                    Slot = SpellSlot.E,
                    DangerLevel = 4,
                    SDataName = "XenZhaoSweep",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "XinZhao",
                    SpellMenuName = "Crescent Sweep",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "XenZhaoParry",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq2",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "yasuoq2",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq3w",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "yasuoq3w",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "yasuoq",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Zac",
                    SpellMenuName = "Stretching Strike",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "ZacQ",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Zac",
                    SpellMenuName = "Lets Bounce!",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ZacR",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Zed",
                    SpellMenuName = "Death Mark",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ZedUlt",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Ziggs",
                    SpellMenuName = "Satchel Charge",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "ZiggsW",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Zyra",
                    SpellMenuName = "Grasping Roots",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "ZyraGraspingRoots",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Zyra",
                    SpellMenuName = "Stranglethorns",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ZyraBrambleZone",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Taric",
                    SpellMenuName = "Dazzle",
                    Slot = SpellSlot.E,
                    SDataName = "Dazzle",
                    DangerLevel = 5
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Yoric",
                    SpellMenuName = "Omen of Pestilence",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "YorickDecayed",
                });
            CCList.Add(
                new SpellList
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "Steel Tempest (3)",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "YasuoQ3",
                });

            #endregion

            #region SList

            SList.Add(
                new SpellList
                {
                    HeroName = "Fiddlesticks",
                    SpellMenuName = "Dark Wind",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "FiddlesticksDarkWind",
                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Static Field",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "StaticField",

                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Chogath",
                    SpellMenuName = "Feral Scream",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Cone,
                    DangerLevel = 3,
                    SDataName = "FeralScream",

                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Malzahar",
                    SpellMenuName = "Call of the Void",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "AlZaharCalloftheVoid",
                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Talon",
                    SpellMenuName = "Cutthroat",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "TalonCutthroat",
                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Garen",
                    SpellMenuName = "Decisive Strike",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "GarenQ",
                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Viktor",
                    SpellMenuName = "Chaos Storm",
                    Slot = SpellSlot.R,
                    DangerLevel = 3,
                    SDataName = "ViktorChaosStorm",
                });
            SList.Add(
                new SpellList
                {
                    HeroName = "Soraka",
                    SpellMenuName = "Equinox",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "SorakaE",
                });

            #endregion
        }
    }
}
