using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMAS.Models.Components
{
    public class Bullet : KillableGameObject
    {
        public List<BulletListener> Listeners { get; set; }
        protected float HealthChange { get; set; }
        protected BulletEffect Effect { get; set; }
        public bool isVisable { get; set; }
        public float TravelSpeed { get; set; }

        public Bullet(float HealthChange, float TravelSpeed, BulletEffect Effect, Texture2D Texture)
        {
            this.HealthChange = HealthChange;
            this.Effect = Effect;
            this.Texture = Texture;
            this.TravelSpeed = TravelSpeed;
        }

        public Bullet(Bullet CopyBullet)
        {
            Listeners = new List<BulletListener>();
            this.HealthChange = CopyBullet.HealthChange;
            this.Effect = CopyBullet.Effect.Clone();
            this.isVisable = true;
            this.Texture = CopyBullet.Texture;
            this.TravelSpeed = CopyBullet.TravelSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            base.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void OnImpact(GameObject Victim)
        {
            foreach (BulletListener Listener in Listeners)
            {
                Listener.BulletImpact(this, Victim);
                
            }

            if (Victim is KillableGameObject)
            {
                ((KillableGameObject)Victim).HealthPoints += HealthChange;
                Die();
            }
            Effect.OnImpact(Victim);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisable) base.Draw(spriteBatch);
        }

        public override void Die()
        {
            Dead = true;   
        }

        public override void Disable()
        {
           
        }

        public override void Enable()
        {
            
        }
    }
}
