using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WreckingBallFinal.Classes.GameObjects
{
    interface ICollidable
    {
        Rectangle Bounds { get; }

        /// <summary>
        /// Handlers the collision for this object
        /// </summary>
        /// <returns>Return true or false if object can be destroyed</returns>
        bool Collide(bool IsColliding);
    }
}
