using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Graphics;
using SpaceMAS.Settings;
using SpaceMAS.State;
using SpaceMAS.Utils;

namespace SpaceMAS.Level {

    public class LevelIntro {

        private enum IntroState {
            FADING_IN,
            VISIBLE,
            FADING_OUT
        }

        private SpriteFont LevelIntroFont;
        private Vector2 FontPosition;
        private Texture2D Texture;
        private Rectangle IntroScreen;

        public bool IntroRunning = true;
        private double timer;
        private IntroState State = IntroState.FADING_IN;

        private const double FadeInTime = 1000;
        private const double VisibleTime = 2000;
        private const double FadeOutTime = 2000;
        private float Opacity;

        private Level Level;

        public LevelIntro(Level level) {

            Level = level;

            ContentManager contentManager = GameServices.GetService<ContentManager>();
            GraphicsDevice graphicsDevice = GameServices.GetService<GraphicsDevice>();
            GraphicsDeviceManager graphicsDeviceManager = GameServices.GetService<GraphicsDeviceManager>();
            LevelIntroFont = contentManager.Load<SpriteFont>(GeneralSettings.FontsPath + "HandOfSean");

            int Width = graphicsDeviceManager.PreferredBackBufferWidth;
            int Height = graphicsDeviceManager.PreferredBackBufferHeight;

            //Overlay
            IntroScreen = new Rectangle(0, 0, Width, Height);
            Texture = new Texture2D(graphicsDevice, 1, 1);
            Texture.SetData(new [] { Color.Black });

            //Font 
            Vector2 FontSize = LevelIntroFont.MeasureString(level.Name) * 0.5f;
            FontPosition = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2f - FontSize.X / 2, graphicsDeviceManager.PreferredBackBufferHeight / 2f);
        }


        public void Update(GameTime gameTime) {

            if (!IntroRunning) return;
            if (StateProvider.Instance.State != GameState.PLAYING) return;

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter)) {
                IntroRunning = false;
            }

            switch (State) {
                case IntroState.FADING_IN:

                    Opacity += (1f / (float) FadeInTime) * (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (timer > FadeInTime) {
                        timer = 0;
                        State = IntroState.VISIBLE;
                    }
                    break;
                case IntroState.VISIBLE:

                    Opacity = 1;

                    if (timer > VisibleTime) {
                        timer = 0;
                        State = IntroState.FADING_OUT;
                    }
                    break;
                case IntroState.FADING_OUT:

                    Opacity -= (1f / (float) FadeOutTime) * (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                    if(timer > FadeOutTime) {
                        IntroRunning = false;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Drawing.DrawString(spriteBatch, LevelIntroFont, Level.Name, FontPosition, Color.White, Opacity, 0.5f, GameDrawOrder.FOREGROUND_TOP);
            Drawing.Draw(spriteBatch, Texture, IntroScreen, Color.White, Opacity, GameDrawOrder.FOREGROUND_MIDDLE);
        }
    }
}