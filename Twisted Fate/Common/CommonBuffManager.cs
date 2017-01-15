using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace TwistedFate.Common
{

    public class CommonBuffManager
    {
        public static List<BuffDatabase> Spells = new List<BuffDatabase>();
        public static List<JungleBuffs> JungleBuffs = new List<JungleBuffs>();
        public static List<PassiveBuffs> PassiveBuffs = new List<PassiveBuffs>();


        public CommonBuffManager()
        {//teemo: CamouflagueBuff

            #region Teleport
            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Teleport",
                    BuffName = "Teleport",
                    Color = Color.Bisque
                });
            #endregion Zhonya

            #region Zhonya
            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Zhonya",
                    BuffName = "Zhonyas Ring",
                    Color = Color.Bisque
                });
            #endregion Zhonya

            #region Aatrox
            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Aatrox",
                    BuffName = "AatroxWONHLifeBuff",
                    Color = Color.FromArgb(85, 4, 144)
                });

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Aatrox",
                    BuffName = "AatroxPassiveActivate",
                    Color = Color.FromArgb(85, 4, 144)
                });
            #endregion Aatrox


            #region Anivia
            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Anivia Passive",
                    BuffName = "Rebirth",
                    Color = Color.FromArgb(85, 4, 144)
                });

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Anivia",
                    BuffName = "RebirthCooldown",
                    Color = Color.FromArgb(85, 4, 144)
                });
            #endregion Anivia

            #region Volibear

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Volibear",
                    BuffName = "VolibearPassiveCD",
                    Color = Color.Red
                });
            #endregion Volibear

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Zac",
                    BuffName = "ZacRebirthCooldown",
                    Color = Color.FromArgb(85, 4, 144)
                });

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "Zac",
                    BuffName = "zacrebirthstart",
                    Color = Color.FromArgb(85, 4, 144)
                });


            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "TwistedFate",
                    BuffName = "Pick A Card Gold",
                    Color = Color.FromArgb(205, 255, 241, 0)
                });

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "TwistedFate",
                    BuffName = "Pick A Card Blue",
                    Color = Color.FromArgb(207, 0, 31, 255)
                });

            PassiveBuffs.Add(
                new PassiveBuffs
                {
                    ChampionName = "TwistedFate",
                    BuffName = "Pick A Card Red",
                    Color = Color.FromArgb(203, 255, 0, 0)
                });

           





            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region Blue

            JungleBuffs.Add(new JungleBuffs
            {
                Number = 1,
                BuffName = "CrestoftheAncientGolem",
                Color = System.Drawing.Color.Blue
            });

            #endregion Blue

            #region Red

            JungleBuffs.Add(new JungleBuffs
            {
                Number = 0,
                BuffName = "BlessingoftheLizardElder",
                Color = System.Drawing.Color.Red
            });

            #endregion Red

            #region RiftHerald

            JungleBuffs.Add(new JungleBuffs
            {
                Number = 2,
                BuffName = "RiftHeraldBuffCounter",
                Color = System.Drawing.Color.Indigo
            });
            #endregion RiftHerald
        }

    }

    public class BuffDatabase
    {
        public string ChampionName;
        public int NetworkId;
        public string BuffName;
        public SpellSlot Slot;

        public BuffDatabase() { }

        public BuffDatabase(string championName, int networkId, string buffName, SpellSlot slot)
        {
            ChampionName = championName;
            NetworkId = networkId;
            BuffName = buffName;
            Slot = slot;
        }
    }

    public class JungleBuffs
    {
        public int Number;
        public string BuffName;
        public System.Drawing.Color Color;

        public JungleBuffs() { }

        public JungleBuffs(int number, string buffName, System.Drawing.Color color)
        {
            Number = number;
            BuffName = buffName;
            Color = color;
        }
    }


    public class PassiveBuffs
    {
        public string ChampionName;
        public string BuffName;
        public System.Drawing.Color Color;

        public PassiveBuffs() { }

        public PassiveBuffs(string championName, string buffName, System.Drawing.Color color)
        {
            ChampionName = championName;
            BuffName = buffName;
            Color = color;
        }
    }
}
