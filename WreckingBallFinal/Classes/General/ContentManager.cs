using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WreckingBallFinal.Classes.General
{
    public static class ContentManager
    {
        public static Texture2D BrickTexture;
        public static Texture2D BrickBrokenTexture;

        public static SpriteFont Verdana;

        public static void LoadContent(Game game)
        {
            BrickTexture        = game.Content.Load<Texture2D>("Brick");
            BrickBrokenTexture  = game.Content.Load<Texture2D>("BrickBroken");
            Verdana             = game.Content.Load<SpriteFont>("Verdana");
        }
    }
}
