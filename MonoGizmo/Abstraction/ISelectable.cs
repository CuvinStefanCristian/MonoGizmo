using Microsoft.Xna.Framework;

namespace MonoGizmo.Abstraction
{
    public interface ISelectable
    {
        Vector2 Center { get; }
        float Rotation { get; }

        bool IsSelected(Point point);
        void Translate(Vector2 translate);
        void Scale(Vector2 scale, Vector2 currentMousePosition);
        void Rotate(float radians);
    }
}
