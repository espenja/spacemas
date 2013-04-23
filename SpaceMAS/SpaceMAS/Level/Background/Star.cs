using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Graphics;
using SpaceMAS.Models;

namespace SpaceMAS.Level.Background
{
    public class Star : GameObject {

        private float TimeToLive;
        private float Opacity = 1;
        private Starfield starField;

        public Star(Starfield starfield, int x, int y, float scale, float timeToLive) {

            starField = starfield;
            TimeToLive = timeToLive;
            Texture = starfield.Texture;
            Position = new Vector2(x, y);
            Scale = scale;
        }

        public override void Update(GameTime gameTime)
        {
            Opacity -= (1f / TimeToLive) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(Opacity <= 0) {
                starField.RemoveStar(this);
            }

        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, null, new Color(1, 1, 1, Opacity), 0, Vector2.Zero, Scale, SpriteEffects.None, GameDrawOrder.GAME_LEVEL_BOTTOM);
        }

    }
}
