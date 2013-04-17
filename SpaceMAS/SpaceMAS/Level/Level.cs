using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;

namespace SpaceMAS.Level {

    public class Level {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Spawner> Spawners { get; private set; }
        public StateProvider StateProvider;
        public LevelIntro LevelIntro;
        public List<Player> Players; 

        public Level() {
            Spawners = new List<Spawner>();
        }

        public void Initialize() {
            LevelIntro = new LevelIntro(this);
            Players = GameServices.GetService<List<Player>>();
        }

        public void AddSpawner(Spawner spawner) {
            Spawners.Add(spawner);
        }

        public void AddEnemy(Enemy enemy, int spawnerId) {
            Spawner spawner = Spawners.Find(p => p.Id == spawnerId);
            spawner.AddEnemy(enemy);
        }

        public void Update(GameTime gameTime) {
            if(LevelIntro.IntroRunning)
                LevelIntro.Update(gameTime);
            else {
                foreach(Player player in Players)
                    player.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(LevelIntro.IntroRunning)
                LevelIntro.Draw(spriteBatch);
            else {
                foreach (Player player in Players)
                    player.Draw(spriteBatch);
            }
        }
    }
}
