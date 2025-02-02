using Microsoft.Xna.Framework;
using System;

namespace MonoGizmo.DataStructures
{
    internal struct Circle
    {
        public Vector2 Center;
        public float Radius;

        public readonly float Diameter => 2 * Radius;
        public readonly float Circumference => 2 * MathF.PI * Radius;

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public readonly bool Contains(Point point)
        {
            var dx = Center.X - point.X;
            var dy = Center.Y - point.Y;
            var d2 = dx * dx + dy * dy;
            var r2 = Radius * Radius;
            return d2 <= r2;
        }
    }
}
