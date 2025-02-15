using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Enums;
using MonoGizmo.Graphics;

namespace MonoGizmo.Abstraction
{
    internal abstract class GizmoBase
    {
        protected ISelectable SelectedItem;
        protected GizmoManager GizmoManager;

        public bool IsActive = false, IsAttached = false;
        public GizmoType GizmoType { get; protected set; }

        bool _isToggled = false;
        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if(_isToggled != value)
                {
                    _isToggled = value;

                    if (value) GizmoManager.DisableAll();
                    else GizmoManager.EnableAll();
                }
            }
        }

        protected GizmoBase(GizmoManager gizmoManager)
        {
            GizmoManager = gizmoManager;
        }

        public virtual void Attach(ISelectable selectable)
        {
            SelectedItem = selectable;
            IsAttached = true;
        }

        public void Detach()
        {
            SelectedItem = null;
            IsAttached = false;
            IsToggled = false;
        }

        public abstract void Update(MouseState mouseState, GameTime gameTime);
        public abstract void Draw(ShapeBatch shapeBatch, GameTime gameTime);
    }
}
