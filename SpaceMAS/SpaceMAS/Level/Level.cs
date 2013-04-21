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
using SpaceMAS.Utils.Collition;

namespace SpaceMAS.Level {

    public class Level {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Spawner> Spawners { get; private set; }
        public StateProvider StateProvider;
        public LevelIntro LevelIntro;
        public float LevelPlayingTime { get; private set; }
        private int updatesSinceLastCollDet;

        //Two lists because when iterating through and updating all the objects, new objects can be added to the list
        private List<GameObject> SafeToIterate { get; set; }
        public List<GameObject> AllDrawableGameObjects { get; set; }

        private static readonly object DrawableGameObjectsLock = new object();

        public QuadTree QuadTree { get; private set; }


        public Level() {

            GraphicsDevice graphicsDevice = GameServices.GetService<GraphicsDevice>();

            QuadTree = new QuadTree(0, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
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

        private void CleanUp() {
            //Remove objects outside the screen
            AllDrawableGameObjects.RemoveAll(go => (go.Position.X < 0 || go.Position.X > GeneralSettings.screenWidth + go.Width
                                                    || go.Position.Y < 0 || go.Position.Y > GeneralSettings.screenHeight + go.Height));

            //remove killed objts
            AllDrawableGameObjects.RemoveAll(o => o is KillableGameObject && ((KillableGameObject) o).Dead);

            //Remove spawner if all its enemies has spawned
            Spawners.RemoveAll(s => s.Enemies.Count == 0);



        }

        public void Update(GameTime gameTime) {

            if (LevelIntro.IntroRunning) {
                LevelIntro.Update(gameTime);
                return;
            }

            QuadTree.clear();

            foreach (var gameObject in AllDrawableGameObjects) {
                QuadTree.insert(gameObject);
            }

            LevelPlayingTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            SafeToIterate = new List<GameObject>();
            SafeToIterate.AddRange(AllDrawableGameObjects);
            AllDrawableGameObjects = new List<GameObject>();


            foreach (GameObject go in SafeToIterate) {
                go.Update(gameTime);
            }

            //Only check detection ever 5th update
            //if (updatesSinceLastCollDet == 5) {
                //updatesSinceLastCollDet = 0;
                //foreach (GameObject go in SafeToIterate) {
                //    foreach (GameObject go2 in SafeToIterate) {
                //        if (go is Bullet && !(go2 is Bullet) && go2 is KillableGameObject && !(go2 is Player) && go != go2 && go.IntersectPixels(go2)) {
                //            ((Bullet) go).OnImpact(go2);
                //        }
                //    }
                //}
            //}
            //else updatesSinceLastCollDet++;

            AllDrawableGameObjects.AddRange(SafeToIterate);

            foreach (Spawner spawner in Spawners) {
                spawner.Update(LevelPlayingTime, gameTime);
            }

            CleanUp();
        }

        public void Draw(SpriteBatch spriteBatch) {
            QuadTree.Draw(spriteBatch);
            if (LevelIntro.IntroRunning)
                LevelIntro.Draw(spriteBatch);
            else {
                foreach (GameObject go in SafeToIterate)
                    go.Draw(spriteBatch);
            }
        }
    }
}