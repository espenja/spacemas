using System.Collections.Generic;

namespace SpaceMAS.Models.Enemies.Spawner {
    public class EnemySpawner : GameObject {
        public List<Enemy> Enemies { get; private set; }
    }
}
