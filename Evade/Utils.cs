// Copyright 2014 - 2014 Esk0r
// Utils.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Evade
{
    public static class Utils
    {
        public static int TickCount => (int)(Game.Time * 1000f);

        public static List<Vector2> To2DList(this Vector3[] v)
        {
            return v.Select(point => point.To2D()).ToList();
        }

        public static void SendMovePacket(this Obj_AI_Base v, Vector2 point)
        {
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, point.To3D(), false);
        }

        public static Obj_AI_Base Closest(List<Obj_AI_Base> targetList, Vector2 from)
        {
            var dist = float.MaxValue;
            Obj_AI_Base result = null;

            foreach (var target in targetList)
            {
                var distance = Vector2.DistanceSquared(from, target.ServerPosition.To2D());
                if (!(distance < dist)) continue;
                dist = distance;
                result = target;
            }

            return result;
        }

        /// <summary>
        /// Returns when the unit will be able to move again
        /// </summary>
        public static int ImmobileTime(Obj_AI_Base unit)
        {
            var result = (from buff in unit.Buffs where buff.IsActive && Game.Time <= buff.EndTime && (buff.Type == BuffType.Charm || buff.Type == BuffType.Knockup || buff.Type == BuffType.Stun || buff.Type == BuffType.Suppression || buff.Type == BuffType.Snare) select buff.EndTime).Concat(new[] {0f}).Max();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return (result == 0f) ? -1 : (int) (TickCount + (result - Game.Time) * 1000);
        }


        public static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
        {
            var from = Drawing.WorldToScreen(start);
            var to = Drawing.WorldToScreen(end);
            Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
            //Drawing.DrawLine(from.X, from.Y, to.X, to.Y, width, color);
        }
    }

    internal class SpellList<T> : List<T>
    {
        public event EventHandler OnAdd;

        public new void Add(T item)
        {
            OnAdd?.Invoke(this, null);

            base.Add(item);
        }
    }
}