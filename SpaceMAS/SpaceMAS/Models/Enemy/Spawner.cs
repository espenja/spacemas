using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace SpaceMAS.Models.Enemy {
    public class Spawner : GameObject {

        public int Id { get; set; }
        public long SpawnTime { get; set; }
        public long SpawnRate { get; set; }

        public List<Enemy> Enemies { get; set; }

        public Spawner(int Id, long SpawnTime, long SpawnRate) {
            this.Id = Id;
            this.SpawnRate = SpawnRate;
            this.SpawnTime = SpawnTime;
            Enemies = new List<Enemy>();
        }

        public Spawner()
        {
            Enemies = new List<Enemy>();
        }

        //public new Vector2 Position {
            //get { return base.Position; }
          //  set { base.Position = value; }
        //}

        public void AddEnemy(Enemy enemy) {
            Enemies.Add(enemy);
        }

        public Enemy GetNext()
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
