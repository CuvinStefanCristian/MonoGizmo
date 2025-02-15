using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.DataStructures;
using MonoGizmo.Enums;
using MonoGizmo.Graphics;
using System;

namespace MonoGizmo.Gizmos
{
    internal sealed class RotationGizmo : GizmoBase
    {
        Circle _outerBounds, _innerBounds;

        float _initialAngle;
        Vector2 _initialMousePosition;

        public RotationGizmo(GizmoManager gizmoManager) : base(gizmoManager)
        {
            GizmoType = GizmoType.Rotate;
        }

        public override void Attach(ISelectable selectable)
        {
            base.Attach(selectable);

            CalculateBounds();
        }

        void CalculateBounds()
        {
            _outerBounds = new Circle(SelectedItem.Center, GizmoManager.Options.RotationRadius);
            _innerBounds = new Circle(SelectedItem.Center, _outerBounds.Radius - GizmoManager.Options.RotationThickness);
        }

        public bool CheckBounds(Point point)
        {
            return _outerBounds.Contains(point) && !_innerBounds.Contains(point);
        }

        public override void Update(MouseState mouseState, GameTime gameTime)
        {
            CalculateBounds();

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if(IsToggled || CheckBounds(mouseState.Position))
                { 
                    if (!IsToggled)
                    {
                        IsToggled = true;
                        _initialMousePosition = mouseState.Position.ToVector2();
                        var initialDelta = _initialMousePosition - SelectedItem.Center;
                        _initialAngle = MathF.Atan2(initialDelta.Y, initialDelta.X);
                    }

                    // Calculate the current angle relative to the SelectedItem's center
                    var currentDelta = mouseState.Position.ToVector2() - SelectedItem.Center;
                    var currentAngle = MathF.Atan2(currentDelta.Y, currentDelta.X);

                    // Compute the delta angle and rotate the SelectedItem incrementally
                    var deltaAngle = currentAngle - _initialAngle;
                    SelectedItem.Rotate(deltaAngle);

                    // Update the initial angle to the current angle for the next frame
                    _initialAngle = currentAngle;
                }
            }
            else if(mouseState.LeftButton == ButtonState.Released)
            {
                IsToggled = false;
            }
        }

        public override void Draw(ShapeBatch shapeBatch, GameTime gameTime)
        {
            shapeBatch.BorderCircle(_outerBounds.Center, GizmoManager.Options.RotationRadius, GizmoManager.Options.RotationColor, GizmoManager.Options.RotationThickness);
        }
    }
}
