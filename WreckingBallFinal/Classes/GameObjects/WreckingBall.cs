using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using WreckingBallFinal.Classes.GameObjects;

namespace WreckingBallFinal
{
    class WreckingBall : GameObject
    {
        // Sloopkogel is 250 x 250 pixels (Ware grootte)
        private const int WIDTH_BALL = 250; 

        private float _speed;
        private float _originHeight;

        /// <summary>
        /// De OriginHeight is een berekening op basis van ware grootte
        /// </summary>
        public float OriginHeight
        {
            get { return _originHeight; }
            set {
                if (value <= (Texture.Height - WIDTH_BALL) * Scale) _originHeight = (Texture.Height - WIDTH_BALL) - (value / Scale);
            }
        }

        /// <summary>
        /// Int: Start angle of WreckingBall in degrees. Zero is vertical
        /// </summary>
        public int       Angle       { get; set; }
        public float     ChainLength { get; set; }
        public Stopwatch Timer       { get; set; }

        public Rectangle HitArea
        {
            get
            {            
                // Middelpunt van de kogel bepalen. 
                float lengthChainCentre = (Texture.Height - OriginHeight) * Scale - (Texture.Width / 2) * Scale;
                
                // Vermenigvuldigen met -1 om links- en rechtsom draaien te corrigeren
                double dX = -1 * Math.Sin(Rotation) * lengthChainCentre;
                double dY = Math.Cos(Rotation) * lengthChainCentre;

                return new Rectangle(
                    (int)(Position.X + dX - (Texture.Width / 2) * Scale),
                    (int)(0 + dY - (WIDTH_BALL / 2) * Scale),
                    (int)(Texture.Width * Scale),
                    (int)(WIDTH_BALL * Scale));
            }
        }
        
        public WreckingBall(Game game, Vector2 position, Color color, float scale)
            : base(game, position, color, scale)
        {
            Angle = 0;
        }

        public override void Initialize()
        {
            ChainLength   = 200; // van 1 - 200
            _speed        = 1;
            _originHeight = ChainLength * -5 + 1000;

            Rotation      = 1;
            

            Timer = new Stopwatch();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture       = Game.Content.Load<Texture2D>("wreckingball");
            _originHeight = (Texture.Height - WIDTH_BALL);
            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Zet de lengte van de ketting vast tussen een minimale (1) en maximale (200) waarde
            MathHelper.Clamp(ChainLength, 1, 200);
            
            Origin = new Vector2(Texture.Width / 2, _originHeight);

            /**
            * http://www.myphysicslab.com/pendulum1.html
            * 
            * Hoek op een bepaalde tijd wordt berekend door:
            * Hoek (t) = initiele hoek * cos (wortel(g / lengte touw) * tijd)
            * 
            * De lengte van het touw bepaald hoe snel de sloopkogel zal gaan slingeren
            */
            //Rotation = MathHelper.ToRadians(Angle) * (float)((Math.Cos(Math.Sqrt(9.78 / (Math.Sqrt(ChainLength) / 10)) * gameTime.TotalGameTime.TotalSeconds)));
            
            // Deze berekening werkt met een stopwatch zonder variabele snelheid
            //Rotation = MathHelper.ToRadians(Angle) * (float)((Math.Cos(Math.Sqrt(9.78 / (Math.Sqrt(ChainLength) / 10)) * Timer.Elapsed.TotalSeconds)));

            // Deze berekening werkt met een stopwatch met variabele snelheid
            //                                                                    9         lengte touw op basis van de input OriginHeight              tijd
            Rotation = MathHelper.ToRadians(Angle) * (float)((Math.Cos(Math.Sqrt(9.78 / (Math.Sqrt((Texture.Height / OriginHeight) * 200f) /30f)) * Timer.Elapsed.TotalSeconds)));
            
            // wrijving
            if (Timer.IsRunning)
            {
                Rotation *= _speed;
                _speed *= 0.998f; ;
                if (_speed < 0) _speed = 0;
            }

            base.Update(gameTime);
        }

        public void Reset()
        {
            _speed  = 1;
            Angle   = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.DEBUG)
            {
                Texture2D text = new Texture2D(Game.GraphicsDevice, 1, 1);
                text.SetData(new[] { Color.Yellow });

                Game1.SpriteBatch.Draw(text, HitArea, Color.White);
            }
            base.Draw(gameTime);
        }
    }
}
