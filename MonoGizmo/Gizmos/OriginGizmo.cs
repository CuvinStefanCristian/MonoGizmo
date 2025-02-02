using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.DataStructures;
using MonoGizmo.Enums;

namespace MonoGizmo.Gizmos
{
    internal sealed class OriginGizmo : GizmoBase
    {
        Circle _bounds;
        Vector2? _offset;

        public OriginGizmo(GizmoManager gizmoManager) : base(gizmoManager)
        {
            GizmoType = GizmoType.Origin;
        }

        public override void Attach(ISelectable selectable)
        {
            base.Attach(selectable);

            _bounds = new Circle(SelectedItem.Center, GizmoManager.Options.OriginSize * GizmoManager.Options.GizmoScale);
        }

        public override void Update(MouseState mouseState, GameTime gameTime)
        {
            // Ensure Bounds.Center is in sync with SelectedItem.Center
            _bounds = new Circle(SelectedItem.Center, GizmoManager.Options.OriginSize * GizmoManager.Options.GizmoScale);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (IsToggled || _bounds.Contains(mouseState.Position))
                {
                    if (!IsToggled)
                    {
                        // Start dragging and calculate Offset
                        IsToggled = true;
                        _offset = mouseState.Position.ToVector2() - _bounds.Center;
                    }

                    // Move the SelectedItem based on mouse position
                    SelectedItem.Translate((mouseState.Position.ToVector2() - _offset - SelectedItem.Center).Value);
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                // Stop dragging when the mouse button is released
                IsToggled = false;
                _offset = null;
            }
        }

        public override void Draw(ShapeBatch shapeBatch, GameTime gameTime)
        {
            shapeBatch.FillCircle(SelectedItem.Center, _bounds.Radius, Color.Yellow);
            shapeBatch.BorderCircle(SelectedItem.Center, _bounds.Radius, Color.Orange, 2);
        }
    }
}
