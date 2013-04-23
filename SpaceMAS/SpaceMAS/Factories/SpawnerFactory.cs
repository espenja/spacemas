using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpaceMAS.Models.Components.ImpactEffects;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Settings;
using SpaceMAS.Utils;

namespace SpaceMAS.Factories
{
    class SpawnerFactory
    {
        private static SpawnerFactory _instance;
        public static SpawnerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpawnerFactory();
                }
                return _instance;
            }

        }
        private readonly List<EnemyType> _enemyTypes = new List<EnemyType>();

        public Spawner CreateSpawner()
        {
            Vector2 position = new Vector2(ThreadSafeRandom.Next(0, GeneralSettings.screenWidth), ThreadSafeRandom.Next(0, GeneralSettings.screenHeight));
            List <Enemy> enemies = new List<Enemy>();
            int enemyTypeIndex = ThreadSafeRandom.Next(_enemyTypes.Count);
            int nofEnemies = ThreadSafeRandom.Next(21);
            for (int i = 0; i < nofEnemies; i++)
            {
                enemies.Add(new Enemy(_enemyTypes[enemyTypeIndex], new DisableEffect(1000)));
            }
            Console.WriteLine(enemies.Count);
            return new Spawner(ThreadSafeRandom.Next(1, 10) * 1000, ThreadSafeRandom.Next(1, 10) * 300, position, enemies);
        }
        
        private SpawnerFactory()
        {
            var contentManager = GameServices.GetService<ContentManager>();
            var directoryInfo = new DirectoryInfo(contentManager.RootDirectory + "\\" + GeneralSettings.EnemyPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            CreateTypes(fileInfos);
        }

        private void CreateTypes(FileInfo[] fileInfos)
        {
            foreach (FileInfo fileInfo in fileInfos)
            {
                CreateType(File.ReadAllLines(fileInfo.FullName));
            }
        }

        private void CreateType(string[] enemyInfo)
        {
            var type = new EnemyType();

            foreach (var s in enemyInfo)
            {
                var header = s.Split(':')[0];
                var info = s.Split(':')[1];
                switch (header)
                {
                    case "id":
                        type.Id = info;
                        break;
                    case "name":
                        type.Name = info;
                        break;
                    case "damage":
                        type.Damage = Convert.ToInt32(info);
                        break;
                    case "speed":
                        type.Speed = Convert.ToInt32(info);
                        break;
                    case "health":
                        type.Health = Convert.ToInt32(info);
                        break;
                    case "bounty":
                        type.Bounty = Convert.ToInt32(info);
                        break;
                }
            }
            _enemyTypes.Add(type);
        }
    }
}
