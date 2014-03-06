#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
using WreckingBallFinal.Classes.GameObjects;
using WreckingBallFinal.Classes.GameObjects.Bricks;
#endregion

namespace WreckingBallFinal
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public const bool DEBUG = false;

        public static SpriteBatch SpriteBatch;

        private GraphicsDeviceManager graphics;
        
        private MouseHandler mouse;
        private WreckingBall wreckingBall;
        private BrickManager brickManager;

        private bool drag;

        private Texture2D   _bg;
        private SpriteFont  _verdanaFont;
        private int         _score;
        private int         _turn;
        private KeyboardState prevState;
        private SaveGameData highScore;
        
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true; ;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            //Changes the settings that you just applied
            graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            wreckingBall    = new WreckingBall(this, new Vector2(500, 0), Color.White, 0.5f);
            mouse           = new MouseHandler(this);
            brickManager    = new BrickManager(this, new Vector2(600, 0));
            _turn           = 1;

            HighScoreManager hsm = new HighScoreManager(this);
            highScore = hsm.ReadData();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            WreckingBallFinal.Classes.General.ContentManager.LoadContent(this);

            _verdanaFont = WreckingBallFinal.Classes.General.ContentManager.Verdana;
            _bg          = Content.Load<Texture2D>("bgWreckingball");
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (mouse.IsMouseDown)
            {
                if (wreckingBall.HitArea.Contains(mouse.MouseState.X, mouse.MouseState.Y))
                {
                    drag = true;
                }
            }
            if (!mouse.IsMouseDown)
                drag = false;

            if (drag)
            {
                wreckingBall.OriginHeight = mouse.MouseState.Y;
                //wreckingBall.Angle = -(int)((Math.Atan2(mouse.MouseState.Y, wreckingBall.Position.X - mouse.MouseState.X) * (180 / Math.PI)) - 90);
            }

            KeyboardState kbs = Keyboard.GetState();

            if (kbs.IsKeyDown(Keys.S) && prevState.IsKeyUp(Keys.S))
            {
                wreckingBall.Angle = 90;
                wreckingBall.Timer.Start();
            }

            if (kbs.IsKeyDown(Keys.R) && prevState.IsKeyUp(Keys.R))
            {
                _turn++;
                if (_turn < 4)
                {
                    wreckingBall.Timer.Reset();
                    wreckingBall.Reset();
                }
                else
                {
                     _turn = 3;
                    if (_score > highScore.Score)
                    {
                        HighScoreManager hsm = new HighScoreManager(this);
                        hsm.SaveData(new SaveGameData() { Score = _score });
                    }
                }
            }

            prevState = kbs;

            for (int i = brickManager.Bricks.Count - 1; i >= 0; i--)
            {
                // Raakt de wrecking ball een blokje dan moet het blokje verwijderd worden.
                if (wreckingBall.HitArea.Intersects(brickManager.Bricks[i].Bounds))
                {
                    // Als het blokje Destructable is moet het blokje opgeruimd worden
                    if (brickManager.Bricks[i].Collide(true))
                    {
                        _score += brickManager.Bricks[i].Points;

                        ExplodingScore explodeScore = new ExplodingScore(
                                                                        this, 
                                                                        brickManager.Bricks[i].Position, 
                                                                        brickManager.Bricks[i].Points, 
                                                                        brickManager.Bricks[i].Color, 1);

                        brickManager.Bricks[i].Dispose();
                        brickManager.Bricks[i] = null;
                        brickManager.Bricks.Remove(brickManager.Bricks[i]);
                    }
                    // TODO anders moet de wrecking ball vanaf hier terugkaatsen. 
                    else
                    {

                    }
                }
                else
                {
                    brickManager.Bricks[i].Collide(false);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            string text = "Sleep met de muis de sloopkogel op\nde juiste hoogte\n" +
                          "Met de \"S\" start de kogel met\nslingeren\n" +
                          "Met de \"R\" gaat de kogel terug\nnaar het midden";

            SpriteBatch.Begin();
            SpriteBatch.Draw(_bg, new Vector2(0, 0), Color.White);

            SpriteBatch.DrawString(_verdanaFont, text, new Vector2(32, 32), Color.DarkGray);
            SpriteBatch.DrawString(_verdanaFont, text, new Vector2(30, 30), Color.White);

            SpriteBatch.DrawString(_verdanaFont, "Beurt: "   + _turn, new Vector2(31, 181), Color.DarkGray);
            SpriteBatch.DrawString(_verdanaFont, "Beurt: "   + _turn, new Vector2(30, 180), Color.White);
            
            SpriteBatch.DrawString(_verdanaFont, "Punten: "  + _score, new Vector2(31, 201), Color.WhiteSmoke);
            SpriteBatch.DrawString(_verdanaFont, "Punten: "  + _score, new Vector2(30, 200), Color.DarkOliveGreen);

            SpriteBatch.DrawString(_verdanaFont, "HighScore: " + highScore.Score, new Vector2(31, 251), Color.WhiteSmoke);
            SpriteBatch.DrawString(_verdanaFont, "HighScore: " + highScore.Score, new Vector2(30, 250), Color.BlueViolet);

            base.Draw(gameTime);
            SpriteBatch.End();
        }
    }
}
