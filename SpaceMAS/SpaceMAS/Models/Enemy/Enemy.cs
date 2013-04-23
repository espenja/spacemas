using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Models.Players;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        private HealthBar HealthBar { get; set; }
        public int Bounty { get; set; }

        //public new float Health {
        //    get { return base.Health; }
        //    set { base.Health = value; }
        //}

        public Enemy() {
            Rotation = 0f;
            AccelerationRate = 100f;
            RotationRate = 5f;
            MaxHealthPoints = 25;
            HealthPoints = 25;
            Bounty = 10;

            HealthBar = new HealthBar(this);
        }

        public override void Update(GameTime gameTime)
        {
            Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!Disabled)
                Move(gameTime);
            HealthBar.Update(gameTime);

            base.Update(gameTime); 
        }

        public override void Draw(SpriteBatch spriteBatch) {
            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime) {

            var players = GameServices.GetService<List<Player>>();
            Player closestTarget = null;
            var targetDistance = float.MaxValue;

            foreach (var player in players)
            {
                var distance = Math.Abs((float)Math.Sqrt(Math.Pow(player.Position.X - Position.X, 2) 
                    + Math.Pow((player.Position.Y - player.Position.Y), 2)));
                if (distance < targetDistance || closestTarget == null)
                {
                    targetDistance = distance;
                    closestTarget = player;
                }
            }

            var closestPosDistance = AccelerationRate / 20;
            if (closestTarget != null) {
                if (closestTarget.Position.X - Position.X > closestPosDistance)
                {
                    Position = new Vector2(Position.X + AccelerationRate * (float)gameTime.ElapsedGameTime.TotalSeconds,
                        Position.Y);
                }
                else if (closestTarget.Position.X - Position.X < -closestPosDistance)
                {
                    Position = new Vector2(Position.X - AccelerationRate * (float)gameTime.ElapsedGameTime.TotalSeconds,
                        Position.Y);
                }
                if (closestTarget.Position.Y - Position.Y > closestPosDistance)
                {
                    Position = new Vector2(Position.X, Position.Y + AccelerationRate * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else if (closestTarget.Position.Y - Position.Y < -closestPosDistance)
                {
                    Position = new Vector2(Position.X, Position.Y - AccelerationRate * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            
        }


        public override void Disable()
        {
            Disabled = true;
        }

        public override void Enable()
        {
            Disabled = false;
        }

        public override void Die()
        {
            Dead = true;
        }
    }
}