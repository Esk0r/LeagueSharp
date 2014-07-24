#region

using LeagueSharp;

#endregion

namespace Marksman
{
    internal static class Utils
    {
        public static void PrintMessage(string message)
        {
            Game.PrintChat("<font color='#70DBDB'>Marksman:</font> <font color='#FFFFFF'>" + message + "</font>");
        }
    }
}