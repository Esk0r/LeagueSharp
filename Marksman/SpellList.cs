using System.Collections.Generic;
using LeagueSharp;

namespace Marksman
{
    public enum BlockableSpells
    {
        SivirE,
        JinxE,
        CaitlynW,
        QuickSilverSash
    }

    public enum SkillShotType
    {
        SkillshotUnknown,
        SkillshotCircle,
        SkillshotLine,
        SkillshotCone,
        SkillshotTargeted
    }

    public class SpellList
    {
        public string ChampionName { get; set; }
        public string DisplayName { get; set; }
        public string BuffName { get; set; }
        public SpellSlot Slot { get; set; }
        public SkillShotType SkillType { get; set; }

        public BlockableSpells[] CanBlockWith = { };
        public bool DefaultMenuValue { get; set; }
        public int Delay { get; set; }


        public static List<SpellList> BuffList = new List<SpellList>();

        static SpellList()
        {

            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Darius",
                    DisplayName = "Darius (W)",
                    BuffName = "DariusNoxianTacticsONH",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotUnknown,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Diana",
                    DisplayName = "Diana (Q)",
                    BuffName = "DianaArc",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fizz",
                    DisplayName = "Fizz (R)",
                    BuffName = "fizzmarinerdoombomb",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Galio",
                    DisplayName = "Galio (R)",
                    BuffName = "GalioIdolOfDurand",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    CanBlockWith =
                        new[]
                        {
                            BlockableSpells.QuickSilverSash, BlockableSpells.SivirE, BlockableSpells.CaitlynW,
                            BlockableSpells.JinxE
                        },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "LeBlanc",
                    DisplayName = "LeBlanc (E)",
                    BuffName = "LeblancSoulShackle",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malzahar",
                    DisplayName = "Malzahar (R)",
                    BuffName = "AlZaharNetherGrasp",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith =
                        new[]
                        {
                            BlockableSpells.QuickSilverSash, BlockableSpells.SivirE, BlockableSpells.CaitlynW,
                            BlockableSpells.JinxE
                        },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Mordekaiser",
                    DisplayName = "Mordekaiser (R)",
                    BuffName = "MordekaiserChildrenOfTheGrave",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nocturne",
                    DisplayName = "Nocturne (R)",
                    BuffName = "NocturneParanoia",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Poppy",
                    DisplayName = "Poppy (R)",
                    BuffName = "PoppyDiplomaticImmunity",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rammus",
                    DisplayName = "Rammus (E)",
                    BuffName = "PuncturingTaunt",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "TwistedFate",
                    DisplayName = "Twisted Fate (R)",
                    BuffName = "Destiny",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotUnknown,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Skarner",
                    DisplayName = "Skarner (R)",
                    BuffName = "SkarnerImpale",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Urgot",
                    DisplayName = "Urgot (R)",
                    BuffName = "UrgotSwap2",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vladimir",
                    DisplayName = "Vladimir (R)",
                    BuffName = "VladimirHemoplague",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Warwick",
                    DisplayName = "Warwick (R)",
                    BuffName = "suppression",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Morgana",
                    DisplayName = "Morgana (Q)",
                    BuffName = "DarkBindingMissile",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Morgana",
                    DisplayName = "Morgana (R)",
                    BuffName = "SoulShackless",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zilean",
                    DisplayName = "Zilean (Q)",
                    BuffName = "timebombenemybuff",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = false,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zed",
                    DisplayName = "Zed (R)",
                    BuffName = "zedulttargetmark",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotTargeted,
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 3
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Aatorx",
                    DisplayName = "Dark Flight",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "AatroxQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ahri",
                    DisplayName = "Charm",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "AhriSeduce",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Alistar",
                    DisplayName = "Pulverize",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "Pulverize",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Alistar",
                    DisplayName = "Headbutt",
                    Slot = SpellSlot.W,
                    BuffName = "Headbutt",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Amumu",
                    DisplayName = "Bandage Toss",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "BandageToss",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Amumu",
                    DisplayName = "Curse of the Sad Mummy",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "CurseoftheSadMummy",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Annie",
                    DisplayName = "Tibbers",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "InfernalGuardian",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Azir",
                    DisplayName = "Emperor's Divide",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "AzirR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank",
                    DisplayName = "Rocket Grab",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RocketGrab",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank",
                    DisplayName = "Power Fist",
                    Slot = SpellSlot.E,
                    BuffName = "PowerFist",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Brand",
                    DisplayName = "Sear",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "BrandBlazeMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Bruam",
                    DisplayName = "Winter's Bite",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "BraumQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Bruam",
                    DisplayName = "Glacial Fissure",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "BraumR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Caitlyn",
                    DisplayName = "90 Caliber Net",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "CaitlynEntrapment",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Caitlyn",
                    DisplayName = "90 Caliber Net",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "R",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Cassiopeia",
                    DisplayName = "Petrifying Gaze",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "CassiopeiaPetrifyingGaze",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Cho'gath",
                    DisplayName = "Rupture",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "Rupture",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Darius",
                    DisplayName = "Aprehend",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "DariusAxeGrabCone",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Diana",
                    DisplayName = "Moonfall",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "DianaVortex",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "DrMundo",
                    DisplayName = "Cleaver",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "InfectedCleaverMissileCast",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Draven",
                    DisplayName = "Stand Aside",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "DravenDoubleShot",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Elise",
                    DisplayName = "Cocoon",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "DravenDoubleShot",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Evelynn",
                    DisplayName = "Agony's Embrace",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "EvelynnR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fizz",
                    DisplayName = "Chum the Waters",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "FizzMarinerDoomMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Galio",
                    DisplayName = "Resolute Smite",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GalioResoluteSmite",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Galio",
                    DisplayName = "Idol Of Durand",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GalioIdolOfDurand",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gnar",
                    DisplayName = "Wallop",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "GnarBigW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gnar",
                    DisplayName = "GNAR!",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GnarR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Barrel Roll",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GragasQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Body Slam",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "GragasE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Explosive Cask",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GragasR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Heimerdinger",
                    DisplayName = "Electron Storm Grenade",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HeimerdingerE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Hecarim",
                    DisplayName = "Onslaught of Shadows",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HecarimUlt",
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Hecarim",
                    DisplayName = "Devestating Charge",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HecarimRamp",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Janna",
                    DisplayName = "Howling Gale",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "HowlingGale",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Janna",
                    DisplayName = "Zephyr",
                    Slot = SpellSlot.W,
                    BuffName = "ReapTheWhirlwind",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jax",
                    DisplayName = "Counter Strike",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JaxCounterStrike",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "JarvanIV",
                    DisplayName = "Dragon Strike",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JarvanIVDragonStrike",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jayce",
                    DisplayName = "Thundering Blow",
                    Slot = SpellSlot.E,
                    BuffName = "JayceThunderingBlow",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jinx",
                    DisplayName = "Zap!",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JinxW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jinx",
                    DisplayName = "Chompers!",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JinxE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karma",
                    DisplayName = "Inner Flame (Mantra)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "KarmaQMantra",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karma",
                    DisplayName = "Sprit Bond",
                    Slot = SpellSlot.W,
                    BuffName = "KarmaQMantra",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Kassadin",
                    DisplayName = "Force Pulse",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "ForcePulse",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Khazix",
                    DisplayName = "Void Spikes",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "KhazixW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Kayle",
                    DisplayName = "Reckoning",
                    Slot = SpellSlot.Q,
                    BuffName = "JudicatorReckoning",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "KogMaw",
                    DisplayName = "Void Ooze",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "KogMawVoidOoze",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leblanc",
                    DisplayName = "Soul Shackle",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeblancSoulShackle",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leblanc",
                    DisplayName = "Soul Shackle (Mimic)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeblancSoulShackleM",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "LeeSin",
                    DisplayName = "Dragon's Rage",
                    Slot = SpellSlot.R,
                    BuffName = "BlindMonkRKick",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Zenith Blade",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeonaZenithBlade",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Shield of Daybreak",
                    Slot = SpellSlot.Q,
                    BuffName = "LeonaShieldOfDaybreak",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Solar Flare",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "LeonaSolarFlare",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lissandra",
                    DisplayName = "Ice Shard",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LissandraQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lissandra",
                    DisplayName = "Ring of Frost",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LissandraW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lulu",
                    DisplayName = "Glitterlance",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuluQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lulu",
                    DisplayName = "Glitterlance: Extended",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuluQMissileTwo",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Light Binding",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuxLightBinding",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Lucent Singularity",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "LuxLightStrikeKugel",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Final Spark",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuxMaliceCannon",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malphite",
                    DisplayName = "Unstoppable Force",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "UFSlash",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malphite",
                    DisplayName = "Sismic Shard",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SismicShard",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malzahar",
                    DisplayName = "Nether Grasp",
                    Slot = SpellSlot.R,
                    BuffName = "AlZaharNetherGrasp",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Maokai",
                    DisplayName = "Twisted Advance",
                    Slot = SpellSlot.W,
                    BuffName = "MaokaiUnstableGrowth",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Maokai",
                    DisplayName = "Arcane Smash",
                    Slot = SpellSlot.Q,
                    BuffName = "MaokaiTrunkLine",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Mordekaiser",
                    DisplayName = "Children of the Grave",
                    Slot = SpellSlot.Q,
                    BuffName = "MordekaiserChildrenOfTheGrave",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Wukong",
                    DisplayName = "Cyclone",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "MonkeyKingSpinToWin",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nami",
                    DisplayName = "Aqua Prision",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "NamiQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nami",
                    DisplayName = "Tidal Wave",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "NamiR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nasus",
                    DisplayName = "Wither",
                    Slot = SpellSlot.Q,
                    BuffName = "NasusW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karthus",
                    DisplayName = "Wall of Pain",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "KarthusWallOfPain",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Dredge Line",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "NautilusAnchorDragMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Riptide",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "NautilusSplashZone",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Depth Charge",
                    Slot = SpellSlot.R,
                    BuffName = "NautilusGrandLine",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nidalee",
                    DisplayName = "Javelin Toss",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JavelinToss",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Olaf",
                    DisplayName = "Undertow",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "OlafAxeThrowCast",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Orianna",
                    DisplayName = "Command: Dissonance ",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "OrianaDissonanceCommand",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Orianna",
                    DisplayName = "OrianaDetonateCommand",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "OrianaDetonateCommand",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Quinn",
                    DisplayName = "Blinding Assault",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "QuinnQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rammus",
                    DisplayName = "Puncturing Taunt",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "PuncturingTaunt",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rengar",
                    DisplayName = "Bola Strike (Emp)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RengarEFinal",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fiddlesticks",
                    DisplayName = "Terrify",
                    Slot = SpellSlot.Q,
                    BuffName = "Terrify",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Renekton",
                    DisplayName = "Ruthless Predator",
                    Slot = SpellSlot.W,
                    BuffName = "RenektonPreExecute",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Riven",
                    DisplayName = "Ki Burst",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RivenMartyr",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rumble",
                    DisplayName = "RumbleGrenade",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RumbleGrenade",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rumble",
                    DisplayName = "RumbleCarpetBombM",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RumbleCarpetBombMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ryze",
                    DisplayName = "Rune Prision",
                    Slot = SpellSlot.W,
                    BuffName = "RunePrison",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sejuani",
                    DisplayName = "Arctic Assault",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SejuaniArcticAssault",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sejuani",
                    DisplayName = "Glacial Prision",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SejuaniGlacialPrisonStart",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Singed",
                    DisplayName = "Mega Adhesive",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "MegaAdhesive",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Singed",
                    DisplayName = "Fling",
                    Slot = SpellSlot.E,
                    BuffName = "Fling",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nocturne",
                    DisplayName = "Unspeakable Horror",
                    Slot = SpellSlot.E,
                    BuffName = "NocturneUnspeakableHorror",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Shen",
                    DisplayName = "ShenShadowDash",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ShenShadowDash",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Shyvana",
                    DisplayName = "ShyvanaTransformCast",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ShyvanaTransformCast",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Skarner",
                    DisplayName = "Fracture",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SkarnerFractureMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Skarner",
                    DisplayName = "Impale",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SkarnerFractureMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Pantheon",
                    DisplayName = "Aegis of Zeonia",
                    Slot = SpellSlot.W,
                    BuffName = "PantheonW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Pantheon",
                    DisplayName = "Heroic Charge",
                    Slot = SpellSlot.W,
                    BuffName = "PoppyHeroicCharge",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nunu",
                    DisplayName = "Ice Blast",
                    Slot = SpellSlot.E,
                    BuffName = "Ice Blast",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sona",
                    DisplayName = "Crescendo",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SonaCrescendo",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Swain",
                    DisplayName = "Nevermove",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SwainShadowGrasp",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Syndra",
                    DisplayName = "Scatter the Weak",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "SyndraE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Thresh",
                    DisplayName = "Death Sentence",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ThreshQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Thresh",
                    DisplayName = "Flay",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ThreshEFlay",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Tristana",
                    DisplayName = "Buster Shot",
                    Slot = SpellSlot.R,
                    BuffName = "BusterShot",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Trundle",
                    DisplayName = "Pillar of Ice",
                    Slot = SpellSlot.E,
                    BuffName = "TrundleCircle",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Trundle",
                    DisplayName = "Subjugate",
                    Slot = SpellSlot.R,
                    BuffName = "TrundlePain",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Tryndamere",
                    DisplayName = "Mocking Shout",
                    Slot = SpellSlot.W,
                    BuffName = "MockingShout",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });

            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Twitch",
                    DisplayName = "Venom Cask",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "TwitchVenomCaskMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Urgot",
                    DisplayName = "Corrosive Charge",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "UrgotPlasmaGrenadeBoom",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Varus",
                    DisplayName = "Hail of Arrowws",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VarusE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Varus",
                    DisplayName = "Chain of Corruption",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VarusR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Veigar",
                    DisplayName = "Event Horizon",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VeigarEventHorizon",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "VelkozQ",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VelkozQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "Plasma Fission",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VelkozQSplit",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "Tectonic Disruption",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VelkozE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vi",
                    DisplayName = "Vault Breaker",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ViQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vi",
                    DisplayName = "Assault and Battery",
                    Slot = SpellSlot.R,
                    BuffName = "ViR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Viktor",
                    DisplayName = "Gravity Field",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "ViktorGravitonField",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vayne",
                    DisplayName = "Condemn",
                    Slot = SpellSlot.E,
                    BuffName = "Vayne Condemn",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Warwick",
                    DisplayName = "Infinite Duress",
                    Slot = SpellSlot.R,
                    BuffName = "InfiniteDuress",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Xerath",
                    DisplayName = "Eye of Destruction",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "XerathArcaneBarrage2",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Xerath",
                    DisplayName = "Shocking Orb",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "XerathMageSpearMissile",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "XinZhao",
                    DisplayName = "Three Talon Strike",
                    Slot = SpellSlot.Q,
                    BuffName = "XenZhaoComboTarget",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "XinZhao",
                    DisplayName = "Audacious Charge",
                    Slot = SpellSlot.E,
                    BuffName = "XenZhaoSweep",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "XinZhao",
                    DisplayName = "Crescent Sweep",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "XenZhaoParry",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "yasuoq2",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq2",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "yasuoq3w",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq3w",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "yasuoq",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zac",
                    DisplayName = "Stretching Strike",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZacQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zac",
                    DisplayName = "Lets Bounce!",
                    Slot = SpellSlot.R,
                    BuffName = "ZacR",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zed",
                    DisplayName = "Death Mark",
                    Slot = SpellSlot.R,
                    BuffName = "ZedUlt",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ziggs",
                    DisplayName = "Satchel Charge",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "ZiggsW",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zyra",
                    DisplayName = "Grasping Roots",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZyraGraspingRoots",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zyra",
                    DisplayName = "Stranglethorns",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZyraBrambleZone",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Taric",
                    DisplayName = "Dazzle",
                    Slot = SpellSlot.E,
                    BuffName = "Dazzle",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yoric",
                    DisplayName = "Omen of Pestilence",
                    Slot = SpellSlot.W,
                    BuffName = "YorickDecayed",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "Steel Tempest (3)",
                    Slot = SpellSlot.Q,
                    BuffName = "YasuoQ3",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fiddlesticks",
                    DisplayName = "Dark Wind",
                    Slot = SpellSlot.E,
                    BuffName = "FiddlesticksDarkWind",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank",
                    DisplayName = "Static Field",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "StaticField",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Chogath",
                    DisplayName = "Feral Scream",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "FeralScream",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malzahar",
                    DisplayName = "Call of the Void",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "AlZaharCalloftheVoid",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Garen",
                    DisplayName = "Decisive Strike",
                    Slot = SpellSlot.Q,
                    BuffName = "GarenQ",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Viktor",
                    DisplayName = "Chaos Storm",
                    Slot = SpellSlot.R,
                    BuffName = "ViktorChaosStorm",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Soraka",
                    DisplayName = "Equinox",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SorakaE",
                    CanBlockWith = new[] { BlockableSpells.QuickSilverSash, BlockableSpells.SivirE },
                    DefaultMenuValue = true,
                    Delay = 0
                });
        }
    }
}
