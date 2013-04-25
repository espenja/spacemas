using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Utils;

namespace SpaceMAS.Factories {
    internal class SpawnerFactory {

        private static SpawnerFactory instance;

        public static SpawnerFactory Instance {
            get { return instance ?? (instance = new SpawnerFactory()); }
        }

        private SpawnerFactory() { }

        public Spawner CreateSpawnerWithRandomPosition() {
            var graphicsDevice = GameServices.GetService<GraphicsDevice>();

            var width = graphicsDevice.Viewport.Width;
            var height = graphicsDevice.Viewport.Height;
            var random = new Random();

            return  new Spawner(1000, 1000, new Vector2(random.Next(width), random.Next(height)));
        }

        public Spawner CreateSpawnerWithEnemies(string enemyId, int amount) {
            var spawner = new Spawner(1000, 1000, new Vector2(100, 100));
            spawner.Enemies.AddRange(EnemyFactory.Instance.CreateEnemies(enemyId, amount));
            return spawner;
        }

        public Spawner CreateSpawnerWithRandomPositionAndEnemies(string enemyId, int amount) {
            var spawner = CreateSpawnerWithRandomPosition();
            spawner.Enemies.AddRange(EnemyFactory.Instance.CreateEnemies(enemyId, amount));
            return spawner;
        }
    }
}
