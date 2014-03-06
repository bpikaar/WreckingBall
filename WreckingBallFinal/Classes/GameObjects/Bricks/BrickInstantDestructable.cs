using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WreckingBallFinal.Classes.GameObjects;

namespace WreckingBallFinal
{
    class BrickInstantDestructable : Brick
    {
        public BrickInstantDestructable(Game game, Vector2 position, Color? color = null, float scale = 1)
            : base(game, position, color, scale)
        {
            Destructable = true;
            Points       = 100;
        }

        public override bool Collide(bool IsColliding)
        {
            return Destructable;
        }
    }
}
