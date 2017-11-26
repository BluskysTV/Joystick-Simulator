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

        public enum GameState
        {
            Paused,
            Playing,
            End,
            Settings,
            MainMenu
        }

        GameState CurrentState = GameState.Settings;

        TimeSpan vibTime;
        TimeSpan elaspedVibTime;

        TimeSpan spawnTimerSmall;
        TimeSpan spawnTimerBig;
        TimeSpan spawnBackTimer;
        TimeSpan spawnPowerTimer;
        TimeSpan spawnMirageTimer;

        TimeSpan multiTimer;

        TimeSpan elaspedGameTimeSmall;
        TimeSpan elaspedGameTimeBig;
        TimeSpan elaspedGameTimer;
        TimeSpan elaspedPowerTimer;
        TimeSpan elaspedMirageTimer;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite ship;
        Sprite arrows;

        Sprite easy;
        Sprite medium;
        Sprite hard;

        Sprite shipMirror1;
        Sprite shipMirror2;
        Sprite shipMirror3;
        Sprite shipMirror4;
        Sprite shield;
        Sprite shieldNum;
        Sprite background;

        Sprite start;
        Sprite settings;
        Sprite quit;
        Sprite pausedToMain;
        
        
        Sprite pausedScreen;
        Sprite menuScreen;

        Sprite endScreen;
        Vector2 speed;
        Vector2 tempSpeed;
        GamePadState pks;

        public bool mirage = false;

        //int spawnTime = 7000;
        int spawnTimeSmall = 20000;
        int spawnTimeBig = 8000;
        int spawnBackTime = 5000;
        //int spawnPowerTime = 30000;
        int spawnPowerTime = 20000;
        int spawnMirageTime = 30000;

        int score;
        int armor = 3;
        
        bool destroyed = false;
        bool debug = false;

        Texture2D[] randTexturesSmall;
        Texture2D[] randTexturesBig;
        Texture2D[] randBackTextures;
        Texture2D[] randPowerTextures;
        Texture2D[] randMirageTextures;
        Random random;

        List<Bullets> bullets;
        List<Asteroid> asteroidsBig;
        List<Asteroid> asteroidsSmall;
        List<Asteroid> backAsteroids;
        List<Asteroid> powerUp;
        List<Asteroid> mirageFour;
        int rumbleTime = 120;
        int mag = 14;

        int selectionMain = 0;
        int selectionSettingsVertical = 0;
        int selectionSettingsHorizontal = 0;
        int selectionSettingsDifficulty;
        int selectionPaused = 1;

        int difficulty = 1;

        bool shotVib = false;
        bool adminMode = false;

        SpriteFont font;
        SpriteFont biggerfont;

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
            shipMirror1 = new Sprite((Content.Load<Texture2D>("drone")), new Vector2(ship.Position.X - 110, ship.Position.Y), Color.White, 0, 0);
            shipMirror2 = new Sprite((Content.Load<Texture2D>("drone")), new Vector2(ship.Position.X + 110, ship.Position.Y), Color.White, 0, 0);
            shipMirror3 = new Sprite((Content.Load<Texture2D>("drone")), new Vector2(ship.Position.X, ship.Position.Y + 110), Color.White, 0, 0);
            shipMirror4 = new Sprite((Content.Load<Texture2D>("drone")), new Vector2(ship.Position.X, ship.Position.Y - 110), Color.White, 0, 0);

            background = new Sprite((Content.Load<Texture2D>("blackish2")),new Vector2(0, 0),Color.White, 0, 0);
            pausedScreen = new Sprite((Content.Load<Texture2D>("pausedScreen")),new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),Color.White, 0, 0);
            menuScreen = new Sprite((Content.Load<Texture2D>("menuscreen")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
            endScreen = new Sprite((Content.Load<Texture2D>("endScreen")),new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),Color.White, 0, 0);
            arrows = new Sprite((Content.Load<Texture2D>("arrows")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 66), Color.White, 0, 0);
            pausedToMain = new Sprite((Content.Load<Texture2D>("mainButton")), new Vector2(GraphicsDevice.Viewport.Width / 2 - 25, GraphicsDevice.Viewport.Height / 2 + 160), Color.White, 0, 0);

            easy = new Sprite((Content.Load<Texture2D>("easy")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
            medium = new Sprite((Content.Load<Texture2D>("medium")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
            hard = new Sprite((Content.Load<Texture2D>("hard")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);

            font = Content.Load<SpriteFont>("Font"); //Game Font
            biggerfont = Content.Load<SpriteFont>("BiggerFont"); //Game Font
            tempSpeed = new Vector2(7.5f, 7.5f);
            speed = new Vector2(0, 0);

            vibTime = TimeSpan.FromMilliseconds(rumbleTime);
            elaspedVibTime = new TimeSpan();
            multiTimer = TimeSpan.FromSeconds(10); 

            randTexturesBig = new Texture2D[] { //All of the random textures for frontAsteroids
                Content.Load<Texture2D>("meteorBrown_big1"),
                Content.Load<Texture2D>("meteorBrown_big2"),
                Content.Load<Texture2D>("meteorBrown_big3"),
                Content.Load<Texture2D>("meteorBrown_big3"),
                Content.Load<Texture2D>("meteorBrown_big1"),
                Content.Load<Texture2D>("meteorBrown_big2"),
                Content.Load<Texture2D>("meteorBrown_big3")
            };
            randTexturesSmall = new Texture2D[] { //All of the random textures for frontAsteroids               
                Content.Load<Texture2D>("meteorBrown_med3"),
                Content.Load<Texture2D>("meteorBrown_med1"),
            };

            randBackTextures = new Texture2D[] { //All of the random textures for backAsteroids
                Content.Load<Texture2D>("Big1"),
                Content.Load<Texture2D>("Med1"),
                Content.Load<Texture2D>("Med2"),
                Content.Load<Texture2D>("Small1"),
                Content.Load<Texture2D>("Small2")
            };

            randPowerTextures = new Texture2D[] { //All of the random textures for powerUps
                Content.Load<Texture2D>("powerupRed_shield")
            };

            randMirageTextures = new Texture2D[] { //All of the random textures for mirageFour
                Content.Load<Texture2D>("powerupYellow_bolt")
            };

            random = new Random();
            spawnTimerSmall = TimeSpan.FromMilliseconds(spawnTimeSmall); //Spawntimer for frontAsteroids
            spawnTimerBig = TimeSpan.FromMilliseconds(spawnTimeBig); //Spawntimer for frontAsteroids
            spawnBackTimer = TimeSpan.FromMilliseconds(spawnBackTime); //Spawntimer for backAsteroids
            spawnPowerTimer = TimeSpan.FromMilliseconds(spawnPowerTime);
            spawnMirageTimer = TimeSpan.FromMilliseconds(spawnMirageTime);
            bullets = new List<Bullets>();
            asteroidsSmall = new List<Asteroid>(); //Asteroids Class Small
            asteroidsBig = new List<Asteroid>(); //Asteroids Class Big
            backAsteroids = new List<Asteroid>(); //Background Asteroids Class
            powerUp = new List<Asteroid>(); //Power ups
            mirageFour = new List<Asteroid>(); //Mirages

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
            xBone = GamePad.GetState(PlayerIndex.One); //Change this when you plug in a different controller

            elaspedGameTimeSmall += gameTime.ElapsedGameTime;
            elaspedGameTimeBig += gameTime.ElapsedGameTime;
            elaspedGameTimer += gameTime.ElapsedGameTime;
            elaspedPowerTimer += gameTime.ElapsedGameTime;
            elaspedMirageTimer += gameTime.ElapsedGameTime;

            if (mirage && CurrentState != GameState.Paused && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                multiTimer -= gameTime.ElapsedGameTime;
                if(multiTimer <= TimeSpan.Zero)//> TimeSpan.FromSeconds(10))
                {
                    multiTimer = TimeSpan.Zero;
                    mirage = false;
                }
            }

            if(CurrentState == GameState.MainMenu && xBone.DPad.Up == ButtonState.Pressed && pks.DPad.Up == ButtonState.Released)
            {
                selectionMain--;
            }
            if (CurrentState == GameState.MainMenu && xBone.DPad.Down == ButtonState.Pressed && pks.DPad.Down == ButtonState.Released)
            {
                selectionMain++;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Left == ButtonState.Pressed && pks.DPad.Left == ButtonState.Released && selectionSettingsVertical == 1)
            {
                selectionSettingsHorizontal--;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Right == ButtonState.Pressed && pks.DPad.Right == ButtonState.Released && selectionSettingsVertical == 1)
            {
                selectionSettingsHorizontal++;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Up == ButtonState.Pressed && pks.DPad.Up == ButtonState.Released)
            {
                selectionSettingsVertical--;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Down == ButtonState.Pressed && pks.DPad.Down == ButtonState.Released)
            {
                selectionSettingsVertical++;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Right == ButtonState.Pressed && pks.DPad.Right == ButtonState.Released && selectionSettingsVertical == 0)
            {
                selectionSettingsDifficulty++;
            }
            if (CurrentState == GameState.Settings && xBone.DPad.Left == ButtonState.Pressed && pks.DPad.Left == ButtonState.Released && selectionSettingsVertical == 0)
            {
                selectionSettingsDifficulty--;
            }
            if (CurrentState == GameState.Paused && xBone.DPad.Up == ButtonState.Pressed && pks.DPad.Up == ButtonState.Released)
            {
                selectionPaused--;
            }
            if (CurrentState == GameState.Paused && xBone.DPad.Down == ButtonState.Pressed && pks.DPad.Down == ButtonState.Released)
            {
                selectionPaused++;
            }
            if (CurrentState == GameState.MainMenu && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionMain == 1)
            {
                CurrentState = GameState.Playing;
            }
            if (CurrentState == GameState.MainMenu && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionMain == 2)
            {
                CurrentState = GameState.Settings;
            }
            if (CurrentState == GameState.MainMenu && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionMain == 3)
            {
                Exit();
            }
            if (CurrentState == GameState.Settings && xBone.Buttons.B == ButtonState.Pressed && pks.Buttons.B == ButtonState.Released)
            {
                CurrentState = GameState.MainMenu;
            }
            if (CurrentState == GameState.Paused && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionPaused == 2)
            {
                CurrentState = GameState.MainMenu;
            }



            if (selectionMain > 3)
            {
                selectionMain--;
            }
            if(selectionMain < 1)
            {
                selectionMain++;
            }
            if(selectionSettingsHorizontal < 0)
            {
                selectionSettingsHorizontal++;
            }
            if(selectionSettingsHorizontal > 5)
            {
                selectionSettingsHorizontal--;
            }
            if (selectionSettingsVertical < 0)
            {
                selectionSettingsVertical++;
            }
            if (selectionSettingsVertical > 2)
            {
                selectionSettingsVertical--;
            }

            if (selectionSettingsDifficulty < 1)
            {
                selectionSettingsDifficulty++;
            }
            if (selectionSettingsDifficulty > 3)
            {
                selectionSettingsDifficulty--;
            }

            if (CurrentState == GameState.Settings && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionSettingsVertical == 0 && selectionSettingsDifficulty == 1)
            {
                difficulty = 1; //Easy
            }
            if(CurrentState == GameState.Settings && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionSettingsVertical == 0 && selectionSettingsDifficulty == 2)
            {
                difficulty = 2; //Medium
            }
            if (CurrentState == GameState.Settings && xBone.Buttons.A == ButtonState.Pressed && pks.Buttons.A == ButtonState.Released && selectionSettingsVertical == 0 && selectionSettingsDifficulty == 3)
            {
                difficulty = 3; //Hard
            }



            if (CurrentState == GameState.End && xBone.Buttons.B == ButtonState.Pressed && pks.Buttons.B == ButtonState.Released || CurrentState == GameState.MainMenu) //If you die, reset everything
            {                
                asteroidsSmall.Clear();
                asteroidsBig.Clear();
                backAsteroids.Clear();
                bullets.Clear();
                powerUp.Clear();
                mirageFour.Clear();
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                ship.Position.X = GraphicsDevice.Viewport.Width / 2;
                ship.Position.Y = GraphicsDevice.Viewport.Height / 2;
                armor = 3;
                score = 0;
                spawnTimeSmall = 20000;
                spawnTimeBig = 8000;
                spawnBackTime = 700;
            }

            if (xBone.Buttons.Start == ButtonState.Pressed && pks.Buttons.Start == ButtonState.Released && CurrentState != GameState.End) //Pause
            {           
                if (CurrentState == GameState.Playing && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
                {CurrentState = GameState.Paused;}
                else if (CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
                {CurrentState = GameState.Playing;}
            }

            if (xBone.Buttons.Start == ButtonState.Pressed && pks.Buttons.Start == ButtonState.Released && CurrentState == GameState.End) //Kill the game
            {
                Exit();
            }
            if(armor > 4)
            {
                shieldNum = new Sprite((Content.Load<Texture2D>("extraarmor")), new Vector2(121, 40), Color.White, 0, 0);
                armor -= 1;
            }
            if (armor == 4)
            {
                shield = new Sprite((Content.Load<Texture2D>("overarmor")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("extraarmor")), new Vector2(125, 40), Color.White, 0, 0);
            } //Level 3 Armor
            if (armor == 3)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield3")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("threearmor")), new Vector2(80, 40), Color.White, 0, 0);
            } //Level 3 Armor
            if(armor == 2)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield2")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("twoarmor")), new Vector2(80, 40), Color.White, 0, 0);
            } //Level 2 Armor
            if(armor == 1)
            {
                shield = new Sprite((Content.Load<Texture2D>("shield1")), new Vector2(ship.Position.X, ship.Position.Y), Color.White, 0, 0);
                shieldNum = new Sprite((Content.Load<Texture2D>("onearmor")), new Vector2(80, 40), Color.White, 0, 0);
            } //Level 1 Armor
            if(armor < 1)
            {
                shieldNum = new Sprite((Content.Load<Texture2D>("noarmor")), new Vector2(80, 40), Color.White, 0, 0);
            } //If armor is non-existant
            if(adminMode == true) //enable admin mode
            {
                armor = 3;
            }

            if (CurrentState == GameState.Paused && selectionPaused == 1)
            {
                pausedToMain = new Sprite((Content.Load<Texture2D>("mainButton")), new Vector2(GraphicsDevice.Viewport.Width / 2 - 15, GraphicsDevice.Viewport.Height / 2 + 120), Color.White, 0, 0);
            }
            if(CurrentState == GameState.Paused && selectionPaused == 2)
            {
                pausedToMain = new Sprite((Content.Load<Texture2D>("mainButtonSelect")), new Vector2(GraphicsDevice.Viewport.Width / 2 - 15, GraphicsDevice.Viewport.Height / 2 + 120), Color.White, 0, 0);
            }
            if(selectionPaused > 2)
            {
                selectionPaused--;
            }
            if(selectionPaused < 1)
            {
                selectionPaused++;
            }

            if (CurrentState != GameState.Playing && CurrentState != GameState.Settings && CurrentState != GameState.Paused)
            {
                if (selectionMain == 0)
                {
                    start = new Sprite((Content.Load<Texture2D>("start")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    settings = new Sprite((Content.Load<Texture2D>("settings")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    quit = new Sprite((Content.Load<Texture2D>("quit")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionMain == 1)
                {
                    start = new Sprite((Content.Load<Texture2D>("start2")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    settings = new Sprite((Content.Load<Texture2D>("settings")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    quit = new Sprite((Content.Load<Texture2D>("quit")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionMain == 2)
                {
                    start = new Sprite((Content.Load<Texture2D>("start")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    settings = new Sprite((Content.Load<Texture2D>("settings2")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    quit = new Sprite((Content.Load<Texture2D>("quit")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionMain == 3)
                {
                    start = new Sprite((Content.Load<Texture2D>("start")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    settings = new Sprite((Content.Load<Texture2D>("settings")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    quit = new Sprite((Content.Load<Texture2D>("quit2")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionMain > 3)
                {
                    start = new Sprite((Content.Load<Texture2D>("start")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    settings = new Sprite((Content.Load<Texture2D>("settings")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                    quit = new Sprite((Content.Load<Texture2D>("quit")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
            }
            if(CurrentState == GameState.Settings)
            {
                if (selectionSettingsHorizontal == 0)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip1_orange")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsHorizontal == 1)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip1_red")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsHorizontal == 2)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip1_green")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsHorizontal == 3)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip2_blue")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsHorizontal == 4)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip2_green")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsHorizontal == 5)
                {
                    ship = new Sprite((Content.Load<Texture2D>("playerShip2_orange")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, 0, 0);
                }
                if (selectionSettingsVertical == 0)
                {
                    arrows = new Sprite((Content.Load<Texture2D>("arrows")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 65), Color.White, 0, 0);
                }
                if (selectionSettingsVertical == 1)
                {
                    selectionSettingsDifficulty = 0;
                    arrows = new Sprite((Content.Load<Texture2D>("arrowsSelect")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 65), Color.White, 0, 0);
                }


                if (selectionSettingsDifficulty == 1)
                {
                    easy = new Sprite((Content.Load<Texture2D>("easySelect")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    medium = new Sprite((Content.Load<Texture2D>("medium")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    hard = new Sprite((Content.Load<Texture2D>("hard")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                }
                if (selectionSettingsDifficulty == 2)
                {
                    easy = new Sprite((Content.Load<Texture2D>("easy")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    medium = new Sprite((Content.Load<Texture2D>("mediumSelect")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    hard = new Sprite((Content.Load<Texture2D>("hard")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                }
                if (selectionSettingsDifficulty == 3)
                {
                    easy = new Sprite((Content.Load<Texture2D>("easy")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    medium = new Sprite((Content.Load<Texture2D>("medium")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                    hard = new Sprite((Content.Load<Texture2D>("hardSelect")), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 80), Color.White, 0, 0);
                }

                if(difficulty == 1)
                {
                    easy.Position.Y = GraphicsDevice.Viewport.Height / 2 - 92;
                }
                if (difficulty == 2)
                {
                    medium.Position.Y = GraphicsDevice.Viewport.Height / 2 - 92;
                }
                if (difficulty == 3)
                {
                    hard.Position.Y = GraphicsDevice.Viewport.Height / 2 - 92;
                }
            }

            //if (score %1000 == 0)

            shipMirror1.Position = new Vector2(ship.Position.X - 110, ship.Position.Y);
            shipMirror2.Position = new Vector2(ship.Position.X + 110, ship.Position.Y);
            shipMirror3.Position = new Vector2(ship.Position.X, ship.Position.Y + 110);
            shipMirror4.Position = new Vector2(ship.Position.X, ship.Position.Y - 110);

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

            if (CurrentState != GameState.Paused && CurrentState != GameState.End && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                if (xBone.Triggers.Right > 0.5f && pks.Triggers.Right < 0.5f && destroyed == false && adminMode == false && mirage == true)
                {
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(ship.Position.X, ship.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(shipMirror1.Position.X, shipMirror1.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(shipMirror2.Position.X, shipMirror2.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(shipMirror3.Position.X, shipMirror3.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(shipMirror4.Position.X, shipMirror4.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    shotVib = true;
                    elaspedVibTime = new TimeSpan();
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    bullets[bullets.Count - 1].rotation = ship.rotation;
                    bullets[bullets.Count - 2].rotation = ship.rotation;
                    bullets[bullets.Count - 3].rotation = ship.rotation;
                    bullets[bullets.Count - 4].rotation = ship.rotation;
                    bullets[bullets.Count - 5].rotation = ship.rotation;

                    bullets[bullets.Count - 1].velocity = new Vector2((float)Math.Cos(ship.rotation - MathHelper.PiOver2), (float)Math.Sin(ship.rotation - MathHelper.PiOver2)) * mag;

                }
                if (xBone.Triggers.Right > 0.5f && pks.Triggers.Right < 0.5f && destroyed == false && adminMode == false && mirage == false)
                {
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(ship.Position.X, ship.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    //bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(shipMirror1.Position.X, shipMirror1.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    shotVib = true;
                    elaspedVibTime = new TimeSpan();
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    bullets[bullets.Count - 1].rotation = ship.rotation;
                    bullets[bullets.Count - 1].velocity = new Vector2((float)Math.Cos(ship.rotation - MathHelper.PiOver2), (float)Math.Sin(ship.rotation - MathHelper.PiOver2)) * mag;
                }



                if (xBone.Triggers.Right > 0.5f && CurrentState != GameState.End && adminMode == true)
                {
                    bullets.Add(new Bullets(Content.Load<Texture2D>("laserRed16"), new Vector2(ship.Position.X, ship.Position.Y), new Vector2(22, 0), MathHelper.ToDegrees(ship.rotation)));
                    shotVib = true;
                    elaspedVibTime = new TimeSpan();
                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    bullets[bullets.Count - 1].rotation = ship.rotation;
                    bullets[bullets.Count - 1].velocity = new Vector2((float)Math.Cos(ship.rotation - MathHelper.PiOver2), (float)Math.Sin(ship.rotation - MathHelper.PiOver2)) * mag;
                }
         
                if (elaspedGameTimeSmall >= spawnTimerSmall)
                {
                    Texture2D randImg = randTexturesSmall[random.Next(0, randTexturesSmall.Length)];
                    if (asteroidsSmall.Count < 30)
                    {
                        asteroidsSmall.Add(new Asteroid(randImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position, random));
                    }
                    else
                    {

                    }
                    elaspedGameTimeSmall = new TimeSpan();
                }
                if (elaspedGameTimeBig >= spawnTimerBig)
                {
                    Texture2D randImg = randTexturesBig[random.Next(0, randTexturesBig.Length)];
                    if (asteroidsBig.Count < 30)
                    {
                        asteroidsBig.Add(new Asteroid(randImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position, random));
                    }
                    else
                    {

                    }
                    elaspedGameTimeBig = new TimeSpan();
                }
                if (elaspedGameTimer >= spawnBackTimer)
                {
                    Texture2D randBackImg = randBackTextures[random.Next(0, randBackTextures.Length)];
                    backAsteroids.Add(new Asteroid(randBackImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position, random));
                    elaspedGameTimer = new TimeSpan();
                }
                if (elaspedPowerTimer >= spawnPowerTimer)
                {
                    Texture2D randPowerImg = randPowerTextures[random.Next(0, randPowerTextures.Length)];
                    powerUp.Add(new Asteroid(randPowerImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position, random));
                    elaspedPowerTimer = new TimeSpan();
                }
                if (elaspedMirageTimer >= spawnMirageTimer)
                {
                    Texture2D randMirageImg = randMirageTextures[random.Next(0, randMirageTextures.Length)];
                    mirageFour.Add(new Asteroid(randMirageImg, random.Next(3, 6), GraphicsDevice.Viewport, ship.Position, random));
                    elaspedMirageTimer = new TimeSpan();

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

                if (xBone.Buttons.X == ButtonState.Pressed && xBone.Buttons.Y == ButtonState.Pressed && pks.Buttons.Y == ButtonState.Released)
                {
                    adminMode = !adminMode;
                }

                magnitude = (float)(Math.Pow(xBone.ThumbSticks.Right.X, 2) + Math.Pow(xBone.ThumbSticks.Right.Y, 2));

                if (magnitude > .85f)
                {
                    ship.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                    if (mirage)
                    {
                        shipMirror1.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                        shipMirror2.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                        shipMirror3.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                        shipMirror4.rotation = (float)(Math.Atan2(-xBone.ThumbSticks.Right.Y, xBone.ThumbSticks.Right.X)) + MathHelper.PiOver2;
                    }
                }

                speed.Y = -xBone.ThumbSticks.Left.Y * tempSpeed.Y;
                speed.X = xBone.ThumbSticks.Left.X * tempSpeed.X;               
                ship.Position += speed;
                if(mirage)
                { 
                    shipMirror1.Position += speed;
                    shipMirror2.Position += speed;
                    shipMirror3.Position += speed;
                    shipMirror4.Position += speed;
                }
                shield.Position = ship.Position;
                shield.rotation = ship.rotation;

                foreach (Asteroid asteroid in asteroidsSmall)
                {
                    asteroid.Update();
                }
                foreach (Asteroid asteroid in asteroidsBig)
                {
                    asteroid.Update();
                }
                foreach (Asteroid asteroid in backAsteroids)
                {
                    asteroid.Update();
                }
                foreach (Asteroid asteroid in powerUp)
                {
                    asteroid.Update();
                }
                foreach (Asteroid asteroid in mirageFour)
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
                bool bulletRemoved = false;
                for (int i = 0; i < asteroidsSmall.Count; i++)
                {

                    if (v >= 0 && bullets.Count >= 0 && asteroidsSmall.Count >= 0 && bullets[v].HitBox.Intersects(asteroidsSmall[i].HitBox))
                    {
                        asteroidsSmall[i].Position = new Vector2(-900, -900);
                        asteroidsSmall[i].SetDirection(GraphicsDevice.Viewport);

                        //bullets.Remove(bullets[v]);
                        bullets.RemoveAt(v);
                        spawnTimeSmall += 3;
                        spawnTimeBig += 3;
                        spawnBackTime -= 1;
                        score += 100;
                        i--;
                        //if(v > 0)
                        {
                            v--;
                        }
                        bulletRemoved = true;
                        //break;
                        spawnTimerSmall = TimeSpan.FromMilliseconds(spawnTimeSmall);
                        spawnTimerBig = TimeSpan.FromMilliseconds(spawnTimeBig);
                        spawnBackTimer = TimeSpan.FromMilliseconds(spawnBackTime);
                        break;
                        
                    }
                
                                    
                }
                for (int t = 0; t < asteroidsBig.Count; t++)
                {

                    if (v >=0 && bullets.Count >= 0 && asteroidsBig.Count >= 0 && bullets[v].HitBox.Intersects(asteroidsBig[t].HitBox))
                    {

                        if (asteroidsSmall.Count < 30)
                        {
                            asteroidsSmall.Add(new Asteroid(randTexturesSmall[random.Next(randTexturesSmall.Length)], random.Next(3, 6), asteroidsBig[t].velocity, asteroidsBig[t].Position, random));
                            asteroidsSmall.Add(new Asteroid(randTexturesSmall[random.Next(randTexturesSmall.Length)], random.Next(3, 6), asteroidsBig[t].velocity, asteroidsBig[t].Position, random));
                        }

                        asteroidsBig[t].Position = new Vector2(-900, -900);
                        asteroidsBig[t].SetDirection(GraphicsDevice.Viewport);

                        Texture2D randImg = randTexturesSmall[random.Next(0, randTexturesSmall.Length)];


                        //bullets.Remove(bullets[v]);
                        bullets.RemoveAt(v);
                        spawnTimeBig += 3;
                        spawnTimeSmall += 3;

                       
                        spawnBackTime -= 3;
                        score += 100;
                        t--;

                        bulletRemoved = true;
                        //break;
                        spawnTimerSmall = TimeSpan.FromMilliseconds(spawnTimeSmall);
                        spawnTimerBig = TimeSpan.FromMilliseconds(spawnTimeBig);
                        spawnBackTimer = TimeSpan.FromMilliseconds(spawnBackTime);
                        break;

                    }


                }

                if (bulletRemoved)
                {
                    continue;
                }
                for(int u = 0; u < powerUp.Count; u++)
                {

                   if (bullets.Count >= 0 && powerUp.Count >= 0 && asteroidsSmall.Count >= 0 && asteroidsBig.Count >= 0 && bullets[v].HitBox.Intersects(powerUp[u].HitBox))
                    {
                        //bullets.Remove(bullets[v]);
                        //powerUp.Remove(powerUp[u]);
                        bullets.RemoveAt(v);
                        powerUp.RemoveAt(u);
                        
                        u--;

                        armor++;
                        
                        bulletRemoved = true;
                        spawnPowerTimer = TimeSpan.FromMilliseconds(spawnPowerTime);
                        break;
                       
                    }

                    
                }

                if(bulletRemoved)
                {
                    continue;
                }

                for (int u = 0; u < mirageFour.Count; u++)
                {

                    if (bullets.Count >= 0 && mirageFour.Count >= 0 && asteroidsSmall.Count >= 0 && asteroidsBig.Count >= 0 && bullets[v].HitBox.Intersects(mirageFour[u].HitBox))
                    {
                        //bullets.Remove(bullets[v]);
                        //powerUp.Remove(powerUp[u]);
                        bullets.RemoveAt(v);
                        mirageFour.RemoveAt(u);

                        if (mirage != true)
                        {
                            mirage = true;
                            multiTimer = TimeSpan.FromSeconds(10);
                        }                        

                        bulletRemoved = true;
                        spawnMirageTimer = TimeSpan.FromMilliseconds(spawnMirageTime);
                        break;
                    }
                }

                if (bulletRemoved)
                {
                    continue;
                }

                if (bullets[v].HitBox.Left < 0
                    ||bullets[v].HitBox.Bottom > GraphicsDevice.Viewport.Height 
                    ||bullets[v].HitBox.Top < 0 
                    ||bullets[v].HitBox.Right > GraphicsDevice.Viewport.Width)
                {
                    bullets.RemoveAt(v);
                    if (v < 0) {v--;}
                    break;
                }

                if (bullets.Count > 0)
                {
                  bullets[v].Update();
                } //Update The Bullets

            }          
        }

        public void UpdateAsteroids()
        {
            for(int u = 0; u < asteroidsSmall.Count; u++)
            {
                if (asteroidsSmall[u].HitBox.Right < -asteroidsSmall[u].HitBox.Width * 2 ||
                    asteroidsSmall[u].HitBox.Left > GraphicsDevice.Viewport.Width + asteroidsSmall[u].HitBox.Width * 2 ||
                    asteroidsSmall[u].HitBox.Top < -asteroidsSmall[u].HitBox.Height * 2||
                    asteroidsSmall[u].HitBox.Bottom > GraphicsDevice.Viewport.Height + asteroidsSmall[u].HitBox.Height * 2)
                {
                    //asteroidsSmall[u].Position = new Vector2(-900, -900);
                    //asteroidsSmall[u].SetDirection(GraphicsDevice.Viewport);
                    asteroidsSmall.RemoveAt(u);
                    u--;
                    continue;
                }

                if (armor == 0 && asteroidsSmall[u].HitBox.Intersects(ship.HitBox))
                {
                    CurrentState = GameState.End;
                }
            }
            for (int b = 0; b < asteroidsBig.Count; b++)
            {

                if (asteroidsBig[b].HitBox.Right < -asteroidsBig[b].HitBox.Width * 2 ||
                    asteroidsBig[b].HitBox.Left > GraphicsDevice.Viewport.Width + asteroidsBig[b].HitBox.Width * 2 ||
                    asteroidsBig[b].HitBox.Top < -asteroidsBig[b].HitBox.Height * 2 ||
                    asteroidsBig[b].HitBox.Bottom > GraphicsDevice.Viewport.Height + asteroidsBig[b].HitBox.Height * 2)
                {
                    asteroidsBig[b].Position = new Vector2(-900, -900);
                    asteroidsBig[b].SetDirection(GraphicsDevice.Viewport);
                    b--;
                    continue;
                }

                if (armor == 0 && asteroidsBig[b].HitBox.Intersects(ship.HitBox))
                {
                    CurrentState = GameState.End;
                }
            }
        }

        public void UpdateSheilds()
        {
            for (int y = 0; y < asteroidsSmall.Count; y++)
            {
                if(ship.HitBox.Intersects(asteroidsSmall[y].HitBox) && armor != 0)
                {
                    asteroidsSmall[y].Position = new Vector2(-900, -900);
                    asteroidsSmall[y].SetDirection(GraphicsDevice.Viewport);

                    armor--;
                    y--;
                }

                else
                { }
            }

            for (int x = 0; x < asteroidsBig.Count; x++)
            {
                if (ship.HitBox.Intersects(asteroidsBig[x].HitBox) && armor != 0)
                {
                    asteroidsBig[x].Position = new Vector2(-900, -900);
                    asteroidsBig[x].SetDirection(GraphicsDevice.Viewport);

                    armor--;
                    x--;
                } 

                else
                { }
            }

            for (int w = 0; w < asteroidsSmall.Count; w++)
            {
                if (ship.HitBox.Intersects(asteroidsSmall[w].HitBox) && armor != 0)
                {
                    asteroidsSmall[w].Position = new Vector2(-900, -900);
                    asteroidsSmall[w].SetDirection(GraphicsDevice.Viewport);

                    armor--;
                    w--;
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

            if(CurrentState == GameState.Settings)
            {
                arrows.Draw(spriteBatch);
                ship.Draw(spriteBatch);
                easy.Draw(spriteBatch);
                medium.Draw(spriteBatch);
                hard.Draw(spriteBatch);
            }


            if (debug == true)
            {
                spriteBatch.DrawString(font, $"X:{x} Y:{y}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 40), Color.White);
                spriteBatch.DrawString(font, $"Magnitude:{magnitude}", new Vector2(5, GraphicsDevice.Viewport.Height / 2), Color.White);
                spriteBatch.DrawString(font, $"X:{v} Y:{w}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                spriteBatch.DrawString(font, $"ship position: ({ship.Position.X},{ship.Position.Y})", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 20), Color.White);

                spriteBatch.DrawString(font, $"rotation \n {ship.rotation}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 60), Color.Wheat);
                spriteBatch.DrawString(font, $"asteroids count \n Small: {asteroidsSmall.Count}, Big: {asteroidsBig.Count}, Back: {backAsteroids.Count}, Power: {powerUp.Count}, Mirage: {mirageFour.Count}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 100), Color.Wheat);
                spriteBatch.DrawString(font, $"bullet count \n {bullets.Count}", new Vector2(5, GraphicsDevice.Viewport.Height / 2 + 140), Color.Wheat);
                spriteBatch.DrawString(font, $"Selection {selectionMain}", new Vector2(GraphicsDevice.Viewport.Width / 2 + 430, GraphicsDevice.Viewport.Height / 2 - 400), Color.Wheat);

                spriteBatch.DrawString(font, $"Spawner: {spawnTimeSmall} , {spawnTimeBig}", new Vector2(GraphicsDevice.Viewport.Width / 2 + 430, GraphicsDevice.Viewport.Height / 2 - 400), Color.Wheat);
                spriteBatch.DrawString(font, $"Back Spawner: {spawnBackTime}", new Vector2(GraphicsDevice.Viewport.Width / 2 + 430, GraphicsDevice.Viewport.Height / 2 - 380), Color.Wheat);
            }

            if (CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                spriteBatch.DrawString(font, $"SCORE: {score}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 60, GraphicsDevice.Viewport.Height / 2 - 360), Color.White);
            }
            if (adminMode == true)
            {
                spriteBatch.DrawString(font, $"Admin Mode Enabled", new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 385), Color.White);
            }

            foreach (Asteroid asteroid in backAsteroids)
            { asteroid.Draw(spriteBatch); }

            foreach (Asteroid asteroid in powerUp)
            { asteroid.Draw(spriteBatch); }

            foreach (Asteroid asteroid in mirageFour)
            { asteroid.Draw(spriteBatch); }

            foreach (Bullets bullet in bullets)
            { bullet.Draw(spriteBatch); }

            foreach (Asteroid asteroid in asteroidsSmall)
            {
                asteroid.Draw(spriteBatch);
            }
            foreach (Asteroid asteroid in asteroidsBig)
            { asteroid.Draw(spriteBatch); }

            if (armor >= 1 && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                shield.Draw(spriteBatch);
            }
            else { }

            if (CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                shieldNum.Draw(spriteBatch);
            }

            if (CurrentState != GameState.End && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                ship.Draw(spriteBatch);
                if (mirage)
                {
                    shipMirror1.Draw(spriteBatch);
                    shipMirror2.Draw(spriteBatch);
                    shipMirror3.Draw(spriteBatch);
                    shipMirror4.Draw(spriteBatch);
                }

            }
            else if (CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                endScreen.Draw(spriteBatch);
            }

            if (mirage)
            {
                spriteBatch.DrawString(biggerfont, $"{multiTimer.Seconds}", new Vector2(GraphicsDevice.Viewport.X + 630, GraphicsDevice.Viewport.Y + 200), Color.Red);
            }

            if (CurrentState == GameState.Paused && CurrentState != GameState.End && CurrentState != GameState.MainMenu && CurrentState != GameState.Settings)
            {
                pausedScreen.Draw(spriteBatch); // Paused
                pausedToMain.Draw(spriteBatch);
            }

            if (CurrentState == GameState.MainMenu)
            {
                menuScreen.Draw(spriteBatch); // MainMenu Screen
            }

            if (CurrentState == GameState.MainMenu && CurrentState != GameState.Settings)
            { 
            quit.Draw(spriteBatch);
            start.Draw(spriteBatch);
            settings.Draw(spriteBatch);
            }

            spriteBatch.DrawString(biggerfont, $"{difficulty}", new Vector2(GraphicsDevice.Viewport.X + 630, GraphicsDevice.Viewport.Y + 10), Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
