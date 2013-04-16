using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceMAS.Level;
using SpaceMAS.Models.Players;
using SpaceMAS.Settings;
using SpaceMAS.Utils;
using SpaceMAS.Menu;
using SpaceMAS.Models;

namespace SpaceMAS {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private const int screenWidth = 1024;
        private const int screenHeight = 800;

        public List<Player> players = new List<Player>();
        private bool gamePaused = false;
        private Player pausingPlayer;

        //Needed for not letting actions get spammed every update (eg if you click Up in the main menu the up
        //action should not be done 182734 times(every update)
        private float timeSinceLastAction = 0f;

        private GameState currentGameState = GameState.MAINMENU;
        private MenuButton playBtn, highsBtn, optionsBtn, quitBtn;
        private List<MenuButton> mainMenuButtons = new List<MenuButton>();
        private int highlightedButtonIndex = 3;

        private Player thisPlayer;
        private LevelController LevelController;

        public Game1() {
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

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            thisPlayer = new Player("fictive", new Vector2(300, 300)) {Texture = Content.Load<Texture2D>("Textures/player")};
            players.Add(thisPlayer);

            //Menu
            mainMenuButtons.Add(playBtn = new MenuButton(Content.Load<Texture2D>("Buttons/play"),
                                                         new Vector2(200, 200), graphics.GraphicsDevice,
                                                         thisPlayer.PlayerControls, GameState.PLAYING));
            mainMenuButtons.Add(highsBtn = new MenuButton(Content.Load<Texture2D>("Buttons/highscore"),
                                                          new Vector2(200, 300), graphics.GraphicsDevice,
                                                          thisPlayer.PlayerControls, GameState.HIGHSCORE));
            mainMenuButtons.Add(optionsBtn = new MenuButton(Content.Load<Texture2D>("Buttons/options"),
                                                            new Vector2(200, 400), graphics.GraphicsDevice,
                                                            thisPlayer.PlayerControls, GameState.OPTIONS));
            mainMenuButtons.Add(quitBtn = new MenuButton(Content.Load<Texture2D>("Buttons/quit"),
                                                         new Vector2(200, 500), graphics.GraphicsDevice,
                                                         thisPlayer.PlayerControls, GameState.QUIT));


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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param Name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                currentGameState == GameState.QUIT)
                this.Exit();

            switch (currentGameState) {
                case GameState.HIGHSCORE:
                    break;
                case GameState.MAINMENU:
                    UpdateMenuState();
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

        public void UpdateMenuState() {
            if (timeSinceLastAction > 0.2f) {
                if (Keyboard.GetState().IsKeyDown(thisPlayer.PlayerControls.MenuDown)) {
                    if (highlightedButtonIndex < mainMenuButtons.Count - 1) highlightedButtonIndex += 1;
                    else highlightedButtonIndex = 0;
                    timeSinceLastAction = 0f;

                }
                else if (Keyboard.GetState().IsKeyDown(thisPlayer.PlayerControls.MenuUp)) {
                    if (highlightedButtonIndex > 0) highlightedButtonIndex -= 1;
                    else highlightedButtonIndex = mainMenuButtons.Count - 1;
                    timeSinceLastAction = 0f;
                }
            }

            foreach (MenuButton button in mainMenuButtons) {
                button.Update(mainMenuButtons[highlightedButtonIndex]);
            }

            //Button in menu pressed, changes gamestate
            if (mainMenuButtons[highlightedButtonIndex].isPressed) {
                LevelController.CurrentLevel.GameState = GameState.PLAYING;
                currentGameState = mainMenuButtons[highlightedButtonIndex].changesToState;
            }
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param Name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(16, 36, 35));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            switch (currentGameState) {
                case GameState.HIGHSCORE:
                    break;
                case GameState.MAINMENU:
                    foreach (MenuButton button in mainMenuButtons) {
                        button.Draw(spriteBatch);
                    }
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
