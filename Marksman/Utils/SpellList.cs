using System.Collections.Generic;
using LeagueSharp;

namespace Marksman.Utils
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
        public static List<SpellList> BuffList = new List<SpellList>();
        public BlockableSpells[] CanBlockWith = {};

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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 3
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Aatorx",
                    DisplayName = "Aatrox (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "AatroxQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ahri",
                    DisplayName = "Ahri (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "AhriSeduce",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Alistar",
                    DisplayName = "Alistar (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "Pulverize",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Alistar",
                    DisplayName = "Alistar (W)",
                    Slot = SpellSlot.W,
                    BuffName = "Headbutt",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Amumu",
                    DisplayName = "Amumu (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "BandageToss",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Amumu",
                    DisplayName = "Amumu (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "CurseoftheSadMummy",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Annie",
                    DisplayName = "Annie (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "InfernalGuardian",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Azir",
                    DisplayName = "Azir (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "AzirR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank",
                    DisplayName = "Blitzcrank (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RocketGrab",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank (E)",
                    DisplayName = "Power Fist",
                    Slot = SpellSlot.E,
                    BuffName = "PowerFist",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    BuffName = "caitlynaceinthehole",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Chogath",
                    DisplayName = "Rupture",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "Rupture",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Elise",
                    DisplayName = "Elise (Human-E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "EliseHumanE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Evelynn",
                    DisplayName = "Evelynn (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "EvelynnR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fizz",
                    DisplayName = "Fizz (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "FizzMarinerDoomMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Galio",
                    DisplayName = "Galio (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GalioResoluteSmite",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gnar",
                    DisplayName = "Gnar (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "GnarBigW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gnar",
                    DisplayName = "Gnar (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GnarR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Gragas (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GragasQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Gragas (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "GragasE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Gragas",
                    DisplayName = "Gragas (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "GragasR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Heimerdinger",
                    DisplayName = "Heimerdinger (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HeimerdingerE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Hecarim",
                    DisplayName = "Hecarim (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HecarimUlt"
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Hecarim",
                    DisplayName = "Hecarim (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "HecarimRamp",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Janna",
                    DisplayName = "Janna (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "HowlingGale",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Janna",
                    DisplayName = "Janna (W)",
                    Slot = SpellSlot.W,
                    BuffName = "ReapTheWhirlwind",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jax",
                    DisplayName = "Jax (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JaxCounterStrike",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 2
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "JarvanIV",
                    DisplayName = "Jarvan (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JarvanIVDragonStrike",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jayce",
                    DisplayName = "Jayce (Melee-E)",
                    Slot = SpellSlot.E,
                    BuffName = "JayceThunderingBlow",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jinx",
                    DisplayName = "Jinx (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JinxW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Jinx",
                    DisplayName = "Jinx (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JinxE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karma",
                    DisplayName = "Karma (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "KarmaQMantra",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karma",
                    DisplayName = "Karma (W)",
                    Slot = SpellSlot.W,
                    BuffName = "KarmaQMantra",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Kassadin",
                    DisplayName = "Kassadin (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "ForcePulse",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Khazix",
                    DisplayName = "Khazix (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "KhazixW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Kayle",
                    DisplayName = "Kayle (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "JudicatorReckoning",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "KogMaw",
                    DisplayName = "Kog'Maw (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "KogMawVoidOoze",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leblanc",
                    DisplayName = "Leblanc (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeblancSoulShackle",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leblanc",
                    DisplayName = "Leblanc (R->E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeblancSoulShackleM",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "LeeSin",
                    DisplayName = "LeeSin (R)",
                    Slot = SpellSlot.R,
                    BuffName = "BlindMonkRKick",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Leona (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LeonaZenithBlade",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Leona (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "LeonaShieldOfDaybreak",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Leona",
                    DisplayName = "Leona (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "LeonaSolarFlare",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lissandra",
                    DisplayName = "Lissandra (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LissandraQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lissandra",
                    DisplayName = "Lissandra (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LissandraW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lulu",
                    DisplayName = "Lulu (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuluQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lulu",
                    DisplayName = "Lulu (Q:Extended)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuluQMissileTwo",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Lux (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuxLightBinding",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Lux (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "LuxLightStrikeKugel",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Lux",
                    DisplayName = "Lux (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "LuxMaliceCannon",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malphite",
                    DisplayName = "Malphite (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "UFSlash",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malphite",
                    DisplayName = "Malphite (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SismicShard",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malzahar",
                    DisplayName = "Malzahar (R)",
                    Slot = SpellSlot.R,
                    BuffName = "AlZaharNetherGrasp",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Maokai",
                    DisplayName = "Maokai (W)",
                    Slot = SpellSlot.W,
                    BuffName = "MaokaiUnstableGrowth",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Maokai",
                    DisplayName = "MaoKai (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "MaokaiTrunkLine",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Mordekaiser",
                    DisplayName = "Mordekaiser (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "MordekaiserChildrenOfTheGrave",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Wukong",
                    DisplayName = "Wukong (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "MonkeyKingSpinToWin",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nami",
                    DisplayName = "Nami (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "NamiQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nami",
                    DisplayName = "Nami (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "NamiR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nasus",
                    DisplayName = "Nasus (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "NasusW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Karthus",
                    DisplayName = "Karthus (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "KarthusWallOfPain",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 3
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Nautilus (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "NautilusAnchorDragMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Nautilus (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "NautilusSplashZone",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nautilus",
                    DisplayName = "Nautilus (R)",
                    Slot = SpellSlot.R,
                    BuffName = "NautilusGrandLine",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nidalee",
                    DisplayName = "Nidalee (Q:Human)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "JavelinToss",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Olaf",
                    DisplayName = "Olaf (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "OlafAxeThrowCast",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Orianna",
                    DisplayName = "Orianna (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "OrianaDissonanceCommand",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Orianna",
                    DisplayName = "Orianna (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "OrianaDetonateCommand",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
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
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rammus",
                    DisplayName = "Rammus (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "PuncturingTaunt",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rengar",
                    DisplayName = "Rengar (E:Emp)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RengarEFinal",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Fiddlesticks",
                    DisplayName = "Fiddle (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "Terrify",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Renekton",
                    DisplayName = "Renekton (W)",
                    Slot = SpellSlot.W,
                    BuffName = "RenektonPreExecute",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Riven",
                    DisplayName = "Riven (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RivenMartyr",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rumble",
                    DisplayName = "Rumble (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RumbleGrenade",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Rumble",
                    DisplayName = "Rumble (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "RumbleCarpetBombMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ryze",
                    DisplayName = "Ryze (W)",
                    Slot = SpellSlot.W,
                    BuffName = "RunePrison",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sejuani",
                    DisplayName = "Sejuani (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SejuaniArcticAssault",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sejuani",
                    DisplayName = "Sejuani (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SejuaniGlacialPrisonStart",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Singed",
                    DisplayName = "Singed (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "MegaAdhesive",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Singed",
                    DisplayName = "Singed (E)",
                    Slot = SpellSlot.E,
                    BuffName = "Fling",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nocturne",
                    DisplayName = "Nocturne (E)",
                    Slot = SpellSlot.E,
                    BuffName = "NocturneUnspeakableHorror",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Shen",
                    DisplayName = "Shen (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ShenShadowDash",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Shyvana",
                    DisplayName = "Shyvana (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ShyvanaTransformCast",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Skarner",
                    DisplayName = "Skarner (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SkarnerFractureMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Skarner",
                    DisplayName = "Skarner (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SkarnerFractureMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Pantheon",
                    DisplayName = "Pantheon (W)",
                    Slot = SpellSlot.W,
                    BuffName = "PantheonW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Nunu",
                    DisplayName = "Nunu (E)",
                    Slot = SpellSlot.E,
                    BuffName = "Ice Blast",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Sona",
                    DisplayName = "Sona (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "SonaCrescendo",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Swain",
                    DisplayName = "Swain (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SwainShadowGrasp",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Syndra",
                    DisplayName = "Syndra (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "SyndraE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Thresh",
                    DisplayName = "Thresh (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ThreshQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Thresh",
                    DisplayName = "Thresh (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ThreshEFlay",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Tristana",
                    DisplayName = "Tristana (R)",
                    Slot = SpellSlot.R,
                    BuffName = "BusterShot",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Trundle",
                    DisplayName = "Trundle (E)",
                    Slot = SpellSlot.E,
                    BuffName = "TrundleCircle",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Trundle",
                    DisplayName = "Trundle (R)",
                    Slot = SpellSlot.R,
                    BuffName = "TrundlePain",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Tryndamere",
                    DisplayName = "Tryndamere (W)",
                    Slot = SpellSlot.W,
                    BuffName = "MockingShout",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });

            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Twitch",
                    DisplayName = "Twitch (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "TwitchVenomCaskMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Urgot",
                    DisplayName = "Urgot (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "UrgotPlasmaGrenadeBoom",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Varus",
                    DisplayName = "Varus (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VarusE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Varus",
                    DisplayName = "Varus (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VarusR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Veigar",
                    DisplayName = "Veigar (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VeigarEventHorizon",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "Velkoz (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VelkozQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "Velkoz (Q:Split)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "VelkozQSplit",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Velkoz",
                    DisplayName = "Velkoz (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "VelkozE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vi",
                    DisplayName = "Vi (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ViQ",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vi",
                    DisplayName = "Vi (R)",
                    Slot = SpellSlot.R,
                    BuffName = "ViR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Viktor",
                    DisplayName = "Viktor (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "ViktorGravitonField",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Vayne",
                    DisplayName = "Vayne (E)",
                    Slot = SpellSlot.E,
                    BuffName = "Vayne Condemn",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Warwick",
                    DisplayName = "Warwick (R)",
                    Slot = SpellSlot.R,
                    BuffName = "InfiniteDuress",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Xerath",
                    DisplayName = "Xerath (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "XerathArcaneBarrage2",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Xerath",
                    DisplayName = "Xerath (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "XerathMageSpearMissile",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "XinZhao",
                    DisplayName = "XinZhao (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "XenZhaoComboTarget",
                    CanBlockWith = new[] {BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "XinZhao",
                    DisplayName = "XinZhao (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "XenZhaoParry",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "Yasuo (Q2)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq2",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "Yasuo (Q3)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq3w",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yasuo",
                    DisplayName = "Yasuo (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "yasuoq",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zac",
                    DisplayName = "Zac (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZacQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zac",
                    DisplayName = "Zac (R)",
                    Slot = SpellSlot.R,
                    BuffName = "ZacR",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Ziggs",
                    DisplayName = "Ziggs (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "ZiggsW",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zyra",
                    DisplayName = "Zyra (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZyraGraspingRoots",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Zyra",
                    DisplayName = "Zyra (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "ZyraBrambleZone",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Taric",
                    DisplayName = "Taric (E)",
                    Slot = SpellSlot.E,
                    BuffName = "Dazzle",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Yoric",
                    DisplayName = "Yoric (W)",
                    Slot = SpellSlot.W,
                    BuffName = "YorickDecayed",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Blitzcrank",
                    DisplayName = "Blitzcrank (R)",
                    Slot = SpellSlot.R,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "StaticField",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Chogath",
                    DisplayName = "Cho'Gath (W)",
                    Slot = SpellSlot.W,
                    SkillType = SkillShotType.SkillshotCone,
                    BuffName = "FeralScream",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Malzahar",
                    DisplayName = "Malzahar (Q)",
                    Slot = SpellSlot.Q,
                    SkillType = SkillShotType.SkillshotLine,
                    BuffName = "AlZaharCalloftheVoid",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Garen",
                    DisplayName = "Garen (Q)",
                    Slot = SpellSlot.Q,
                    BuffName = "GarenQ",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Viktor",
                    DisplayName = "Viktor (R)",
                    Slot = SpellSlot.R,
                    BuffName = "ViktorChaosStorm",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
            BuffList.Add(
                new SpellList
                {
                    ChampionName = "Soraka",
                    DisplayName = "Soraka (E)",
                    Slot = SpellSlot.E,
                    SkillType = SkillShotType.SkillshotCircle,
                    BuffName = "SorakaE",
                    CanBlockWith = new[] {BlockableSpells.QuickSilverSash, BlockableSpells.SivirE},
                    DefaultMenuValue = true,
                    Delay = 0
                });
        }

        public string ChampionName { get; set; }
        public string DisplayName { get; set; }
        public string BuffName { get; set; }
        public SpellSlot Slot { get; set; }
        public SkillShotType SkillType { get; set; }
        public bool DefaultMenuValue { get; set; }
        public int Delay { get; set; }
    }
}