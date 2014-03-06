using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WreckingBallFinal.Classes.GameObjects;

namespace WreckingBallFinal
{
    class BrickNonDestructable : Brick
    {
        Random _random;

        public BrickNonDestructable(Game game, Vector2 position, Color? color = null, float scale = 1)
            : base(game, position, color, scale)
        {
            Destructable = false;
        }

        public override bool Collide(bool IsColliding)
        {
            return Destructable;
        }
    }
}
