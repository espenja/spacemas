using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Graphics;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Components {
    public class HealthBar : GameObject {

        private KillableGameObject Parent { get; set; }
        private float Percent { get; set; }
        private Color HealthColor { get; set; }

        public HealthBar(KillableGameObject parent) {
            Parent = parent;
            Percent = 1;
            Texture = new Texture2D(GameServices.GetService<GraphicsDevice>(), 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime) {

            if(Parent.Dead) {
                //remove object
                if (Parent is Players.Player)
                {
                    //Make player stuff happen
                }
                else
                    Parent.Die();
            }

            float percent = (Parent.HealthPoints / Parent.MaxHealthPoints);

            if (percent > 0.7f)
            {
                if (Percent >= 0.7f)
                    //Texture.SetData(new[] {Color.ForestGreen});
                    HealthColor = Color.LightGreen;
            }
            else if (percent >= 0.4 && percent < 0.7)
            {
                if (Percent >= 0.7 || Percent < 0.4)
                    //Texture.SetData(new[] { Color.OrangeRed });
                    HealthColor = Color.Orange;
            }
            else if (percent < 0.4)
                if (Percent > 0.4)
                    //Texture.SetData(new[] { Color.Red });
                    HealthColor = Color.DarkRed;

            Percent = percent;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Vector2 anchor = Parent.UpperLeftCorner();
            double healthBarWidth = Parent.Width * Percent;

            Drawing.Draw(spriteBatch, Texture, new Rectangle((int)anchor.X, (int)anchor.Y - 10, (int)healthBarWidth, 3), HealthColor, GameDrawOrder.GAME_LEVEL_TOP);
        }
    }
}
