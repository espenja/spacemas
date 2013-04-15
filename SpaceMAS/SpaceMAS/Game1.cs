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
using SpaceMAS.Models.Player;
using SpaceMAS.Settings;
using SpaceMAS.Utils;
using SpaceMAS.Menu;
using SpaceMAS.Models;

namespace SpaceMAS {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private int screenWidth = 800;
        private int screenHeight = 600;

        private List<Player> players = new List<Player>();
        private bool gamePaused = false;
        private Player pausingPlayer;

        //Needed for not letting actions get spammed every update (eg if you click Up in the main menu the up
        //action should not be done 182734 times(every update)
        private float timeSinceLastAction = 0f;

        private GameState currentGameState = GameState.MainMenu;
        private MenuButton playBtn, highsBtn, optionsBtn, quitBtn;
        private List<MenuButton> mainMenuButtons = new List<MenuButton>();
        private int highlightedButtonIndex = 3;

        private Player thisPlayer;

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
            GameServices.AddService(GraphicsDevice);
            GameServices.AddService(Content);

            LevelController levelController = new LevelController();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Screen stuff
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            Random random = new Random();
            thisPlayer = new Player("fictive", new Vector2(300, 300)) { Texture = Content.Load<Texture2D>("helicopter") };
            players.Add(thisPlayer);

            //Menu
            mainMenuButtons.Add(playBtn = new MenuButton(Content.Load<Texture2D>("PlayButton"), 
                new Vector2(200, 200), graphics.GraphicsDevice, thisPlayer.PlayerControls, GameState.Playing));
            mainMenuButtons.Add(highsBtn = new MenuButton(Content.Load<Texture2D>("HighsButton"),
                new Vector2(200, 300), graphics.GraphicsDevice, thisPlayer.PlayerControls, GameState.HighScore));
            mainMenuButtons.Add(optionsBtn = new MenuButton(Content.Load<Texture2D>("OptionsButton"),
                new Vector2(200, 400), graphics.GraphicsDevice, thisPlayer.PlayerControls, GameState.Options));
            mainMenuButtons.Add(quitBtn = new MenuButton(Content.Load<Texture2D>("QuitButton"),
                new Vector2(200, 500), graphics.GraphicsDevice, thisPlayer.PlayerControls, GameState.Quit));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentGameState == GameState.Quit)
                this.Exit();

            switch (currentGameState)
            {
                case GameState.HighScore:
                    break;
                case GameState.MainMenu:
                    UpdateMenuState();
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    UpdatePlayingState(gameTime);
                    break;
            }
            timeSinceLastAction += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public void UpdateMenuState()
        {
            if (timeSinceLastAction > 0.2f)
            {
                if (Keyboard.GetState().IsKeyDown(thisPlayer.PlayerControls.MenuDown))
                {
                    if (highlightedButtonIndex < mainMenuButtons.Count - 1) highlightedButtonIndex += 1;
                    else highlightedButtonIndex = 0;
                    timeSinceLastAction = 0f;

                }
                else if (Keyboard.GetState().IsKeyDown(thisPlayer.PlayerControls.MenuUp))
                {
                    if (highlightedButtonIndex > 0) highlightedButtonIndex -= 1;
                    else highlightedButtonIndex = mainMenuButtons.Count - 1;
                    timeSinceLastAction = 0f;
                }
            }

            foreach (MenuButton button in mainMenuButtons)
            {
                button.Update(mainMenuButtons[highlightedButtonIndex]);
            }

            //Button in menu pressed, changes gamestate
            if (mainMenuButtons[highlightedButtonIndex].isPressed)
            {
                currentGameState = mainMenuButtons[highlightedButtonIndex].changesToState;
            }
        }

        public void UpdatePlayingState(GameTime gameTime)
        {
            if (!gamePaused)
            {
                foreach (Player player in players)
                {
                    player.Update(gameTime);
                }
            }
            if (timeSinceLastAction > 1)
            {
                foreach (Player player in players)
                {
                    if (player.ClickedPauseKey() && !gamePaused)
                    {
                        PauseGame(player);
                    }
                    else if (gamePaused && player.ClickedPauseKey() && player == pausingPlayer)
                    {
                        UnPause();
                    }
                }
            }
            
        }

        public void PauseGame(Player pausingPlayer)
        {
            gamePaused = true;
            this.pausingPlayer = pausingPlayer;
            timeSinceLastAction = 0f;
        }

        public void UnPause()
        {
            gamePaused = false;
            pausingPlayer = null;
            timeSinceLastAction = 0f;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param Name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.HighScore:
                    break;
                case GameState.MainMenu:
                    foreach (MenuButton button in mainMenuButtons)
                    {
                        button.Draw(spriteBatch);
                    }
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    foreach (Player player in players)
                    {
                        player.Draw(spriteBatch);
                    }
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
