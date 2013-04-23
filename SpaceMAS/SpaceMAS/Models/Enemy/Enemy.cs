﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Level;
using SpaceMAS.Models.Components;
using SpaceMAS.Models.Players;
using SpaceMAS.Utils;
using SpaceMAS.State;

namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        private HealthBar HealthBar { get; set; }
        public int Bounty { get; set; }
        private bool _isDiffBoosted;
        public int ImpactDamage { get; set; }

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
            ImpactDamage = 1;
            _isDiffBoosted = false;

            HealthBar = new HealthBar(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isDiffBoosted) {
            switch (StateProvider.Instance.State)
                {
                    case GameState.PLAYING_EASY:
                        break;
                    case GameState.PLAYING_NORMAL:
                        MaxHealthPoints *= 2;
                        Bounty *= 2;
                        HealthPoints *= 2;
                        break;
                    case GameState.PLAYING_HARD:
                        MaxHealthPoints *= 4;
                        Bounty *= 4;
                        HealthPoints *= 4;
                        break;
                }
            }
            _isDiffBoosted = true;
            Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!Disabled && !Dead)
                Move(gameTime);
                CheckCollide();
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

        private void CheckCollide() {
            Level.Level level = GameServices.GetService<LevelController>().CurrentLevel;
            List<GameObject> gameObjectsNearby = level.QuadTree.retrieve(new List<GameObject>(), this);

            foreach (GameObject gameObject in gameObjectsNearby)
            {
                if (gameObject is Player && gameObject.IntersectPixels(this)) {
                    OnImpact(gameObject);
                }
            }
        }

        public void OnImpact(GameObject victim)
        {
            if (victim is Player)
            {
                ((Player)victim).HealthPoints -= ImpactDamage;
                Console.WriteLine(((Player)victim).HealthPoints);
                Die();
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