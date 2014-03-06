using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WreckingBallFinal.Classes.GameObjects;
using WreckingBallFinal.Classes.General;

namespace WreckingBallFinal
{
    class BrickTwoTimesDestructable : Brick
    {
        private bool _broken;
        private bool _prevState;

        public BrickTwoTimesDestructable(Game game, Vector2 position, Color? color = null, float scale = 1)
            : base(game, position, color, scale)
        {
            Destructable = false;
            Points       = 500;
        }

        public override bool Collide(bool IsColliding)
        {
            if (!_broken && _prevState)
            {
                Texture = ContentManager.BrickBrokenTexture;
                _broken = true;
            }
            else if (_broken && !_prevState)
            {
                Destructable = true;
            }

            _prevState = IsColliding;

            return Destructable;
        }
    }
}
