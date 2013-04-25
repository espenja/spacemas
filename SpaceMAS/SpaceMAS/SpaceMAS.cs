using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Factories;
using SpaceMAS.Level;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;
using SpaceMAS.Menu;
using SpaceMAS.Settings;
using SpaceMAS.Graphics;

namespace SpaceMAS {

    public class SpaceMAS : Game {

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public List<Player> players = new List<Player>();

        //Needed for not letting actions get spammed every update (eg if you click Up in the main menu the up
        //action should not be done 182734 times(every update)
        private float timeSinceLastAction;
        private LevelController LevelController;
        private MenuController MenuController;
        //to see fps
        public SpriteFont TextFont { get; private set; }

        public Texture2D T { get; set; }

        private int ShopCost { get; set; }

        //save gamestate for pause
        private GameState BeforePauseState { get; set; }

        public SpaceMAS() {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 600);
            //graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.ApplyChanges();
           
            GameServices.AddService(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            T = new Texture2D(GraphicsDevice, 1, 1);
            T.SetData(new [] { Color.White } );

            //Screen stuff
            graphics.PreferredBackBufferWidth = GeneralSettings.screenWidth;
            graphics.PreferredBackBufferHeight = GeneralSettings.screenHeight;
            graphics.ApplyChanges();

            GameServices.AddService(graphics);
            GameServices.AddService(GraphicsDevice);
            GameServices.AddService(Content);

            LevelController = new LevelController();
            LevelController.GoToNextLevel();
            GameServices.AddService(LevelController);

            MenuController = new MenuController();
            ShopCost = 50;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            TextFont = Content.Load<SpriteFont>("Fonts/HandOfSean");
            players.Add(new Player("Player 1", new Vector2(300, 300), Content.Load<Texture2D>("Textures/player")));
            players.Add(new Player("Player 2", new Vector2(400, 400), Content.Load<Texture2D>("Textures/player")));

            GameServices.AddService(players);
            LevelController.InitializeLevels();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
                
            switch (StateProvider.Instance.State) {
                case GameState.QUIT:
                    Exit();
                    break;
                case GameState.HIGHSCORE:
                    if (Keyboard.GetState().IsKeyDown(Controls.Back)) StateProvider.Instance.State = GameState.MENU;
                    break;
                case GameState.MENU:
                    MenuController.Update(gameTime);
                    break;
                case GameState.CONTROLS:
                    UpdateControlsState(gameTime);
                    break;
                case GameState.GAMEPAUSED:
                    UpdateGamepausedState();
                    break;
                case GameState.LEVEL_INTRO:
                    LevelController.CurrentLevel.LevelIntro.Update(gameTime);
                    break;
                case GameState.PLAYING_EASY:
                    UpdatePlayingState(gameTime);
                    break;
                case GameState.PLAYING_NORMAL:
                    UpdatePlayingState(gameTime);
                    break;
                case GameState.PLAYING_HARD:
                    UpdatePlayingState(gameTime);
                    break;
                case GameState.SHOP_DAMAGE:
                    foreach (var player in players) {
                        if (Keyboard.GetState().IsKeyDown(player.PlayerControls.MenuSelect)) {
                            if (player.Money >= ShopCost) {
                                player.Weapon.BulletType.HealthChange -= 10;
                                player.Money -= ShopCost;
                            }   
                        }
                        StateProvider.Instance.State = GameState.MENU;
                    }
                    break;
                case GameState.SHOP_ACCELERATION:
                    foreach (var player in players) {
                        if (Keyboard.GetState().IsKeyDown(player.PlayerControls.MenuSelect)) {
                            if (player.Money >= ShopCost) {
                                player.AccelerationRate += 1;
                                player.Money -= ShopCost;
                            } 
                        }
                        StateProvider.Instance.State = GameState.MENU;
                    }
                    break;
                case GameState.SHOP_HEALTH:
                    foreach (var player in players) {
                        if (Keyboard.GetState().IsKeyDown(player.PlayerControls.MenuSelect)) {
                            if (player.Money >= ShopCost) {
                                player.MaxHealthPoints += 10;
                                player.HealthPoints += 10;
                                player.Money -= ShopCost;
                            } 
                        }
                        StateProvider.Instance.State = GameState.MENU;
                    }
                    break;
                case GameState.SHOP_HEAL:
                    foreach (var player in players) {
                        if (Keyboard.GetState().IsKeyDown(player.PlayerControls.MenuSelect)) {
                            if (player.Money >= ShopCost) {
                                player.HealthPoints = player.MaxHealthPoints;
                                player.Money -= ShopCost;
                            }    
                        }
                        StateProvider.Instance.State = GameState.MENU;
                    }
                    break;
                case GameState.SHOP_BULLETSPEED:
                    foreach (var player in players) {
                        if (Keyboard.GetState().IsKeyDown(player.PlayerControls.MenuSelect)) {
                            if (player.Money >= ShopCost) {
                                player.Weapon.BulletType.TravelSpeed += 50;
                                player.Money -= ShopCost;
                            } 
                        }
                        StateProvider.Instance.State = GameState.MENU;
                    }
                    break;
                case GameState.SHOP_DONE:
                    LevelController.GoToNextLevel();
                    break;
            }
            timeSinceLastAction += (float) gameTime.ElapsedGameTime.TotalSeconds;
             
            base.Update(gameTime);
        }

        protected void UpdateControlsState(GameTime gameTime) {
            foreach (Player player in players) {
                if (Keyboard.GetState().IsKeyDown(Controls.Back)) {
                    StateProvider.Instance.State = GameState.MENU;
                }
            }
            Draw(gameTime);
            
        }

