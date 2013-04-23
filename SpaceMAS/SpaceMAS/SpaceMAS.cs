using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        //save gamestate for pause
        private GameState BeforePauseState { get; set; }

        public SpaceMAS() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            TextFont = Content.Load<SpriteFont>("Fonts/HandOfSean");
            players.Add(new Player("fictive", new Vector2(300, 300), Content.Load<Texture2D>("Textures/player")));
            players.Add(new Player("fictive2", new Vector2(400, 400), Content.Load<Texture2D>("Textures/player")));

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

        protected override void Update(GameTime gameTime) {
                
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || StateProvider.Instance.State == GameState.QUIT)
                this.Exit();

            switch (StateProvider.Instance.State) {
                case GameState.HIGHSCORE:
                    break;
                case GameState.MENU:
                    MenuController.Update(gameTime);
                    break;
                case GameState.OPTIONS:
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
            }
            timeSinceLastAction += (float) gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected void UpdatePlayingState(GameTime gameTime) {
            if (LevelController.CurrentLevel != null)
                LevelController.CurrentLevel.Update(gameTime);
            else
                Exit();
            if (timeSinceLastAction > 0.1) {
                foreach (Player player in players) {
                    if (player.ClickedPauseKey()) {
                        PauseGame();
                    }
                }
            }
        }

        protected void UpdateGamepausedState() {
            if (timeSinceLastAction > 0.1) {
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
            Vector2 fontOrigin = TextFont.MeasureString(fps.ToString()) / 2;
            Vector2 position = new Vector2(GeneralSettings.screenHeight - 10, GeneralSettings.screenWidth - 10);
            spriteBatch.DrawString(TextFont, fps.ToString(), position, Color.LightGreen, 0, fontOrigin, 0.25f, SpriteEffects.None, 0.5f);
               

            switch (StateProvider.Instance.State) {
                case GameState.HIGHSCORE:
                    break;
                case GameState.MENU:
                    MenuController.Draw(spriteBatch);
                    break;
                case GameState.OPTIONS:
                    break;
                case GameState.GAMEPAUSED:
                    LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
                case GameState.LEVEL_INTRO:
                    LevelController.CurrentLevel.LevelIntro.Draw(spriteBatch);
                    break;
                case GameState.PLAYING_EASY:
                    if (LevelController.CurrentLevel != null)
                    {
                        LevelController.CurrentLevel.Draw(spriteBatch);
                        DrawPlayerMoney(spriteBatch);
                    }
                    break;
                case GameState.PLAYING_NORMAL:
                    if (LevelController.CurrentLevel != null)
                    {
                        LevelController.CurrentLevel.Draw(spriteBatch);
                        DrawPlayerMoney(spriteBatch);
                    }
                    break;
                case GameState.PLAYING_HARD:
                    if (LevelController.CurrentLevel != null)
                    {
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
            foreach (var player in players)
            {
                spriteBatch.DrawString(TextFont, player.Name + " : " + player.Money.ToString() + "$", 
                    position, Color.Gold, 0, Vector2.Zero, 0.4f,
                    SpriteEffects.None, GameDrawOrder.BACKGROUND_TOP);
                position.Y += 50;
            }
        }
    }
}
