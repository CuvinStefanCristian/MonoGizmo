using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.DataStructures;
using MonoGizmo.Enums;
using MonoGizmo.Gizmos;
using MonoGizmo.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGizmo
{
    public sealed class GizmoManager
    {
        public GizmoOptions Options;
        public TransformType TransformType { get; set; } = TransformType.World;

        ShapeBatch _shapeBatch;
        GizmoBase[] _gizmos;

        bool _canSelect = false;
        ISelectable _selected;
        MouseState _previousMouseState;

        QuadTree _quadTree;

        IEnumerable<ISelectable> _selectables;
        public IEnumerable<ISelectable> Selectables 
        {
            get => _selectables;
            set
            {
                if (_selectables == value)
                    return;

                _selectables = value;
                CheckCollection();
            }
        }

        public GizmoManager(GraphicsDevice graphicsDevice, GizmoOptions gizmoOptions = null)
        {
            _shapeBatch = new ShapeBatch(graphicsDevice);

            gizmoOptions ??= new GizmoOptions();
            Options = gizmoOptions;

            _gizmos = new GizmoBase[]
            {
                new OriginGizmo(this),
                new TranslateGizmo(this),
                new RotationGizmo(this),
                new ScaleGizmo(this),
            };

            _quadTree = new QuadTree(0, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
        }

        public void Update(MouseState mouseState, GameTime gameTime)
        {
            foreach (var gizmo in _gizmos)
            {
                if (gizmo.IsActive && gizmo.IsAttached)
                    gizmo.Update(mouseState, gameTime);
            }

            _quadTree.Clear();
            foreach (var selectable in Selectables)
                _quadTree.Insert(selectable);

            if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                if (_selected != null && _selected.IsSelected(mouseState.Position))
                    return;

                List<ISelectable> potentialSelectables = new List<ISelectable>();
                _quadTree.Retrieve(potentialSelectables, mouseState.Position.ToVector2());

                foreach (var selectable in potentialSelectables)
                {
                    if (selectable.IsSelected(mouseState.Position) && !_canSelect)
                    {
                        _selected = selectable;

                        foreach (var gizmo in _gizmos)
                            if (gizmo.IsActive)
                                gizmo.Attach(selectable);
                        
                        break;
                    }
                }
            }

            _previousMouseState = mouseState;
        }

        public void Draw(GameTime gameTime)
        {
            _shapeBatch.Begin();

            foreach (var gizmo in _gizmos)
            {
                if (gizmo.IsActive && gizmo.IsAttached)
                    gizmo.Draw(_shapeBatch, gameTime);
            }

            _shapeBatch.End();
        }

        public void ActivateType(GizmoType gizmoType)
        {
            foreach (var gizmo in _gizmos)
                if (gizmo.GizmoType == gizmoType)
                {
                    gizmo.IsActive = true;

                    if (_selected != null)
                        gizmo.Attach(_selected);
                }
        }

        public void DeactivateType(GizmoType gizmoType) 
        {
            foreach (var gizmo in _gizmos)
                if (gizmo.GizmoType == gizmoType)
                {
                    gizmo.IsActive = false;

                    if (_selected != null)
                        gizmo.Detach();
                }
        }

        public bool IsTypeActive(GizmoType gizmoType)
        {
            foreach (var gizmo in _gizmos)
                if (gizmo.GizmoType == gizmoType)
                    return gizmo.IsActive;
            return false;
        }

        public void ClearSelection()
        {
            foreach(var gizmo in _gizmos)
                if (gizmo.IsActive)
                    gizmo.Detach();
        }

        internal void CheckCollection()
        {
            if (_selected == null)
                return;

            bool contains = false;
            foreach (var selectable in Selectables)
            {
                if (selectable == _selected)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                ClearSelection();
                _selected = null;
            }
        }

        internal void DisableAll()
        {
            foreach (var gizmo in _gizmos)
                if (!gizmo.IsToggled)
                    gizmo.Detach();

            _canSelect = true;
        }

        internal void EnableAll()
        {
            foreach (var gizmo in _gizmos.Where(g => g.IsActive))
                if (!gizmo.IsToggled)
                    gizmo.Attach(_selected);

            _canSelect = false;
        }

        //internal void DisableAll()
        //{
        //    foreach (var gizmo in _gizmos)
        //        if (!gizmo.IsToggled)
        //            DeactivateType(gizmo.GizmoType);

        //    _canSelect = true;
        //}

        //internal void EnableAll()
        //{
        //    foreach (var gizmo in _gizmos.Where(g => g.IsActive))
        //        ActivateType(gizmo.GizmoType);

        //    _canSelect = false;
        //}
    }
}
