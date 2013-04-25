using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceMAS.Models.Players;
using SpaceMAS.Utils;
using SpaceMAS.Level;

namespace SpaceMAS.Models.Components
{
    public class Weapon
    {
        protected float Firerate { get; set; }
        public Bullet BulletType { get; set; }
        protected float TimeSinceLastShot { get; set; }
        protected Player Owner { get; set; }
        public bool IsDisabled { get; set; }

        public Weapon(Bullet bulletType, float firerate, Player owner)
        {
            BulletType = bulletType;
            Firerate = firerate;
            Owner = owner;
            TimeSinceLastShot = firerate;
        }

        public void Shoot(GameTime gameTime)
        {
            TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!IsDisabled && TimeSinceLastShot >= Firerate)
            {
                Bullet newBullet = new Bullet(BulletType);
                newBullet.Scale = BulletType.Scale;
                newBullet.Velocity = new Vector2((float)Math.Cos(Owner.Rotation), (float)Math.Sin(Owner.Rotation)) * newBullet.TravelSpeed;
                newBullet.Position = Owner.Position + new Vector2((float)Math.Cos(Owner.Rotation), (float)Math.Sin(Owner.Rotation)) * 40f;
                newBullet.Listeners.Add(Owner);
                LevelController lcon = GameServices.GetService<LevelController>();
                lcon.CurrentLevel.AllDrawableGameObjects.Add(newBullet);
                TimeSinceLastShot = 0;

            }
        }
    }
}
