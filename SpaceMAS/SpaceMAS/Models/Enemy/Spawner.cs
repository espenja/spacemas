using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceMAS.Models.Enemy {
    public class Spawner : GameObject {

        public int Id { get; set; }
        public long SpawnTime { get; set; }
        public long SpawnRate { get; set; }

        public List<Enemy> Enemies { get; set; }

        public Spawner() {
            Enemies = new List<Enemy>();
        }

        public new Vector2 Position {
            get { return base.Position; }
            set { base.Position = value; }
        }

        public void AddEnemy(Enemy enemy) {
            Enemies.Add(enemy);
        }
    }
}
