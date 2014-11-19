#region

using System.Linq;
using LeagueSharp;

#endregion

namespace Marksman
{
    internal static class Utils
    {
        private static readonly string[] BetterWithEvade =
        {
            "Corki", "Ezreal", "Graves", "Lucian", "Sivir", "Tristana", "Caitlyn", "Vayne"
        };

        public static void PrintMessage(string message)
        {
            Game.PrintChat("<font color='#70DBDB'>Marksman:</font> <font color='#FFFFFF'>" + message + "</font>");
            
            foreach (
                var xChampList in BetterWithEvade.Where(xChampList => ObjectManager.Player.ChampionName == xChampList))
            {
                Game.PrintChat(
                    string.Format(
                        "<font color='#70DBDB'>We recommend you use Evade# with </font>{0}<font color='#70DBDB'> http://joduska.me/forum/topic/26-",
                        xChampList));
            }
        }
    }
}
