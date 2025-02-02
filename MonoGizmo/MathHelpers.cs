using Microsoft.Xna.Framework;
using System;

namespace MonoGizmo
{
    internal static class MathHelpers
    {
        public static float ToDegrees(float radians) =>
            radians / (MathF.PI * 2.0f) * 360.0f;

        public static float ToRadians(float degrees) =>
            degrees / 360.0f * MathF.PI * 2.0f;

        public static Vector2 GetRigidRotation(Vector2 rigidPoint, Vector2 movingPoint, float degrees)
        {
            return new Vector2()
            {
                X = rigidPoint.X + (movingPoint.X - rigidPoint.X) * MathF.Cos(degrees) + (movingPoint.Y - rigidPoint.Y) * MathF.Sin(degrees),
                Y = rigidPoint.Y + (movingPoint.X - rigidPoint.X) * MathF.Sin(degrees) + (movingPoint.Y - rigidPoint.Y) * MathF.Cos(degrees)
            };
        }

        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }
    }
}
