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
using System;

namespace SpaceMAS.Level {

    public class Level {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Spawner> Spawners { get; private set; }
        public StateProvider StateProvider;
        public LevelIntro LevelIntro;
        public float LevelPlayingTime { get; private set; }

        //Two lists because when iterating through and updating all the objects, new objects can be added to the list
        private List<GameObject> SafeToIterate { get; set; }
        public List<GameObject> AllDrawableGameObjects { get; set; }


        public Level() {
            Spawners = new List<Spawner>();
            SafeToIterate = new List<GameObject>();
            AllDrawableGameObjects = new List<GameObject>();
        }

        public void Initialize() {
            LevelIntro = new LevelIntro(this);
            AllDrawableGameObjects.AddRange(GameServices.GetService<List<Player>>());
        }

        public void AddSpawner(Spawner spawner) {
            Spawners.Add(spawner);
        }

        public void AddEnemy(Enemy enemy, int spawnerId) {
            Spawner spawner = Spawners.Find(p => p.Id == spawnerId);
            spawner.AddEnemy(enemy);
        }

        //Removes gameobjects that is outside of the screen and dead killablegameobjects
        private void CleanUp()
        {
            foreach (GameObject go in SafeToIterate)
            {
                if (go.Position.X < 0 || go.Position.X > GeneralSettings.screenWidth + go.Width
                    || go.Position.Y < 0 || go.Position.Y > GeneralSettings.screenHeight + go.Height)
                {
                    AllDrawableGameObjects.Remove(go);
                }
                if (go is KillableGameObject)
                {
                    if (((KillableGameObject)go).Dead)
                    {
                        AllDrawableGameObjects.Remove(go);
                    }
                }
            }
        }

        public void Update(GameTime gameTime) {
            if (LevelIntro.IntroRunning)
                LevelIntro.Update(gameTime);
            else
            {
                LevelPlayingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                SafeToIterate = new List<GameObject>();
                SafeToIterate.AddRange(AllDrawableGameObjects);
                AllDrawableGameObjects = new List<GameObject>();
                foreach (GameObject go in SafeToIterate)
                {
                    go.Update(gameTime);
                }

                foreach (GameObject go in SafeToIterate)
                {
                    foreach (GameObject go2 in SafeToIterate)
                    {
                        if (go is Bullet && !(go2 is Bullet) && go2 is KillableGameObject && go != go2 && go.IntersectPixels(go2))
                        {
                            ((Bullet)go).OnImpact(go2);
                        }
                    }
                }
                AllDrawableGameObjects.AddRange(SafeToIterate);

                foreach (Spawner spawner in Spawners)
                {
                    spawner.Update(LevelPlayingTime, gameTime);
                }

                CleanUp();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if(LevelIntro.IntroRunning)
                LevelIntro.Draw(spriteBatch);
            else
            {
                foreach (GameObject go in SafeToIterate)
                    go.Draw(spriteBatch);
            }
        }
    }
}
