using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Joystick_Simulator
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        TimeSpan vibTime;
        TimeSpan elaspedVibTime;
        TimeSpan spawnTimer;
        TimeSpan spawnBackTimer;
        TimeSpan elaspedGameTime;
        TimeSpan elaspedGameTimer;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite ship;
        Sprite shield;
        Sprite shieldNum;
        Sprite background;
        Sprite pausedScreen;
        Sprite endScreen;
        Vector2 speed;
        Vector2 tempSpeed;
        GamePadState pks;

        int spawnTime = 700;
        int spawnBackTime = 700;

        int score;
        int armor = 3;
        
        bool destroyed = false;
        bool debug = false;

        Texture2D[] randTextures;
        Texture2D[] randBackTextures;
        //Texture2D[] armorUp;

        Random random;

        List<Bullets> bullets;
        List<Asteroid> asteroids;
        List<Asteroid> backAsteroids;
        int rumbleTime = 120;
        int mag = 14;

        bool shotVib = false;
        bool paused = false;
        bool adminMode = false;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1300;  //Width 1300
            graphics.PreferredBackBufferHeight = 800; //Height 800
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // List<Bullets> bullets = new List<Bullets>();

            ship = new Sprite((Content.Load<Texture2D>("playerShip1_orange")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
            background = new Sprite((Content.Load<Texture2D>("blackish2")), new Vector2(0, 0), Color.White, 0, 0);
            pausedScreen = new Sprite((Content.Load<Texture2D>("pausedScreen")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
            endScreen = new Sprite((Content.Load<Texture2D>("endScreen")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
            speed = new Vector2(0, 0);
            tempSpeed = new Vector2(7.5f, 7.5f);
            font = Content.Load<SpriteFont>("Font");

            vibTime = TimeSpan.FromMilliseconds(rumbleTime);
            elaspedVibTime = new TimeSpan();

            randTextures = new Texture2D[] {
                Content.Load<Texture2D>("meteorBrown_big1"),
                Content.Load<Texture2D>("meteorBrown_big2"),
                Content.Load<Texture2D>("meteorBrown_big3"),
                Content.Load<Texture2D>("meteorBrown_med3"),
                Content.Load<Texture2D>("meteorBrown_big3"),
                Content.Load<Texture2D>("meteorBrown_big1"),
                Content.Load<Texture2D>("meteorBrown_big2"),
                Content.Load<Texture2D>("meteorBrown_big3"),
                //Content.Load<Texture2D>("spaceMeteors_001")
            };

            randBackTextures = new Texture2D[] {
                Content.Load<Texture2D>("Big1"),
                Content.Load<Texture2D>("Med1"),
                Content.Load<Texture2D>("Med2"),
                Content.Load<Texture2D>("Small1"),
                Content.Load<Texture2D>("Small2")
            };
            //armorUp = new Texture2D[] { Content.Load<Texture2D>("fullarmor"), Content.Load<Texture2D>("twoarmor"), Content.Load<Texture2D>("onearmor") };
            //shield = new Sprite((Content.Load<Texture2D>("shield3")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);

            random = new Random();
            spawnTimer = TimeSpan.FromMilliseconds(spawnTime);
            spawnBackTimer = TimeSpan.FromMilliseconds(spawnBackTime);
            bullets = new List<Bullets>();
            asteroids = new List<Asteroid>();
            backAsteroids = new List<Asteroid>();
            base.Initialize();

        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        GamePadState xBone;
        float magnitude;
        protected override void Update(GameTime gameTime)
        {
            xBone = GamePad.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            elaspedGameTime += gameTime.ElapsedGameTime;
            elaspedGameTimer += gameTime.ElapsedGameTime;

            if (destroyed == true && xBone.Buttons.B == ButtonState.Pressed && pks.Buttons.B == ButtonState.Released)
            {                
                asteroids.Clear();
                bullets.Clear();
                ship.Position.X = GraphicsDevice.Viewport.Width / 2;
                ship.Position.Y = GraphicsDevice.Viewport.Height / 2;
                armor = 3;
                score = 0;
                spawnTime = 700;
                destroyed = !destroyed;
            }

            if (xBone.Buttons.Start == ButtonState.Pressed && pks.Buttons.Start == ButtonState.Released && destroyed != true)
            {
                paused = !paused;
            }

            if (xBone.Buttons.Start == ButtonState.Pressed && pks.Buttons.Start == ButtonState.Released && destroyed == true)
            {
                Exit();
            }


            if (armor == 3)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield3")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("threearmor")), new Vector2(60, 20), Color.White, 0, 0);
            }
            if(armor == 2)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield2")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("twoarmor")), new Vector2(60, 20), Color.White, 0, 0);
            }
            if(armor == 1)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield1")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("onearmor")), new Vector2(60, 20), Color.White, 0, 0);
            }
            if(armor < 1)
            {
                shieldNum = new Sprite((Content.Load<Texture2D>("noarmor")), new Vector2(60, 20), Color.White, 0, 0);
            }
            if(adminMode == true)
            {
                armor = 3;
            }


            if (ship.Position.Y < ship.Texture.Width / 2)
            {
                ship.Position.Y = ship.Texture.Width / 2;
            }
            if (ship.Position.X < ship.Texture.Width / 2)
            {
                ship.Position.X = ship.Texture.Width / 2;
            }
            if(ship.Position.X + ship.Texture.Width / 2 > GraphicsDevice.Viewport.Width)
            {
                ship.Position.X = GraphicsDevice.Viewport.Width - ship.Texture.Width / 2;
            }
            if (ship.Position.Y + ship.Texture.Height / 2 > GraphicsDevice.Viewport.Height)
            {
                ship.Position.Y = GraphicsDevice.Viewport.Height - ship.Texture.Height / 2;
            }

            if (paused == false && destroyed != true)
            {
                if (xBone.Buttons.RightShoulder == ButtonState.Pressed && pks.Buttons.RightShoulder == ButtonState.Released  && destroyed == false && adminMode == false)
                {
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(ship.Position.X, ship.Position.Y), new Vector2(0, 0)));
                    shotVib = true;
                    elaspedVibTime = new TimeSpan();
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    bullets[bullets.Count - 1].rotation = ship.rotation;
                    bullets[bullets.Count - 1].velocity = new Vector2((float)Math.Cos(ship.rotation - MathHelper.PiOver2), (float)Math.Sin(ship.rotation - MathHelper.PiOver2)) * mag;
                }

                if (xBone.Buttons.RightShoulder == ButtonState.Pressed && destroyed == false && adminMode == true)
                {
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(ship.Position.X, ship.Position.Y), new Vector2(0, 0)));
                    shotVib = true;
                    elaspedVibTime = new TimeSpan();
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    bullets[bullets.Count - 1].rotation = ship.rotation;
                    bullets[bullets.Count - 1].velocity = new Vector2((float)Math.Cos(ship.rotation - MathHelper.PiOver2), (float)Math.Sin(ship.rotation - MathHelper.PiOver2)) * mag;
                }

                if (elaspedGameTime >= spawnTimer)
                {
                    Texture2D randImg = randTextures[random.Next(0, randTextures.Length)];
                    asteroids.Add(new Asteroid(randImg, random.Next(3,6) , GraphicsDevice.Viewport, ship.Position));
                    elaspedGameTime = new TimeSpan();
                }
                if (elaspedGameTimer >= spawnBackTimer)
                {
                    Texture2D randBackImg = randBackTextures[random.Next(0, randBackTextures.Length)];
                    backAsteroids.Add(new Asteroid(randBackImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position));
                    elaspedGameTimer = new TimeSpan();
                }

                elaspedVibTime += gameTime.ElapsedGameTime;

                if (elaspedVibTime > vibTime && shotVib)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                    shotVib = false;
                }

                if (xBone.Buttons.RightStick == ButtonState.Pressed && pks.Buttons.RightStick == ButtonState.Released)
                {
                    debug = !debug;
                }

                //Something Something Test Github

                if (xBone.Buttons.X == ButtonState.Pressed && xBone.Buttons.Y == ButtonState.Pressed && pks.Buttons.Y == ButtonState.Released)
                {
                    adminMode = !adminMode;
                }

                magnitude = (float)(Math.Pow(xBone.ThumbSticks.Right.X, 2) + Math.Pow(xBone.ThumbSticks.Right.Y, 2));

                if (magnitude > .85f)
                {
                    ship.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                }

                speed.Y = -xBone.ThumbSticks.Left.Y * tempSpeed.Y;
                speed.X = xBone.ThumbSticks.Left.X * tempSpeed.X;               
                ship.Position += speed;
                shield.Position = ship.Position;
                shield.rotation = ship.rotation;

                foreach (Asteroid asteroid in asteroids)
                {
                    asteroid.Update();
                }
                foreach (Asteroid asteroid in backAsteroids)
                {
                    asteroid.Update();
                }

                UpdateBullets();              
                UpdateAsteroids();
                UpdateSheilds();
            }

            pks = xBone;          
            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            for (int v = 0; v < bullets.Count; v++)
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (bullets.Count >= 0 && asteroids.Count >= 0 && bullets[v].HitBox.Intersects(asteroids[i].HitBox))
                    {

                        asteroids.Remove(asteroids[i]);
                        bullets.Remove(bullets[v]);
                        spawnTime -= 1;
                        score += 100;
                        i--;                     
                        v--;
                        spawnTimer = TimeSpan.FromMilliseconds(spawnTime);
                        //bullets[v].Position += bullets[v].velocity;
                        break;
                    }

                    if(bullets[v].HitBox.Left < 0 ||
                       bullets[v].HitBox.Bottom > GraphicsDevice.Viewport.Height ||
                       bullets[v].HitBox.Top < 0 ||
                       bullets[v].HitBox.Right > GraphicsDevice.Viewport.Width )
                    {
                        bullets.RemoveAt(v);
                        v--;
                        break;
                    }
         
                }   

                if(v >= 0)
                {
                    bullets[v].Update();
                }
            }          
        }

        public void UpdateAsteroids()
        {
            for(int u = 0; u < asteroids.Count; u++)
            {

                if (asteroids[u].HitBox.Right < -asteroids[u].HitBox.Width * 2 ||
                    asteroids[u].HitBox.Left > GraphicsDevice.Viewport.Width + asteroids[u].HitBox.Width * 2 ||
                    asteroids[u].HitBox.Top < -asteroids[u].HitBox.Height * 2||
                    asteroids[u].HitBox.Bottom > GraphicsDevice.Viewport.Height + asteroids[u].HitBox.Height * 2)
                {
                    asteroids.RemoveAt(u);
                    u--;
                    continue;
                }

                if (armor == 0 && asteroids[u].HitBox.Intersects(ship.HitBox))
                {
                    destroyed = true;
                }
            }

            
        }

        public void UpdateSheilds()
        {
            for (int y = 0; y < asteroids.Count; y++)
            {
                if(ship.HitBox.Intersects(asteroids[y].HitBox) && armor != 0)
                {
                    asteroids.Remove(asteroids[y]);                   
                    armor--;
                    y--;
                }
                else
                { }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            background.Draw(spriteBatch);

            float x = xBone.ThumbSticks.Right.X;
            float y = xBone.ThumbSticks.Right.Y;
            float v = xBone.ThumbSticks.Left.X;
            float w = xBone.ThumbSticks.Left.Y;

            if (debug == true)
            {
                spriteBatch.DrawString(font, $"X:{x} Y:{y}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 40), Color.White);
                spriteBatch.DrawString(font, $"Magnitude:{magnitude}", new Vector2(5, GraphicsDevice.Viewport.Height / 2), Color.White);
                spriteBatch.DrawString(font, $"X:{v} Y:{w}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                spriteBatch.DrawString(font, $"ship position: ({ship.Position.X},{ship.Position.Y})", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 20), Color.White);
                spriteBatch.DrawString(font, $"Spawner: {spawnTime}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 60, GraphicsDevice.Viewport.Height / 2 - 400), Color.Wheat);
                spriteBatch.DrawString(font, $"rotation \n {ship.rotation}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 60), Color.Wheat);
                spriteBatch.DrawString(font, $"asteroid count \n {asteroids.Count}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 100), Color.Wheat);
                spriteBatch.DrawString(font, $"bullet count \n {bullets.Count}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 140), Color.Wheat);
            }

            spriteBatch.DrawString(font, $"SCORE: {score}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 60 , GraphicsDevice.Viewport.Height / 2 - 360), Color.White);

            if(adminMode == true)
            {
                spriteBatch.DrawString(font, $"Admin Mode Enabled", new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 385), Color.White);
            }

            foreach (Asteroid asteroid in backAsteroids)
            { asteroid.Draw(spriteBatch); }

            foreach (Bullets bullet in bullets)
            { bullet.Draw(spriteBatch); }

            foreach (Asteroid asteroid in asteroids)
            { asteroid.Draw(spriteBatch); }

            if (armor >= 1)
            { shield.Draw(spriteBatch); }
            else { }

            shieldNum.Draw(spriteBatch);

            if(destroyed == false)
            {
                ship.Draw(spriteBatch);
            }
            else
            {
                endScreen.Draw(spriteBatch);
            }

            if (paused == true && destroyed == false)
            {
                pausedScreen.Draw(spriteBatch); // Paused
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
