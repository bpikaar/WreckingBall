using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WreckingBallFinal.Classes.General;

namespace WreckingBallFinal.Classes.GameObjects
{
    abstract class Brick : GameObject, ICollidable
    {
        protected float _speed = 3;

        public bool Moveable     { get; set; }
        public bool Destructable { get; set; }
        public int  Points       { get; set; }

        public Rectangle Bounds
        {
            get 
            { 
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Texture.Width,
                    Texture.Height
                    ); 
            }
        }

        public Brick(Game game, Vector2 position, Color? color = null, float scale = 1)
            : base(game, position, color, scale)
        {
            Position = position;
            Moveable = true;
            Texture  = ContentManager.BrickTexture;
        }

        public override void Update(GameTime gameTime)
        {
            if (Moveable) Position = new Vector2(Position.X, Position.Y + _speed);

            base.Update(gameTime);
        }

        public abstract bool Collide(bool IsColliding);
    }
}
