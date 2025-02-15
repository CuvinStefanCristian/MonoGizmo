using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.Enums;
using MonoGizmo.Graphics;

namespace MonoGizmo.Gizmos
{
    internal sealed class ScaleGizmo : GizmoBase
    {
        Rectangle _xBounds, _yBounds;

        bool _isXToggled, _isYToggled;
        Point _previousMousePosition;

        public ScaleGizmo(GizmoManager gizmoManager) : base(gizmoManager)
        {
            GizmoType = GizmoType.Scale;
        }

        public override void Attach(ISelectable selectable)
        {
            base.Attach(selectable);

            CalculateBounds();
        }

        void CalculateBounds()
        {
            // Base offsets for X and Y axes in local space
            Vector2 localXOffset = new Vector2(GizmoManager.Options.ScaleLength, 0); // Offset on X-axis
            Vector2 localYOffset = new Vector2(0, -GizmoManager.Options.ScaleLength); // Offset on Y-axis

            // Rotate offsets to align with the entity's rotation
            if (GizmoManager.TransformType == TransformType.Local)
            {
                localXOffset = MathHelpers.GetRigidRotation(Vector2.Zero, localXOffset, SelectedItem.Rotation);
                localYOffset = MathHelpers.GetRigidRotation(Vector2.Zero, localYOffset, -SelectedItem.Rotation);
            }

            // Translate to world space by adding the entity's center position
            Vector2 xOffset = SelectedItem.Center + localXOffset;
            Vector2 yOffset = SelectedItem.Center + localYOffset;

            // Calculate X and Y bounds
            _xBounds = new Rectangle()
            {
                X = (int)(xOffset.X - GizmoManager.Options.ScaleSize * 0.75f),
                Y = (int)(xOffset.Y - GizmoManager.Options.ScaleSize * 2),
                Width = (int)(GizmoManager.Options.ScaleSize * 1.5f),
                Height = (int)(GizmoManager.Options.ScaleSize * 4)
            };

            _yBounds = new Rectangle()
            {
                X = (int)(yOffset.X - GizmoManager.Options.ScaleSize * 2),
                Y = (int)(yOffset.Y - GizmoManager.Options.ScaleSize * 0.75f),
                Width = (int)(GizmoManager.Options.ScaleSize * 4),
                Height = (int)(GizmoManager.Options.ScaleSize * 1.5f)
            };
        }

        public override void Update(MouseState mouseState, GameTime gameTime)
        {
            CalculateBounds();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Calculate mouse movement delta
                Vector2 delta = mouseState.Position.ToVector2() - _previousMousePosition.ToVector2();

                // X Axis Scaling
                if (_isXToggled || (!_isXToggled && _xBounds.Contains(mouseState.Position)))
                {
                    if (!_isXToggled)
                    {
                        _isXToggled = IsToggled = true;
                    }

                    if (delta.X != 0)
                    {
                        float scaleX = delta.X > 0 ? GizmoManager.Options.ScaleStep : -GizmoManager.Options.ScaleStep;
                        Vector2 scaleVector = new Vector2(scaleX, 0f);

                        if (GizmoManager.TransformType == TransformType.Local)
                        {
                            // Rotate scaling vector to align with the local X-axis
                            scaleVector = MathHelpers.RotateVector(scaleVector, SelectedItem.Rotation);
                            scaleVector.Y = 0f; // Ensure Y-axis scaling is not affected
                        }

                        SelectedItem.Scale(scaleVector, mouseState.Position.ToVector2());
                    }
                }

                // Y Axis Scaling
                if (_isYToggled || (!_isYToggled && _yBounds.Contains(mouseState.Position)))
                {
                    if (!_isYToggled)
                    {
                        _isYToggled = IsToggled = true;
                    }

                    if (delta.Y != 0)
                    {
                        float scaleY = delta.Y > 0 ? GizmoManager.Options.ScaleStep : -GizmoManager.Options.ScaleStep;
                        Vector2 scaleVector = new Vector2(0f, scaleY);

                        if (GizmoManager.TransformType == TransformType.Local)
                        {
                            // Rotate scaling vector to align with the local Y-axis
                            scaleVector = MathHelpers.RotateVector(scaleVector, SelectedItem.Rotation);
                            scaleVector.X = 0f; // Ensure X-axis scaling is not affected
                        }

                        SelectedItem.Scale(scaleVector, mouseState.Position.ToVector2());
                    }
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                _isXToggled = _isYToggled = IsToggled = false;
            }

            _previousMousePosition = mouseState.Position;
        }

        public override void Draw(ShapeBatch shapeBatch, GameTime gameTime)
        {
            if(GizmoManager.TransformType == TransformType.World)
            {
                shapeBatch.FillRectangle(_xBounds.Location.ToVector2(), _xBounds.Size.ToVector2(), GizmoManager.Options.XAxisColor);
                shapeBatch.FillRectangle(_yBounds.Location.ToVector2(), _yBounds.Size.ToVector2(), GizmoManager.Options.YAxisColor);
            }
            else
            {
                shapeBatch.FillRectangle(_xBounds.Location.ToVector2(), _xBounds.Size.ToVector2(), GizmoManager.Options.XAxisColor, rotation:SelectedItem.Rotation);
                shapeBatch.FillRectangle(_yBounds.Location.ToVector2(), _yBounds.Size.ToVector2(), GizmoManager.Options.YAxisColor, rotation:SelectedItem.Rotation);
            }
        }
    }
}
