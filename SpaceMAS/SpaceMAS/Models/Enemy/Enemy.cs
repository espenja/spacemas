using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SpaceMAS.Factories;
using SpaceMAS.Interfaces;
using SpaceMAS.Level;
using SpaceMAS.Models.Components;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        public String EnemyID { get; set; }
        public String Name { get; set; }
        public int Damage { get; set; }
        public int Bounty { get; set; }
        public int Speed { get; set; }
        public bool IsDiffBoosted { get; set; }

        public IImpactEffect ImpactEffect { get; set; }

        public override void Update(GameTime gameTime) {
            if (!IsDiffBoosted) {
                switch (StateProvider.Instance.State) {
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
            IsDiffBoosted = true;
            Rotation += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (!Disabled && !Dead)
                Move(gameTime);
            CheckCollide();

            base.Update(gameTime);
        }

        private void Move(GameTime gameTime) {
            var players = GameServices.GetService<List<Player>>();
            Player closestTarget = null;
            var targetDistance = float.MaxValue;

            foreach (var player in players) {
                if (player.Dead) continue;

                var distance = (float) Math.Sqrt(Math.Pow(player.Position.X - Position.X, 2)
                                                 + Math.Pow((player.Position.Y - player.Position.Y), 2));

                if (!(distance < targetDistance) && closestTarget != null) continue;

                targetDistance = distance;
                closestTarget = player;
            }

            var closestPosDistance = Speed / 20;
            if (closestTarget == null) return;

            if (closestTarget.Position.X - Position.X > closestPosDistance) {
                Position = new Vector2(Position.X + Speed * (float) gameTime.ElapsedGameTime.TotalSeconds,
                                       Position.Y);
            }
            else if (closestTarget.Position.X - Position.X < -closestPosDistance) {
                Position = new Vector2(Position.X - Speed * (float) gameTime.ElapsedGameTime.TotalSeconds,
                                       Position.Y);
            }
            if (closestTarget.Position.Y - Position.Y > closestPosDistance) {
                Position = new Vector2(Position.X, Position.Y + Speed * (float) gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (closestTarget.Position.Y - Position.Y < -closestPosDistance) {
                Position = new Vector2(Position.X, Position.Y - Speed * (float) gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private void CheckCollide() {
            var level = GameServices.GetService<LevelController>().CurrentLevel;
            var gameObjectsNearby = level.QuadTree.retrieve(new List<GameObject>(), this);

            foreach (var gameObject in gameObjectsNearby.OfType<Player>().Where(gameObject => gameObject.IntersectPixels(this))) {
                OnImpact(gameObject);
            }
        }

        public void OnImpact(GameObject victim) {
            if (!(victim is Player)) return;

            ((Player)victim).HealthPoints -= Damage;
            //if (ImpactEffect != null)
            //    ImpactEffect.OnImpact(victim);

            Die();
        }

        public override void Disable() {
            Disabled = true;
        }

        public override void Enable() {
            Disabled = false;
        }

        public override void Die() {
            Dead = true;
        }

        public Enemy Clone(Enemy enemy) {
            Name = enemy.Name;
            Bounty = enemy.Bounty;
            Damage = enemy.Damage;
            MaxHealthPoints = enemy.MaxHealthPoints;
            HealthPoints = enemy.HealthPoints;
            Bounty = enemy.Bounty;
            ImpactEffect = enemy.ImpactEffect;
            Texture = enemy.Texture;
            Speed = enemy.Speed;
            IsDiffBoosted = false;
            Dead = false;
            Disabled = false;
            EnemyID = enemy.EnemyID;
            Scale = enemy.Scale;
            return this;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {

            spriteBatch.Draw(Textures.Instance.WhitePixel, new Rectangle(BoundingBox.Left, BoundingBox.Top, BoundingBox.Width, 1), Color.White);
            spriteBatch.Draw(Textures.Instance.WhitePixel, new Rectangle(BoundingBox.Left, BoundingBox.Bottom, BoundingBox.Width, 1), Color.White);
            spriteBatch.Draw(Textures.Instance.WhitePixel, new Rectangle(BoundingBox.Left, BoundingBox.Top, 1, BoundingBox.Height), Color.White);
            spriteBatch.Draw(Textures.Instance.WhitePixel, new Rectangle(BoundingBox.Right, BoundingBox.Top, 1, BoundingBox.Height + 1), Color.White);
            base.Draw(spriteBatch);
        }
    }
}