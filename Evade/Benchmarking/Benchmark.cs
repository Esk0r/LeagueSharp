using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Evade.Benchmarking
{
    public static class Benchmark
    {
        private static Vector2 startPoint;
        private static Vector2 endPoint;

        public static void Initialize()
        {
            Game.OnWndProc += Game_OnWndProc;
        }


        static void SpawnLineSkillShot(Vector2 start, Vector2 end)
        {
            SkillshotDetector.TriggerOnDetectSkillshot(
                   DetectionType.ProcessSpell, SpellDatabase.GetByName("TestLineSkillShot"), Utils.TickCount,
                   start, end, end, ObjectManager.Player);

            Utility.DelayAction.Add(5000, () => SpawnLineSkillShot(start, end));
        }

        static void SpawnCircleSkillShot(Vector2 start, Vector2 end)
        {
            SkillshotDetector.TriggerOnDetectSkillshot(
                   DetectionType.ProcessSpell, SpellDatabase.GetByName("TestCircleSkillShot"), Utils.TickCount,
                   start, end, end, ObjectManager.Player);

            Utility.DelayAction.Add(5000, () => SpawnCircleSkillShot(start, end));
        }


        static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                startPoint = Game.CursorPos.To2D();
            }

            if (args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
            {
                endPoint = Game.CursorPos.To2D();
            }

            if (args.Msg == (uint)WindowsMessages.WM_KEYUP && args.WParam == 'L') //line missile skillshot
            {
                SpawnLineSkillShot(startPoint, endPoint);
            }

            if (args.Msg == (uint)WindowsMessages.WM_KEYUP && args.WParam == 'I') //circular skillshoot
            {
                SpawnCircleSkillShot(startPoint, endPoint);
            }


        }
    }
}
