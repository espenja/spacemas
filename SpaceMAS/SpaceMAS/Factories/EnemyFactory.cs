using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components.ImpactEffects;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Settings;
using SpaceMAS.Utils;

namespace SpaceMAS.Factories {
    internal class EnemyFactory {

        private static EnemyFactory instance;
        private readonly List<Enemy> enemyTemplates = new List<Enemy>();
        private readonly ContentManager contentManager;

        public static EnemyFactory Instance {
            get { return instance ?? (instance = new EnemyFactory()); }
        }

        private EnemyFactory() {
            contentManager = GameServices.GetService<ContentManager>();
            createTemplateEnemies();
        }

        public Enemy CreateEnemy(string enemyId) {
            return new Enemy().Clone(enemyTemplates.Find(e => e.EnemyID == enemyId));
        }

        public Enemy CreateRandomEnemy() {
            var random = new Random();
            return new Enemy().Clone(enemyTemplates[random.Next(0, enemyTemplates.Count)]);
        }

        public List<Enemy> CreateEnemies(string enemyId, int amount) {
            var createdEnemies = new List<Enemy>();

            for (var i = 0; i < amount; i++) {
                createdEnemies.Add(CreateEnemy(enemyId));
            }
            return createdEnemies;
        }

        public List<Enemy> CreateRandomEnemies(int amount) {
            var createdEnemies = new List<Enemy>();

            for (var i = 0; i < amount; i++) {
                createdEnemies.Add(CreateRandomEnemy());
            }
            return createdEnemies;
        } 

        private void createTemplateEnemies() {
            var fileInfos = new DirectoryInfo(contentManager.RootDirectory + "\\" + GeneralSettings.EnemyPath).GetFiles();

            foreach (var fileInfo in fileInfos) {
                CreateEnemy(File.ReadAllLines(fileInfo.FullName));
            }
        }

        private void CreateEnemy(IEnumerable<string> enemyInfo) {

            var enemy = new Enemy();

            foreach (var line in enemyInfo) {
                var key = line.Split(':')[0];
                var value = line.Split(':')[1];

                switch (key) {
                    case "id":
                        enemy.Texture = contentManager.Load<Texture2D>(GeneralSettings.TexturesPath + value.Trim());
                        enemy.EnemyID = value.Trim();
                        break;
                    case "name":
                        enemy.Name = value.Trim();
                        break;
                    case "damage":
                        enemy.Damage = Convert.ToInt32(value);
                        break;
                    case "speed":
                        enemy.Speed = Convert.ToInt32(value);
                        break;
                    case "health":
                        enemy.MaxHealthPoints = float.Parse(value);
                        enemy.HealthPoints = float.Parse(value);
                        break;
                    case "bounty":
                        enemy.Bounty = Convert.ToInt32(value);
                        break;
                }
            }

            enemy.ImpactEffect = new DisableEffect(1000);
            enemyTemplates.Add(enemy);
        }
    }
}
