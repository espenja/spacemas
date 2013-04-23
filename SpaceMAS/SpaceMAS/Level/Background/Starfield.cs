using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Settings;
using SpaceMAS.Utils;

namespace SpaceMAS.Level.Background
{
    public class Starfield {

        private List<Star> Stars;
        private List<Star> SafeToIterate = new List<Star>();
        private List<Star> ToBeRemoved = new List<Star>(); 
        public Texture2D Texture { get; set; }
        private Random Random;

        public Starfield() {
            Stars = new List<Star>();

            ContentManager contentManager = GameServices.GetService<ContentManager>();
            Texture = contentManager.Load<Texture2D>(GeneralSettings.TexturesPath + "star");

            Random = new Random();

            for (int i = 0; i < 300; i++) {
                addNewStar();
            }
        }

        public void RemoveStar(Star star) {
            ToBeRemoved.Add(star);
            addNewStar();
        }

        private void addNewStar() {

            GraphicsDevice graphicsDevice = GameServices.GetService<GraphicsDevice>();

            int x = Random.Next(0, graphicsDevice.Viewport.Width);
            int y = Random.Next(0, graphicsDevice.Viewport.Height);
            float scale = (float) Random.NextDouble();
            float timeToLive = Random.Next(5000, 20000);

            Stars.Add(new Star(this, x, y, scale, timeToLive));
        }

        public void Update(GameTime gameTime) {

            SafeToIterate = new List<Star>();
            SafeToIterate.AddRange(Stars);
            Stars = new List<Star>();

            foreach(Star star in SafeToIterate) {
                star.Update(gameTime);
            }

            Stars.AddRange(SafeToIterate);

            foreach (Star star in ToBeRemoved)
                Stars.Remove(star);
            ToBeRemoved.Clear();
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(Star star in SafeToIterate)
                star.Draw(spriteBatch);
        }

    }
}
