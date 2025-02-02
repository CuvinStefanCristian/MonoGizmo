using Microsoft.Xna.Framework;
using MonoGizmo.Abstraction;
using System;

namespace Pathfinder.Tests
{
    internal class Selectable(Rectangle bounds, Vector2 scale, float rotation) : ISelectable, IEntity
    {
        public Rectangle Bounds => _bounds;
        public Vector2 Scale => _scale;
        public float Rotation => _rotation;

        Rectangle _bounds = bounds;
        Vector2 _scale = scale;
        float _rotation = rotation;

        bool _fliped;
        float _previousMouseX, _previousMouseY;

        public Vector2 Center
        {
            get => _bounds.Center.ToVector2();
            set => _bounds.Location = new Point((int)value.X - _bounds.Width / 2, (int)value.Y - _bounds.Height / 2);
        }

        public bool IsSelected(Point point)
        {
            if(_bounds.Contains(point)) return true;
            return false;
        }

        public void Translate(Vector2 translate)
        {
            _bounds.Location += translate.ToPoint();
        }

        void ISelectable.Scale(Vector2 scale, Vector2 currentMousePosition)
        {
            // Calculate the current center of the bounds
            Vector2 center = new(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);

            // Handle X-axis scaling
            if (scale.X != 0)
            {
                float deltaX = currentMousePosition.X - _previousMouseX;  // Track mouse movement on the X-axis

                // Check if the mouse direction has changed
                bool isMouseMovingRight = deltaX > 0;
                bool isMouseMovingLeft = deltaX < 0;

                // Flip scaling direction or add to the width
                if (_bounds.Width + (int)scale.X < 0 || (_fliped && scale.X < 0))
                {
                    _bounds.Width += Math.Abs((int)scale.X);  // Add positive scaling
                    _fliped = true;
                }
                else
                {
                    if (isMouseMovingRight && !_fliped)  // If mouse is moving to the right, scale normally
                    {
                        _bounds.Width += (int)scale.X;
                    }
                    else if (isMouseMovingLeft && _fliped)  // If mouse is moving to the left, scale down
                    {
                        _bounds.Width -= Math.Abs((int)scale.X);  // Downscale
                    }
                    else
                    {
                        _bounds.Width += (int)scale.X;
                    }

                    _fliped = false;
                }

                // Recalculate X position to maintain the center
                _bounds.X = (int)(center.X - _bounds.Width / 2);
            }

            // Handle Y-axis scaling (same logic as X)
            if (scale.Y != 0)
            {
                float deltaY = currentMousePosition.Y - _previousMouseY;  // Track mouse movement on the Y-axis

                bool isMouseMovingUp = deltaY < 0;  // Moving up should increase the height
                bool isMouseMovingDown = deltaY > 0;  // Moving down should decrease the height

                // Flip scaling direction or add to the height
                if (_bounds.Height + (int)scale.Y < 0 || (_fliped && scale.Y < 0))
                {
                    _bounds.Height += Math.Abs((int)scale.Y);  // Add positive scaling
                    _fliped = true;
                }
                else
                {
                    if (isMouseMovingUp && !_fliped)  // If mouse is moving up, scale normally
                    {
                        _bounds.Height += (int)scale.Y;
                    }
                    else if (isMouseMovingDown && _fliped)  // If mouse is moving down, scale down
                    {
                        _bounds.Height -= Math.Abs((int)scale.Y);  // Downscale
                    }
                    else
                    {
                        _bounds.Height += (int)scale.Y;
                    }

                    _fliped = false;
                }

                // Recalculate Y position to maintain the center
                _bounds.Y = (int)(center.Y - _bounds.Height / 2);
            }

            // Store current mouse position for the next frame
            _previousMouseX = currentMousePosition.X;
            _previousMouseY = currentMousePosition.Y;
        }

        public void Rotate(float radians)
        {
            _rotation += radians;
        }
    }
}
