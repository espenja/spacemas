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

        public Spawner(long spawnTime, long spawnRate, Vector2 position, List<Enemy> enemies)
        {
            SpawnRate = spawnRate;
            SpawnTime = spawnTime;
            Position = position;
            Enemies = enemies;
        }

        public void Update(float levelPlayingTime, GameTime gameTime)
        {
            if (SpawnTime <= levelPlayingTime)
            {
                TimeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TimeSinceLastSpawn >= SpawnRate)
                {
                    Enemy nextEnemy = GetNext();
                    if (nextEnemy != null)
                    {
                        LevelController lcon = GameServices.GetService<LevelController>();
                        nextEnemy.Position = new Vector2(Position.X, Position.Y);
                        lcon.CurrentLevel.AllDrawableGameObjects.Add(nextEnemy);
                    }
                    TimeSinceLastSpawn = 0f;
                }
            }

            base.Update(gameTime);
        }

        //public new Vector2 Position {
            //get { return base.Position; }
          //  set { base.Position = value; }
        //}

        public void AddEnemy(Enemy enemy) {
            Enemies.Add(enemy);
        }

        private Enemy GetNext()
        {
            if (Enemies.Count > 0)
            {
                Enemy e = Enemies[0];
                Enemies.Remove(e);
                return e;
            }
            return null;
        }
    }
}
