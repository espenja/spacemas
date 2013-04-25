using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;

namespace SpaceMAS.Models {
    public abstract class KillableGameObject : GameObject {

        public HealthBar HealthBar { get; set; }
        public float MaxHealthPoints { get; set; }
        private float healthPoints;

        public float HealthPoints {
            get { return healthPoints; }
            set {
                healthPoints = value;
                if (healthPoints <= 0) Die();
                if (healthPoints > MaxHealthPoints) healthPoints = MaxHealthPoints;
            }
        }

        protected KillableGameObject() {
            HealthBar = new HealthBar(this);
        }

        public bool Dead { get; set; }
        public bool Disabled { get; set; }

        public abstract void Die();
        public abstract void Disable();
        public abstract void Enable();

        public override void Update(GameTime gameTime) {
            HealthBar.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            HealthBar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
