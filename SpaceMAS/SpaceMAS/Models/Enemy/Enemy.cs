using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        private HealthBar HealthBar { get; set; }
        private Player.Player target;

        public new float Health {
            get { return base.Health; }
            set { base.Health = value; }
        }

        public Enemy(Vector2 position) {
            Position = position;
            Rotation = 0f;
            AccelerationRate = 300f;
            NaturalDecelerationRate = 0f;
            RotationRate = 2f;

            MaxHealthPoints = 25;
            HealthPoints = 25;

            //Find the nearest Player to target
            target = GetClosestPlayer();

            HealthBar = new HealthBar(this);
        }

        public override void Update(GameTime gameTime) {
            //If target dies, try to find a new target (needs optimalization if we utilize disabled player spaceships)
            if (target.Dead)
                target = GetClosestPlayer();

            Move(gameTime);
            HealthBar.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        //Always move towards the target assigned at spawn
        private void Move(GameTime gameTime) {
            //The angle between the enemy ship and its target player. Used in the Draw method (overloaded type of Draw that accepts radians as rotation)
            var angle = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

            var ElapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var NewVelocity = new Vector2(Velocity.X, Velocity.Y);

            //TODO: Make the enemy vessel rotate and accelerate towards the player target (does not work atm)
            if (Rotation <= angle)
                Rotation += RotationRate*ElapsedGameTime;
            if (Rotation > angle)
                Rotation -= RotationRate*ElapsedGameTime;

        }

        //Returns the player closest to the enemy when it spawns
        private Player.Player GetClosestPlayer() {
            var players = GameServices.GetService<List<Player.Player>>();
            Player.Player closest = null;
            var shortest = float.MaxValue;

            foreach (var player in players) {
                var temp = Vector2.Distance(player.Position, Position);
                if (!(temp < shortest)) continue;
                shortest = temp;
                closest = player;
            }

            return closest;
        }

    }
}