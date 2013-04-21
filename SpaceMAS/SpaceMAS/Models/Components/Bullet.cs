﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Level;
using SpaceMAS.Utils;
using SpaceMAS.Models.Players;
using SpaceMAS.Utils.Collition;

namespace SpaceMAS.Models.Components
{
    public class Bullet : KillableGameObject
    {
        public List<BulletListener> Listeners { get; set; }
        protected float HealthChange { get; set; }
        protected BulletEffect Effect { get; set; }
        public bool isVisible { get; set; }
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
            HealthChange = CopyBullet.HealthChange;
            Effect = CopyBullet.Effect.Clone();
            isVisible = true;
            Texture = CopyBullet.Texture;
            TravelSpeed = CopyBullet.TravelSpeed;
        }

        public override void Update(GameTime gameTime) {
            CheckCollide();
            Move(gameTime);
            base.Update(gameTime);
        }

        private void CheckCollide() {
            Level.Level level = GameServices.GetService<LevelController>().CurrentLevel;
            List<GameObject> GameObjectsNearby = level.QuadTree.retrieve(new List<GameObject>(), this);

            foreach (GameObject gameObject in GameObjectsNearby)
            {
                if (gameObject is KillableGameObject &&
                    !(gameObject is Player) &&
                    !(gameObject is Bullet) &&
                    gameObject.IntersectPixels(this))
                {

                    OnImpact(gameObject);
                }
            }

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
            if (isVisible) base.Draw(spriteBatch);
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
