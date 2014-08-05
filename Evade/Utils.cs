#region

using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

#endregion

namespace Evade
{
    public static class Utils
    {
        public static List<Vector2> To2DList(this Vector3[] v)
        {
            var result = new List<Vector2>();
            foreach (var point in v)
                result.Add(point.To2D());
            return result;
        }

        public static void SendMovePacket(this Obj_AI_Base v, Vector2 point)
        {
            Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(point.X, point.Y)).Send();
        }

        public static Obj_AI_Base Closest(List<Obj_AI_Base> targetList, Vector2 from)
        {
            var dist = float.MaxValue;
            Obj_AI_Base result = null;

            foreach (var target in targetList)
            {
                var distance = Vector2.DistanceSquared(from, target.ServerPosition.To2D());
                if (distance < dist)
                {
                    dist = distance;
                    result = target;
                }
            }

            return result;
        }
    }

    internal class SpellList<T> : List<T>
    {
        public event EventHandler OnAdd;

        public new void Add(T item)
        {
            if (OnAdd != null)
                OnAdd(this, null);

            base.Add(item);
        }
    }
}