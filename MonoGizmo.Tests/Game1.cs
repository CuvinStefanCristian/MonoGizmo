using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGizmo.Abstraction;
using MonoGizmo.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MonoGizmo.Tests
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ShapeBatch _shapeBatch;

        GizmoManager _gizmoManager;
        List<Selectable> _selectables;
        bool _keyDown = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var s1 = new Selectable(new Rectangle(20, 30, 200, 100), Vector2.One, 0);
            var s2 = new Selectable(new Rectangle(50, 98, 100, 100), Vector2.One, 0);
            _selectables = [s1, s2];
            _gizmoManager = new GizmoManager(GraphicsDevice, Content)
            {
                Selectables = _selectables.Where(x => x.GetType() == typeof(Selectable)).Select(x => (ISelectable)x),
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _shapeBatch = new ShapeBatch(GraphicsDevice, Content);

            _gizmoManager.ActivateType(GizmoType.Origin);
            _gizmoManager.ActivateType(GizmoType.Translate);
            _gizmoManager.ActivateType(GizmoType.Rotate);
            _gizmoManager.ActivateType(GizmoType.Scale);

            _selectables.Add(new Selectable(new Rectangle(230, 100, 150, 130), new Vector2(1, 1), 0f));
        }

        bool test = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _gizmoManager.ClearSelection();

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                _gizmoManager.TransformType = MonoGizmo.Enums.TransformType.Local;
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                _gizmoManager.TransformType = MonoGizmo.Enums.TransformType.World;

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.NumPad0) && !_keyDown)
            {
                if (_gizmoManager.IsTypeActive(GizmoType.Origin))
                    _gizmoManager.DeactivateType(GizmoType.Origin);
                else
                    _gizmoManager.ActivateType(GizmoType.Origin);

                _keyDown = true;
            }
            if (keyboardState.IsKeyDown(Keys.NumPad1) && !_keyDown)
            {
                if (_gizmoManager.IsTypeActive(GizmoType.Translate))
                    _gizmoManager.DeactivateType(GizmoType.Translate);
                else
                    _gizmoManager.ActivateType(GizmoType.Translate);

                _keyDown = true;
            }
            if (keyboardState.IsKeyDown(Keys.NumPad2) && !_keyDown)
            {
                if (_gizmoManager.IsTypeActive(GizmoType.Rotate))
                    _gizmoManager.DeactivateType(GizmoType.Rotate);
                else
                    _gizmoManager.ActivateType(GizmoType.Rotate);

                _keyDown = true;
            }
            if(keyboardState.IsKeyDown(Keys.NumPad3) && !_keyDown)
            {
                if (_gizmoManager.IsTypeActive(GizmoType.Scale))
                    _gizmoManager.DeactivateType(GizmoType.Scale);
                else
                    _gizmoManager.ActivateType(GizmoType.Scale);
                _keyDown = true;
            }


            if (keyboardState.IsKeyUp(Keys.NumPad1) && keyboardState.IsKeyUp(Keys.NumPad2) && keyboardState.IsKeyUp(Keys.NumPad3))
            {
                _keyDown = false;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
                foreach (var selectable in _selectables)
                    selectable.Center -= new Vector2(1, 0);
            if (keyboardState.IsKeyDown(Keys.Right))
                foreach (var selectable in _selectables)
                    selectable.Center += new Vector2(1, 0);
            if (keyboardState.IsKeyDown(Keys.Up))
                foreach (var selectable in _selectables)
                    selectable.Center -= new Vector2(0, 1);
            if (keyboardState.IsKeyDown(Keys.Down))
                foreach (var selectable in _selectables)
                    selectable.Center += new Vector2(0, 1);

            _gizmoManager.Update(Mouse.GetState(), gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _shapeBatch.Begin();

            foreach (var selectable in _selectables)
            {
                _shapeBatch.DrawRectangle(selectable.Bounds.Location.ToVector2(), selectable.Bounds.Size.ToVector2(),
                    Color.Green, Color.Transparent, 4, 0, selectable.Rotation);
            }

            _shapeBatch.End();

            _gizmoManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