        private bool AllPlayersDead() {
            foreach (var player in players) {
                if (!player.Dead) return false;
            }
            return true;
        }

        private void UpdateHighScore()
        {
            List<String> highscore = HighscoreProvider.Instance.Highscore;
            foreach (var score in highscore)
            {
                if (LevelController.CurrentLevel.Id >= Convert.ToInt32(score.Split(' ')[1]))
                {
                    highscore.Insert(highscore.IndexOf(score), "Level " + LevelController.CurrentLevel.Id + " : " + players[0].Name + " + " + players[1].Name);
                    HighscoreProvider.Instance.SaveHighscore();
                    return;
                }
            }
            highscore.Add("Level " + LevelController.CurrentLevel.Id + " : " + players[0].Name + " + " + players[1].Name);
            HighscoreProvider.Instance.SaveHighscore();
        }

        private void Reset()
        {
            
        }

        protected void UpdatePlayingState(GameTime gameTime) {
            if (AllPlayersDead())
            {
                MenuController.ChangeMenu(0);
                StateProvider.Instance.State = GameState.MENU;
                UpdateHighScore();
                Reset();
            }
            if (LevelController.CurrentLevel != null)
                LevelController.CurrentLevel.Update(gameTime);
            else
                Exit();
            if (timeSinceLastAction > 0.1) {
                if (players[0].ClickedPauseKey()) {
                    PauseGame();
                }
            }
        }

        protected void UpdateGamepausedState() {
            if (timeSinceLastAction > 0.5) {
                foreach (Player player in players) {
                    if (player.ClickedPauseKey()) {
                        UnPause();
                    }
                }
            }    
        }

        protected void PauseGame() {
            BeforePauseState = StateProvider.Instance.State;
            StateProvider.Instance.State = GameState.GAMEPAUSED;
            timeSinceLastAction = 0f;
        }

        protected void UnPause() {
            StateProvider.Instance.State = BeforePauseState;
            timeSinceLastAction = 0f;
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(16, 36, 35));
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            //fps counter
            int fps = Frames.CalculateFrameRate();
            var fontOrigin = TextFont.MeasureString(fps.ToString()) / 2;
            var position = new Vector2(50, 50);
            spriteBatch.DrawString(TextFont, fps.ToString(), position, Color.LightGreen, 0, fontOrigin, 0.25f, SpriteEffects.None, GameDrawOrder.BACKGROUND_TOP);


            switch (StateProvider.Instance.State) {
                case GameState.HIGHSCORE:
                    DrawHighscore(spriteBatch);
                    break;
                case GameState.MENU:
                    MenuController.Draw(spriteBatch, TextFont);
                    DrawPlayerMoney(spriteBatch);
                    break;
                case GameState.CONTROLS:
                    DrawControls(spriteBatch);
                    break;
                case GameState.GAMEPAUSED:
                    LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
                case GameState.LEVEL_INTRO:
                    LevelController.CurrentLevel.LevelIntro.Draw(spriteBatch);
                    break;
                case GameState.PLAYING_EASY:
                    if (LevelController.CurrentLevel != null) {
                        LevelController.CurrentLevel.Draw(spriteBatch);
                        DrawPlayerMoney(spriteBatch);
                    }
                    break;
                case GameState.PLAYING_NORMAL:
                    if (LevelController.CurrentLevel != null) {
                        LevelController.CurrentLevel.Draw(spriteBatch);
                        DrawPlayerMoney(spriteBatch);
                    }
                    break;
                case GameState.PLAYING_HARD:
                    if (LevelController.CurrentLevel != null) {
                        LevelController.CurrentLevel.Draw(spriteBatch);
                        DrawPlayerMoney(spriteBatch);
                    }
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected void DrawPlayerMoney(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(10, 10);
            float scale = 0.4f;
            foreach (var player in players)
            {
                spriteBatch.DrawString(TextFont, player.Name + " : " + player.Money.ToString() + "$", 
                    position, Color.Gold, 0, Vector2.Zero, scale,
                    SpriteEffects.None, GameDrawOrder.BACKGROUND_TOP);
                position.Y += TextFont.LineSpacing * scale;
            }
        }

        protected void DrawHighscore(SpriteBatch spriteBach) {
            float scale = 0.5f;
            Vector2 position = new Vector2(GeneralSettings.screenHeight / 10, GeneralSettings.screenWidth / 50);
            spriteBatch.DrawString(TextFont, "Highscore", position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
            position.Y += TextFont.LineSpacing*scale;

            scale = 0.3f;
            List<String> highScore = HighscoreProvider.Instance.Highscore;
            int i = 1;
            foreach (var score in highScore) {
                spriteBatch.DrawString(TextFont, i + ": " + score, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                i++;
            }
        }

        protected void DrawControls(SpriteBatch spriteBatch)
        {
            
            Vector2 position = new Vector2(GeneralSettings.screenHeight / 10, GeneralSettings.screenWidth / 50);

            foreach (Player player in players)
            {
                float scale = 0.5f;
                String keyLine = player.Name;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Black, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                scale = 0.3f;

                keyLine = "Accelerate:      " + player.PlayerControls.Accelerate;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Decelerate:      " + player.PlayerControls.Decelerate;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Turn left:           " + player.PlayerControls.TurnLeft;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Turn right:          " + player.PlayerControls.TurnRight;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Shoot:                 " + player.PlayerControls.Shoot;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Menu Select:     " + player.PlayerControls.MenuSelect;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
                keyLine = "Pause:                " + Controls.Pause;
                spriteBatch.DrawString(TextFont, keyLine, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, GameDrawOrder.FOREGROUND_MIDDLE);
                position.Y += TextFont.LineSpacing * scale;
            }
        }
    }
}

