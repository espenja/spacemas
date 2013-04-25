using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Settings;
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
            var contentManager = GameServices.GetService<ContentManager>();

            var width = graphicsDevice.Viewport.Width;
            var height = graphicsDevice.Viewport.Height;
            var random = new Random();

            var spawner = new Spawner(1000, 1000, new Vector2(random.Next(width), random.Next(height)));
            spawner.Texture = contentManager.Load<Texture2D>(GeneralSettings.TexturesPath + "spawner1");

            return spawner;
        }

        public Spawner CreateSpawnerWithEnemies(string enemyId, int amount) {
            var contentManager = GameServices.GetService<ContentManager>();
            var spawner = new Spawner(1000, 1000, new Vector2(100, 100));
            spawner.Texture = contentManager.Load<Texture2D>(GeneralSettings.TexturesPath + "spawner1");

            spawner.Enemies.AddRange(EnemyFactory.Instance.CreateEnemies(enemyId, amount));
            return spawner;
        }

        public Spawner CreateSpawnerWithRandomPositionAndEnemies(string enemyId, int amount) {
            var spawner = CreateSpawnerWithRandomPosition();
            spawner.Enemies.AddRange(EnemyFactory.Instance.CreateEnemies(enemyId, amount));
            return spawner;
        }

        public Spawner CreateSpawnerWIthRandomPositionAndRandomEnemies(int amount) {
            var spawner = CreateSpawnerWithRandomPosition();
            spawner.Enemies.AddRange(EnemyFactory.Instance.CreateRandomEnemies(amount));
            return spawner;
        }
    }
}
