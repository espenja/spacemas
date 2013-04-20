using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Settings;
using SpaceMAS.Models.Players;
using SpaceMAS.Utils;
using SpaceMAS.Level;

namespace SpaceMAS.Models.Components
{
    class Weapon
    {
        protected float Firerate { get; set; }
        protected Bullet BulletType { get; set; }
        protected int NofBullets { get; set; }
        protected float TimeSinceLastShot { get; set; }
        protected Player Owner { get; set; }
        public bool isDisabled { get; set; }


        public Weapon(Bullet BulletType, float Firerate, int NofBullets, Player Owner)
        {
            this.BulletType = BulletType;
            this.Firerate = Firerate;
            this.NofBullets = NofBullets;
            this.Owner = Owner;
            TimeSinceLastShot = Firerate;
        }

        public void Shoot(GameTime gameTime)
        {
            TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!isDisabled && TimeSinceLastShot >= Firerate && NofBullets > 0)
            {
                NofBullets--;
                Bullet NewBullet = new Bullet(BulletType);
                NewBullet.Scale = 0.1f;
                NewBullet.Velocity = new Vector2((float)Math.Cos(Owner.Rotation), (float)Math.Sin(Owner.Rotation)) * NewBullet.TravelSpeed;
                NewBullet.Position = Owner.Position + new Vector2((float)Math.Cos(Owner.Rotation), (float)Math.Sin(Owner.Rotation)) * 40f;
                NewBullet.Listeners.Add(Owner);
                LevelController lcon = GameServices.GetService<LevelController>();
                lcon.CurrentLevel.AllDrawableGameObjects.Add(NewBullet);
                TimeSinceLastShot = 0;

            }
        }
    }
}
