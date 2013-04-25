using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceMAS.Level;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Enemy {
    public class Spawner : GameObject {

        public long SpawnTime { get; set; }
        public long SpawnRate { get; set; }
        private float TimeSinceLastSpawn { get; set; }

        public List<Enemy> Enemies { get; set; }

        public Spawner(long spawnTime, long spawnRate, Vector2 position) {
            SpawnRate = spawnRate;
            SpawnTime = spawnTime;
            Position = position;
            Enemies = new List<Enemy>();
        }

        public Spawner(long spawnTime, long spawnRate, Vector2 position, List<Enemy> enemies)
            : this(spawnTime, spawnRate, position) {
            Enemies = enemies;
        }

        public void Update(float levelPlayingTime, GameTime gameTime) {
            if (SpawnTime <= levelPlayingTime) {
                TimeSinceLastSpawn += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TimeSinceLastSpawn >= SpawnRate) {
                    Enemy nextEnemy = GetNext();
                    if (nextEnemy != null) {
                        LevelController lcon = GameServices.GetService<LevelController>();
                        nextEnemy.Position = new Vector2(Position.X, Position.Y);
                        lcon.CurrentLevel.AllDrawableGameObjects.Add(nextEnemy);
                        Console.WriteLine("Spawning enemy at pos " + nextEnemy.Position);
                    }
                    TimeSinceLastSpawn = 0f;
                }
            }

            foreach (Enemy enemy in Enemies) {
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            //foreach (Enemy enemy in Enemies) {
            //    enemy.Draw(spriteBatch);
            //}
        }

        //public new Vector2 Position {
        //get { return base.Position; }
        //  set { base.Position = value; }
        //}

        private Enemy GetNext() {
            if (Enemies.Count > 0) {
                Enemy e = Enemies[0];
                Enemies.Remove(e);
                return e;
            }
            return null;
        }
    }
}
