// Copyright 2014 - 2014 Esk0r
// Geometry.cs is part of Evade.
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

using System.Linq;
using SharpDX.Direct3D9;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

#endregion

namespace TwistedFate.Common
{
    using System;
    using System.Collections.Generic;

    using ClipperLib;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;


    using Color = System.Drawing.Color;

    /// <summary>
    ///     Class that contains the geometry related methods.
    /// </summary>
    public static class CommonGeometry
    {
        #region Constants

        private const int CircleLineSegmentN = 22;

        #endregion

        public static Font Text;
        public static Font TextPassive;

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            Text.Dispose();
            TextPassive.Dispose();
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            Text.OnResetDevice();
            TextPassive.OnResetDevice();
        }

        private static void DrawingOnOnPreReset(EventArgs args)
        {
            Text.OnLostDevice();
            TextPassive.OnLostDevice();
        }

        public static void Init()
        {
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Draft
                });
            TextPassive = new Font(
               Drawing.Direct3DDevice,
               new FontDescription
               {
                   FaceName = "Calibri",
                   Height = 11,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.Draft
               });
            Drawing.OnPreReset += DrawingOnOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
        }

        public static void DrawBox(Vector2 position, float width, int height, System.Drawing.Color color, int borderwidth, System.Drawing.Color borderColor, string text = "")
        {
            if (color != Color.Transparent)
            {
                Drawing.DrawLine(position.X, position.Y, position.X + width, position.Y, height, color);
            }

            if (borderwidth > 0)
            {
                Drawing.DrawLine(position.X, position.Y, position.X + width - 1, position.Y, borderwidth, borderColor);
                Drawing.DrawLine(position.X, position.Y + height, position.X + width - 1, position.Y + height, borderwidth, borderColor);
                Drawing.DrawLine(position.X, position.Y + 1, position.X, position.Y + height, borderwidth,
                    borderColor);
                Drawing.DrawLine(position.X + width, position.Y + 1, position.X + width, position.Y + height,
                    borderwidth, borderColor);
            }

            if (text != "")
            {

            }
        }

        public static void DrawText(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor, bool shadow = false)
        {
            if (shadow)
            {
                vFont.DrawText(null, vText, (int)vPosX + 2, (int)vPosY + 2, SharpDX.Color.Black);
            }
            vFont.DrawText(null, vText, (int)vPosX, (int)vPosY, vColor);
        }


        #region Public Methods and Operators

        public static Paths ClipPolygons(List<Polygon> polygons)
        {
            var subj = new Paths(polygons.Count);
            var clip = new Paths(polygons.Count);

            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }

            var solution = new Paths();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);

            return solution;
        }

        public static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
        {
            var from = Drawing.WorldToScreen(start);
            var to = Drawing.WorldToScreen(end);
            Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
        }

        /// <summary>
        ///     Returns the position on the path after t milliseconds at speed speed.
        /// </summary>
        public static Vector2 PositionAfter(this GamePath self, int t, int speed, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * speed / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int)to.Distance(from);
                if (d > distance)
                {
                    return from + distance * (to - from).Normalized();
                }
                distance -= d;
            }
            return self[self.Count - 1];
        }

        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        public static Polygon ToPolygon(this Path v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }
            return polygon;
        }

        //Clipper
        public static List<Polygon> ToPolygons(this Paths v)
        {
            var result = new List<Polygon>();

            foreach (var path in v)
            {
                result.Add(path.ToPolygon());
            }

            return result;
        }

        #endregion

        public class Circle
        {
            #region Fields

            public Vector2 Center;

            public float Radius;

            #endregion

            #region Constructors and Destructors

            public Circle(Vector2 center, float radius)
            {
                this.Center = center;
                this.Radius = radius;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();
                var outRadius = (overrideWidth > 0
                    ? overrideWidth
                    : (offset + this.Radius) / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN));

                for (var i = 1; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        this.Center.X + outRadius * (float)Math.Cos(angle),
                        this.Center.Y + outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }

            #endregion
        }

        public class Circle2
        {
            #region Fields

            public Vector2 Center;

            public float Radius;

            public float CurrentLineSegmentN;

            public float MaxLineSegmentN;

            #endregion

            #region Constructors and Destructors

            public Circle2(Vector2 center, float radius, float currentLineSegmentN, float maxLineSegmentN)
            {
                this.Center = center;
                this.Radius = radius;
                this.CurrentLineSegmentN = currentLineSegmentN;
                this.MaxLineSegmentN = maxLineSegmentN;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon2 ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon2();
                var outRadius = (overrideWidth > 0
                    ? overrideWidth
                    : (offset + this.Radius) / (float)Math.Cos(2 * Math.PI / MaxLineSegmentN));

                for (var i = MaxLineSegmentN; i >= CurrentLineSegmentN; i--)
                {
                    var angle = i * 2 * Math.PI / MaxLineSegmentN;
                    var point = new Vector2(this.Center.X + outRadius * (float)Math.Cos(angle),
                        this.Center.Y + outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }

            #endregion
        }

        public class Polygon
        {
            #region Fields

            public List<Vector2> Points = new List<Vector2>();

            #endregion

            #region Public Methods and Operators

            public void Add(Vector2 point)
            {
                this.Points.Add(point);
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= this.Points.Count - 1; i++)
                {
                    var nextIndex = (this.Points.Count - 1 == i) ? 0 : (i + 1);
                    DrawLineInWorld(this.Points[i].To3D(), this.Points[nextIndex].To3D(), width, color);
                }
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector2 point)
            {
                return !IsOutside(point);
            }

            public bool IsInside(List<Vector2> point)
            {
                return point.Select(p => !IsOutside(p)).FirstOrDefault();
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector3 point)
            {
                return !IsOutside(point.To2D());
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(GameObject point)
            {
                return !IsOutside(point.Position.To2D());
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, this.ToClipperPath()) != 1;
            }

            public Path ToClipperPath()
            {
                var result = new Path(this.Points.Count);

                result.AddRange(this.Points.Select(point => new IntPoint(point.X, point.Y)));

                return result;
            }

            #endregion
        }

        public class Polygon2
        {
            #region Fields

            public List<Vector2> Points = new List<Vector2>();

            #endregion

            #region Public Methods and Operators

            public void Add(Vector2 point)
            {
                this.Points.Add(point);
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= this.Points.Count - 1; i++)
                {
                    var nextIndex = (this.Points.Count - 1 == i) ? i : (i + 1);
                    DrawLineInWorld(this.Points[i].To3D(), this.Points[nextIndex].To3D(), width, color);
                    //DrawLineInWorld(this.Points[i].To3D(), this.Points[nextIndex].To3D(), width, color);
                }
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector2 point)
            {
                return !IsOutside(point);
            }

            public bool IsInside(List<Vector2> point)
            {
                return point.Select(p => !IsOutside(p)).FirstOrDefault();
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector3 point)
            {
                return !IsOutside(point.To2D());
            }

            /// <summary>
            /// Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(GameObject point)
            {
                return !IsOutside(point.Position.To2D());
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, this.ToClipperPath()) != 1;
            }

            public Path ToClipperPath()
            {
                var result = new Path(this.Points.Count);

                result.AddRange(this.Points.Select(point => new IntPoint(point.X, point.Y)));

                return result;
            }

            #endregion
        }

        public class Rectangle
        {
            #region Fields

            public Vector2 Direction;

            public Vector2 Perpendicular;

            public Vector2 REnd;

            public Vector2 RStart;

            public float Width;

            #endregion

            #region Constructors and Destructors

            public Rectangle(Vector2 start, Vector2 end, float width)
            {
                this.RStart = start;
                this.REnd = end;
                this.Width = width;
                this.Direction = (end - start).Normalized();
                this.Perpendicular = this.Direction.Perpendicular();
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();

                result.Add(
                    this.RStart + (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                    - offset * this.Direction);
                result.Add(
                    this.RStart - (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                    - offset * this.Direction);
                result.Add(
                    this.REnd - (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                    + offset * this.Direction);
                result.Add(
                    this.REnd + (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                    + offset * this.Direction);

                return result;
            }

            #endregion
        }

        public class Ring
        {
            #region Fields

            public Vector2 Center;

            public float Radius;

            public float RingRadius; //actually radius width.

            #endregion

            #region Constructors and Destructors

            public Ring(Vector2 center, float radius, float ringRadius)
            {
                this.Center = center;
                this.Radius = radius;
                this.RingRadius = ringRadius;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();

                var outRadius = (offset + this.Radius + this.RingRadius)
                                / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN);
                var innerRadius = this.Radius - this.RingRadius - offset;

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        this.Center.X - outRadius * (float)Math.Cos(angle),
                        this.Center.Y - outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        this.Center.X + innerRadius * (float)Math.Cos(angle),
                        this.Center.Y - innerRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }

            #endregion
        }

        public class Sector
        {
            #region Fields

            public float Angle;

            public Vector2 Center;

            public Vector2 Direction;

            public float Radius;

            #endregion

            #region Constructors and Destructors

            public Sector(Vector2 center, Vector2 direction, float angle, float radius)
            {
                this.Center = center;
                this.Direction = direction;
                this.Angle = angle;
                this.Radius = radius;
            }

            #endregion

            #region Public Methods and Operators

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();
                var outRadius = (this.Radius + offset) / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN);

                result.Add(this.Center);
                var Side1 = this.Direction.Rotated(-this.Angle * 0.5f);

                for (var i = 0; i <= CircleLineSegmentN; i++)
                {
                    var cDirection = Side1.Rotated(i * this.Angle / CircleLineSegmentN).Normalized();
                    result.Add(
                        new Vector2(this.Center.X + outRadius * cDirection.X, this.Center.Y + outRadius * cDirection.Y));
                }

                return result;
            }

            #endregion
        }
    }
}