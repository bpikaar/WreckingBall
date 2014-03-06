using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WreckingBallFinal.Classes.GameObjects.Bricks
{
    class BrickManager : GameObject
    {
        private const int AREA_WIDTH  = 570;
        private const int AREA_HEIGHT = 675;

        private const int BRICK_WIDTH  = 57;
        private const int BRICK_HEIGHT = 27;

        private const int NUMBER_OF_BRICKS = AREA_WIDTH / BRICK_WIDTH * AREA_HEIGHT / BRICK_HEIGHT;
        
        private Random _random;
        private float _spawnProbability;

        public float SpawnProbability
        {
            get { return _spawnProbability; }
            set { _spawnProbability = value; }
        }

        public List<Brick> Bricks { get; set; }

        public Rectangle BrickArea {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    AREA_WIDTH,
                    AREA_HEIGHT
                    );
            }
        }

        public BrickManager(Game1 game, Vector2 position)
            : base(game, position, null, 1)
        {
            Texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { Color.Yellow });
        }

        public override void Initialize()
        {
            Bricks              = new List<Brick>();
            _random             = new Random();
            _spawnProbability   = 0.2f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_random.NextDouble() < _spawnProbability && Bricks.Count < NUMBER_OF_BRICKS)
            {
                Brick brick;

                var rand = _random.Next(1, 20);
                
                if  (rand > 16)
                {
                    brick = new BrickTwoTimesDestructable(
                                        Game,
                                        new Vector2(Position.X + _random.Next(0, AREA_WIDTH / BRICK_WIDTH) * BRICK_WIDTH, 0),
                                        Color.AntiqueWhite,
                                        1);
                }
                else if (rand > 12)
                {
                    brick = new BrickInstantDestructable(
                                        Game,
                                        new Vector2(Position.X + _random.Next(0, AREA_WIDTH / BRICK_WIDTH) * BRICK_WIDTH, 0),
                                        Color.IndianRed,
                                        1);
                }
                else 
                {
                    brick = new BrickNonDestructable(
                                        Game,
                                        new Vector2(Position.X + _random.Next(0, AREA_WIDTH / BRICK_WIDTH) * BRICK_WIDTH, 0),
                                        Color.Gray,
                                        1);
                }
                    
                Bricks.Add(brick);
            }

            
            for (int i = Bricks.Count - 1; i >= 0; i--)
            {
                // Raakt een blokje de onderkant van de BrickArea?
                // TODO oplossing maken als de blokjes bovenop de Brickarea gestapeld worden.
                if (!BrickArea.Contains(Bricks[i].Bounds))
                {
                    Bricks[i].Position = new Vector2(Bricks[i].Position.X, AREA_HEIGHT - BRICK_HEIGHT);
                    Bricks[i].Moveable = false;
                }

                // Als een blokje bovenop een ander blokje valt moet deze op de plek net boven het blokje worden geplaatst.
                for (int collBrick = Bricks.Count - 1; collBrick >= 0; collBrick--)
                {
                    if (Bricks[i].Bounds.Intersects(Bricks[collBrick].Bounds) && Bricks[i].Position.Y < Bricks[collBrick].Position.Y)
                    {
                        Bricks[i].Position = new Vector2(Bricks[i].Position.X, Bricks[collBrick].Position.Y - BRICK_HEIGHT - 2);
                        // Als het blokje bovenop gestapeld wordt en dus buiten de brickArea valt moet deze helemaal weggehaald worden
                        // Deze kan dan opnieuw gespawnd gaan worden. 
                        if (Bricks[i].Position.Y < 0)
                        {
                            Bricks[i].Dispose();
                            Bricks[i] = null;
                            Bricks.Remove(Bricks[i]);
                        }
                        break;
                    }
                    
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.DEBUG)
            {
                Texture2D text = new Texture2D(Game.GraphicsDevice, 1, 1);
                text.SetData(new[] { Color.Yellow });

                Game1.SpriteBatch.Draw(text, BrickArea, Color.White);
            }
            base.Draw(gameTime);
        }
    }
}
