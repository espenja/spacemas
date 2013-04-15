using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceMAS.Models.Enemy;

namespace SpaceMAS.Level {

    public class Level {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<Spawner> Spawners { get; private set; }

        public Level() {
            Spawners = new List<Spawner>();
        }

        public void AddSpawner(Spawner spawner) {
            Spawners.Add(spawner);
        }

        public void AddEnemy(Enemy enemy, int spawnerId) {
            Spawner spawner = Spawners.Find(p => p.Id == spawnerId);
            spawner.AddEnemy(enemy);
        }
    }
}
