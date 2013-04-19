using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Settings;
using SpaceMAS.Utils;

namespace SpaceMAS.Level {

    public class LevelController {

        public List<Level> Levels { get; private set; }
        public Level CurrentLevel { get; private set; }

        public LevelController() {
            Levels = new List<Level>();
            LoadLevels();
        }

        public void InitializeLevels() {
            foreach(Level level in Levels) {
                level.Initialize();
            }
        }

        public void GoToNextLevel() {

            if (CurrentLevel == null) {
                CurrentLevel = Levels[0];
                return;
            }

            if (Levels.IndexOf(CurrentLevel) + 1 >= Levels.Count)
                CurrentLevel = null;

            CurrentLevel = Levels.Find(l => l.Id == CurrentLevel.Id + 1);
        }

        private void LoadLevels() {
            var contentManager = GameServices.GetService<ContentManager>();
            var directoryInfo = new DirectoryInfo(contentManager.RootDirectory + "\\" + GeneralSettings.LevelPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo fileInfo in fileInfos) {
                CreateLevel(File.ReadAllLines(fileInfo.FullName));
            }
            Levels = Levels.OrderBy(l => l.Id).ToList();
        }

        private void CreateLevel(string[] levelInfo) {

            Level level = null;

            for (int i = 0; i < levelInfo.Length; i++) {

                string line = levelInfo[i];
                string header = "";

                //Lines starting with # is a comment, discard empty lines
                if (line.StartsWith("#") || line.Trim() == "") continue;
                if (line.StartsWith("%")) header = line.Trim().Remove(0, 1);

                var segments = new Dictionary<string, string>();
                
                //Read info about header until we hit the next header
                for (int j = i+1; j < levelInfo.Length; j++) {
                    
                    string innerLine = levelInfo[j];
                    if(innerLine.StartsWith("#") || innerLine.Trim() == "") continue;

                    //We hit a new segment, break and continue reading
                    if (innerLine.StartsWith("%")) {
                        i = j - 1;
                        break;
                    }

                    string[] keyValue = innerLine.Split(':');
                    segments.Add(keyValue[0].ToLower(), keyValue[1]);
                }

                switch (header) {
                    case "level": {
                        level = makeLevel(segments);
                        break;
                    }
                    case "spawner": {
                        if(level != null)
                            level.AddSpawner(makeSpawner(segments));
                        break;
                    }
                    case "enemy": {
                        if(level != null)
                            makeEnemy(segments, level);
                        break;
                    }
                }
            }

            if (level == null) return;

            Levels.Add(level);
        }

        private Level makeLevel(Dictionary<string, string> levelInfo) {

            Level level = new Level();
            level.Name = GetStringInfo(levelInfo, "name", "Level");
            level.Id = GetIntInfo(levelInfo, "id", "Level");

            return level;
        }

        private Spawner makeSpawner(Dictionary<string, string> spawnerInfo) {

            Spawner spawner = new Spawner();

            spawner.Id = GetIntInfo(spawnerInfo, "id", "Spawner");
            spawner.SpawnTime = GetIntInfo(spawnerInfo, "spawn_time", "Spawner");
            spawner.SpawnRate = GetIntInfo(spawnerInfo, "spawn_rate", "Spawner");
            spawner.Position = GetPosition(GetStringInfo(spawnerInfo, "position", "Spawner"));

            return spawner;
        }

        private void makeEnemy(Dictionary<string, string> enemyInfo, Level level) {

            string id = GetStringInfo(enemyInfo, "id", "Enemy");
            int health = GetIntInfo(enemyInfo, "health", "Enemy");
            int spawnerId = GetIntInfo(enemyInfo, "spawner_id", "Enemy");
            int amount = GetIntInfo(enemyInfo, "amount", "Enemy");

            for (int i = 0; i < amount; i++) {
                Enemy enemy = new Enemy();
                enemy.LoadTexture(id);
                enemy.HealthPoints = health;
                enemy.MaxHealthPoints = health;
                level.AddEnemy(enemy, spawnerId);
            }
        }

        private string GetStringInfo(Dictionary<string, string> info, string key, string objectType) {
            string value;
            if (!info.TryGetValue(key, out value))
                throw new ArgumentException(string.Format("A(n) {0} must have a {1}", objectType, key));
            return value;
        }

        private int GetIntInfo(Dictionary<string, string> info, string key, string objectType) {
            string value_str = GetStringInfo(info, key, objectType);
            int value;

            if(!int.TryParse(value_str, out value)) {
                throw new ArgumentException(string.Format("A(n) {0}'s {1} must be an integer.", objectType, key));
            }

            return value;
        }

        private Vector2 GetPosition(string position) {
            if(!position.Contains(","))
                throw new ArgumentException("A Position must have the format 'x,y'");

            string[] positions = position.Split(',');

            if(positions.Length != 2)
                throw new ArgumentException("A Position must have the format 'x,y'");

            int x;
            int y;

            if(!int.TryParse(positions[0], out x) || !int.TryParse(positions[1], out y))
                throw new ArgumentException("A Position must have the format 'x,y' where 'x' and 'y' are integers");

            return new Vector2(x, y);
        }
    }
}
