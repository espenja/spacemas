using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Level;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;
using SpaceMAS.Menu;
using SpaceMAS.Models;

namespace SpaceMAS {

    public class SpaceMAS : Game {

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private const int screenWidth = 1024;
        private const int screenHeight = 800;

        public List<Player> players = new List<Player>();
        private bool gamePaused;
        private Player pausingPlayer;

        //Needed for not letting actions get spammed every update (eg if you click Up in the main menu the up
        //action should not be done 182734 times(every update)
        private float timeSinceLastAction = 0f;

        private Player thisPlayer;
        private LevelController LevelController;
        private MenuController MenuController;

        public SpaceMAS() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            //Screen stuff
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
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
            thisPlayer = new Player("fictive", new Vector2(300, 300)) {Texture = Content.Load<Texture2D>("Textures/player")};
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
                case GameState.PLAYING:
                    UpdatePlayingState(gameTime);
                    break;
            }
            timeSinceLastAction += (float) gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public void UpdatePlayingState(GameTime gameTime) {
            if (!gamePaused) {
                LevelController.CurrentLevel.Update(gameTime);
            }
            if (timeSinceLastAction > 1) {
                foreach (Player player in players) {
                    if (player.ClickedPauseKey() && !gamePaused) {
                        PauseGame(player);
                    }
                    else if (gamePaused && player.ClickedPauseKey() && player == pausingPlayer) {
                        UnPause();
                    }
                }
            }
        }

        public void PauseGame(Player pausingPlayer) {
            gamePaused = true;
            this.pausingPlayer = pausingPlayer;
            timeSinceLastAction = 0f;
        }

        public void UnPause() {
            gamePaused = false;
            pausingPlayer = null;
            timeSinceLastAction = 0f;
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(16, 36, 35));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            switch (StateProvider.Instance.State) {
                case GameState.HIGHSCORE:
                    break;
                case GameState.MENU:
                    MenuController.Draw(spriteBatch);
                    break;
                case GameState.OPTIONS:
                    break;
                case GameState.PLAYING:
                    LevelController.CurrentLevel.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
