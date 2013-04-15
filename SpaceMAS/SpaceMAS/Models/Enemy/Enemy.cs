using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;

namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        private HealthBar HealthBar { get; set; }

        public new float Health {
            get { return base.Health; }
            set { base.Health = value; }
        }

        public Enemy() {
            Rotation = 0f;
            AccelerationRate = 300f;
            NaturalDecelerationRate = 0f;
            RotationRate = 2f;

            MaxHealthPoints = 25;
            HealthPoints = 25;

            HealthBar = new HealthBar(this);
        }

        public override void Update(GameTime gameTime) {
            Move(gameTime);
            HealthBar.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime) {
            //Enemies should perhaps automatically target the nearest Player?
        }

    }
}