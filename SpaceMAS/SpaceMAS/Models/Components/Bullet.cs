using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Level;
using SpaceMAS.Models.Components.ImpactEffects;
using SpaceMAS.Utils;
using SpaceMAS.Models.Players;

namespace SpaceMAS.Models.Components {
    public class Bullet : KillableGameObject {
        public List<IBulletListener> Listeners { get; set; }
        public float HealthChange { get; set; }
        protected IImpactEffect Effect { get; set; }
        public bool isVisible { get; set; }
        public float TravelSpeed { get; set; }

        public Bullet(float HealthChange, float TravelSpeed, IImpactEffect Effect, Texture2D Texture) {
            this.HealthChange = HealthChange;
            this.Effect = Effect;
            this.Texture = Texture;
            this.TravelSpeed = TravelSpeed;
        }

        public Bullet(Bullet CopyBullet) {
            Listeners = new List<IBulletListener>();
            HealthChange = CopyBullet.HealthChange;
            Effect = CopyBullet.Effect.Clone();
            isVisible = true;
            Texture = CopyBullet.Texture;
            TravelSpeed = CopyBullet.TravelSpeed;
            Scale = 0.4f;
        }

        public override void Update(GameTime gameTime) {
            if (!Dead) {
                CheckCollide();
                Move(gameTime);
            }
            base.Update(gameTime);
        }

        private void CheckCollide() {
            Level.Level level = GameServices.GetService<LevelController>().CurrentLevel;
            List<GameObject> GameObjectsNearby = level.QuadTree.retrieve(new List<GameObject>(), this);

            foreach (GameObject gameObject in GameObjectsNearby) {
                if (gameObject is KillableGameObject &&
                    !(gameObject is Bullet) &&
                    gameObject.IntersectPixels(this)) {

                    OnImpact(gameObject);
                }
            }
        }

        private void Move(GameTime gameTime) {
            Position += Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void OnImpact(GameObject victim) {
            if (victim is Enemy.Enemy) {
                ((Enemy.Enemy) victim).HealthPoints += HealthChange;
                Effect.OnImpact(victim);
                Die();
            } else if (victim is Player)
            {
                ((Player)victim).HealthPoints -= HealthChange;
                Die();
            }
            foreach (IBulletListener listener in Listeners) {
                listener.BulletImpact(this, victim);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible) base.Draw(spriteBatch);
        }

        public override void Die() {
            Dead = true;
        }

        public override void Disable() {

        }

        public override void Enable() {

        }
    }
}
