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
        private List<Enemy> SpawnedEnemies { get; set; } 

        public Spawner(long spawnTime, long spawnRate, Vector2 position) {
            SpawnRate = spawnRate;
            SpawnTime = spawnTime;
            Position = position;
            Enemies = new List<Enemy>();
            SpawnedEnemies = new List<Enemy>();
        }

        public Spawner(long spawnTime, long spawnRate, Vector2 position, List<Enemy> enemies)
            : this(spawnTime, spawnRate, position) {
            Enemies = enemies;
        }

        public void Update(float levelPlayingTime, GameTime gameTime) {

            if (SpawnTime <= levelPlayingTime) {
                TimeSinceLastSpawn += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                if (TimeSinceLastSpawn >= SpawnRate) {
                    var nextEnemy = GetNext();

                    if (nextEnemy != null) {

                        Enemies.Remove(nextEnemy);
                        var lcon = GameServices.GetService<LevelController>();
                        nextEnemy.Position = new Vector2(Position.X, Position.Y);
                        lcon.CurrentLevel.AllDrawableGameObjects.Add(nextEnemy);
                        SpawnedEnemies.Add(nextEnemy);
                        Console.WriteLine(DateTime.Now.Ticks + "Spawning enemy at pos " + nextEnemy.Position);
                    }
                    TimeSinceLastSpawn = 0f;
                }
            }

            foreach (Enemy enemy in SpawnedEnemies) {
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }

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
