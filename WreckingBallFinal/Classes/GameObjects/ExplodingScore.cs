using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WreckingBallFinal.Classes.General;

namespace WreckingBallFinal.Classes.GameObjects
{
    class ExplodingScore : GameObject
    {
        private int _score;

        public ExplodingScore(Game game, Vector2 position, int score, Color color, float scale)
            : base(game, position, color, scale)
        {
            _score = score;
        }

        public override void Update(GameTime gameTime)
        {
            Scale *= 1.06f;

            if (Scale > 20) Dispose();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.SpriteBatch.DrawString(ContentManager.Verdana, _score.ToString(), new Vector2(Position.X + 2, Position.Y + 2), Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
            Game1.SpriteBatch.DrawString(ContentManager.Verdana, _score.ToString(), Position, Color, Rotation, Origin, Scale, SpriteEffects.None, 0);

        }
    }
}
