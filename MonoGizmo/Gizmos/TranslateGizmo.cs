using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.DataStructures;
using MonoGizmo.Enums;

namespace MonoGizmo.Gizmos
{
    internal sealed class TranslateGizmo : GizmoBase
    {
        readonly float X_TRIANGLE_ROTATION = MathHelpers.ToRadians(270), Y_TRIANGLE_ROTATION = MathHelpers.ToRadians(180);

        Circle _xBounds, _yBounds;

        bool _isXToggled, _isYToggled;
        Vector2 _xDrawOffset, _yDrawOffset, _xOffset, _yOffset;

        public TranslateGizmo(GizmoManager gizmoManager) : base(gizmoManager)
        {
            GizmoType = GizmoType.Translate;
        }

        public override void Attach(ISelectable selectable)
        {
            base.Attach(selectable);
            CalculateOffsets();
        }

        /// <summary>
        /// Calculates the offset of the position of the Gizmo relative to the center of the ISelectable for both X and Y axes.
        /// </summary>
        void CalculateOffsets()
        {
            _xDrawOffset = new Vector2(SelectedItem.Center.X + GizmoManager.Options.TranslateLength, SelectedItem.Center.Y);
            _yDrawOffset = new Vector2(SelectedItem.Center.X, SelectedItem.Center.Y - GizmoManager.Options.TranslateLength);

            if (GizmoManager.TransformType == TransformType.Local)
            {
                _xDrawOffset = MathHelpers.GetRigidRotation(SelectedItem.Center, _xDrawOffset, SelectedItem.Rotation);
                _yDrawOffset = MathHelpers.GetRigidRotation(SelectedItem.Center, _yDrawOffset, -SelectedItem.Rotation);
            }

            _xBounds = new Circle(_xDrawOffset, GizmoManager.Options.TranslateSize + 1);
            _yBounds = new Circle(_yDrawOffset, GizmoManager.Options.TranslateSize + 1);
        }

        public override void Update(MouseState mouseState, GameTime gameTime)
        {
            // Update offsets relative to the selected item's center
            CalculateOffsets();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_isXToggled || (!_isYToggled && _xBounds.Contains(mouseState.Position)))
                {
                    if (!_isXToggled)
                    {
                        // Start dragging on the X-axis
                        _isXToggled = IsToggled = true;
                        _xOffset = mouseState.Position.ToVector2() - SelectedItem.Center;
                    }

                    // Translate along the X-axis
                    Vector2 translation = new Vector2(
                        mouseState.Position.X - _xOffset.X - SelectedItem.Center.X,
                        0f);

                    if (GizmoManager.TransformType == TransformType.Local)
                    {
                        // Rotate translation vector by the entity's rotation angle
                        translation = MathHelpers.RotateVector(translation, SelectedItem.Rotation);
                    }

                    SelectedItem.Translate(translation);
                }
                else if (_isYToggled || (!_isXToggled && _yBounds.Contains(mouseState.Position)))
                {
                    if (!_isYToggled)
                    {
                        // Start dragging on the Y-axis
                        _isYToggled = IsToggled = true;
                        _yOffset = mouseState.Position.ToVector2() - SelectedItem.Center;
                    }

                    // Translate along the Y-axis
                    Vector2 translation = new Vector2(
                        0f,
                        mouseState.Position.Y - _yOffset.Y - SelectedItem.Center.Y);

                    if (GizmoManager.TransformType == TransformType.Local)
                    {
                        // Rotate translation vector by the entity's rotation angle
                        translation = MathHelpers.RotateVector(translation, SelectedItem.Rotation);
                    }

                    SelectedItem.Translate(translation);
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                // Reset dragging states and offsets
                _isXToggled = _isYToggled = IsToggled = false;
                _xOffset = _yOffset = Vector2.Zero;
            }
        }

        public override void Draw(ShapeBatch shapeBatch, GameTime gameTime)
        {
            // Draw the X-axis line and triangle
            shapeBatch.FillLine(SelectedItem.Center, _xDrawOffset, 0.2f, GizmoManager.Options.XAxisColor);
            if(GizmoManager.TransformType == TransformType.World)
                shapeBatch.FillEquilateralTriangle(_xDrawOffset, GizmoManager.Options.TranslateSize, GizmoManager.Options.XAxisColor, 0f, X_TRIANGLE_ROTATION);
            else
                shapeBatch.FillEquilateralTriangle(_xDrawOffset, GizmoManager.Options.TranslateSize, GizmoManager.Options.XAxisColor, 0f, SelectedItem.Rotation + X_TRIANGLE_ROTATION);

            // Draw the Y-axis line and triangle
            shapeBatch.FillLine(SelectedItem.Center, _yDrawOffset, 0.2f, GizmoManager.Options.YAxisColor);
            if(GizmoManager.TransformType == TransformType.World)
                shapeBatch.FillEquilateralTriangle(_yDrawOffset, GizmoManager.Options.TranslateSize, GizmoManager.Options.YAxisColor, 0f, Y_TRIANGLE_ROTATION);
            else
                shapeBatch.FillEquilateralTriangle(_yDrawOffset, GizmoManager.Options.TranslateSize, GizmoManager.Options.YAxisColor, 0f, SelectedItem.Rotation + Y_TRIANGLE_ROTATION);
        }
    }
}
