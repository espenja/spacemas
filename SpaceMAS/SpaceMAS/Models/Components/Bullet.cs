using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Interfaces;
using SpaceMAS.Level;
using SpaceMAS.Models.Components.ImpactEffects;
using SpaceMAS.Utils;
using SpaceMAS.Models.Players;

namespace SpaceMAS.Models.Components {
    public class Bullet : KillableGameObject {
        public List<IBulletListener> Listeners { get; set; }
        protected float HealthChange { get; set; }
        protected IImpactEffect Effect { get; set; }
        public bool isVisible { get; set; }
        public float TravelSpeed { get; set; }

        public Bullet(BulletType bulletType) {
            Listeners = new List<IBulletListener>();
            HealthChange = bulletType.HealthChange;
            Effect = bulletType.Effect.Clone();
            isVisible = true;
            Texture = bulletType.Texture;
            TravelSpeed = bulletType.TravelSpeed;
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
                    !(gameObject is Player) &&
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
            if (victim is KillableGameObject) {
                ((KillableGameObject) victim).HealthPoints += HealthChange;
                Die();
            }
            Effect.OnImpact(victim);
            foreach (IBulletListener listener in Listeners) {
                listener.BulletImpact(this, victim);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
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
