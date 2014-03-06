using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WreckingBallFinal
{
    class MouseHandler : GameObject
    {
        private Texture2D   _mouseUp;
        private Texture2D   _mouseDown;

        public bool         IsMouseDown { get; set; }
        public MouseState   MouseState  { get; set; }

        public MouseHandler(Game game)
            : base(game, new Vector2(0,0))
        {
        }

        public override void Initialize()
        {
            Rotation = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _mouseUp    = Game.Content.Load<Texture2D>("Hand");
            _mouseDown  = Game.Content.Load<Texture2D>("Grab");

            Texture     = _mouseUp;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();

            if (MouseState.LeftButton == ButtonState.Released)
            {
                Texture = _mouseUp;
                IsMouseDown = false;
            }
            else
            {
                Texture = _mouseDown;
                IsMouseDown = true;
            }

            Position = new Vector2(MouseState.X - Texture.Width / 2, MouseState.Y - Texture.Height / 2);
 
            base.Update(gameTime);
        }

        
    }
}
