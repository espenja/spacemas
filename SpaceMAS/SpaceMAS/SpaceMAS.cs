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

namespace SpaceMAS {

    public class SpaceMAS : Game {

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public List<Player> players = new List<Player>();

        //Needed for not letting actions get spammed every update (eg if you click Up in the main menu the up
        //action should not be done 182734 times(every update)
        private float timeSinceLastAction;

        private Player thisPlayer;
        private LevelController LevelController;
        private MenuController MenuController;
        //to see fps
        public SpriteFont fpsFont { get; private set; }

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
            fpsFont = Content.Load<SpriteFont>("Fonts/HandOfSean");
            thisPlayer = new Player("fictive", new Vector2(300, 300), Content.Load<Texture2D>("Textures/player"));
            players.Add(thisPlayer);

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
            Vector2 FontOrigin = fpsFont.MeasureString(fps.ToString()) / 2;
            spriteBatch.DrawString(fpsFont, fps.ToString(), FontOrigin, Color.LightGreen, 0, FontOrigin, 0.25f, SpriteEffects.None, 0.5f);
               

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
                        LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
                case GameState.PLAYING_NORMAL:
                    if (LevelController.CurrentLevel != null)
                        LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
                case GameState.PLAYING_HARD:
                    if (LevelController.CurrentLevel != null)
                        LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
