using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;

namespace SpaceMAS.Level {

    public class Level {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Spawner> Spawners { get; private set; }
        public StateProvider StateProvider;
        public LevelIntro LevelIntro;

        //Two lists because when iterating through and updating all the objects, new objects can be added to the list
        private List<GameObject> SafeToIterateAllGameObjects { get; set; }
        public List<GameObject> AllGameObjects { get; set; }


        public Level() {
            Spawners = new List<Spawner>();
            SafeToIterateAllGameObjects = new List<GameObject>();
        }

        public void Initialize() {
            LevelIntro = new LevelIntro(this);
            SafeToIterateAllGameObjects.AddRange(GameServices.GetService<List<Player>>());
        }

        public void AddSpawner(Spawner spawner) {
            Spawners.Add(spawner);
        }

        public void AddEnemy(Enemy enemy, int spawnerId) {
            Spawner spawner = Spawners.Find(p => p.Id == spawnerId);
            spawner.AddEnemy(enemy);
        }

        //Removes gameobjects that is outside of the screen
        private void CleanUp()
        {
            AllGameObjects = new List<GameObject>();
            AllGameObjects.AddRange(SafeToIterateAllGameObjects);
            foreach (GameObject go in SafeToIterateAllGameObjects)
            {
                if (go.Position.X < 0 || go.Position.X > GeneralSettings.screenWidth
                    || go.Position.Y < 0 || go.Position.Y > GeneralSettings.screenHeight)
                {
                    AllGameObjects.Remove(go);
                }
            }
            SafeToIterateAllGameObjects = new List<GameObject>();
            SafeToIterateAllGameObjects.AddRange(AllGameObjects);
        }

        public void Update(GameTime gameTime) {
            AllGameObjects = new List<GameObject>();
            AllGameObjects.AddRange(SafeToIterateAllGameObjects);
            if (LevelIntro.IntroRunning)
                LevelIntro.Update(gameTime);
            else
            {
                foreach (GameObject go in SafeToIterateAllGameObjects)
                    go.Update(gameTime);
            }
            SafeToIterateAllGameObjects = new List<GameObject>();
            SafeToIterateAllGameObjects.AddRange(AllGameObjects);

            foreach (GameObject go in SafeToIterateAllGameObjects)
            {
                foreach (GameObject go2 in SafeToIterateAllGameObjects)
                {
                    if (go is Bullet && go2 is KillableGameObject && go != go2 && go.IntersectPixels(go2))
                    {
                        ((Bullet)go).OnImpact(go2);
                    }
                }
            }
            CleanUp();
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(LevelIntro.IntroRunning)
                LevelIntro.Draw(spriteBatch);
            else
            {
                foreach (GameObject go in SafeToIterateAllGameObjects)
                    go.Draw(spriteBatch);
            }
        }
    }
}
