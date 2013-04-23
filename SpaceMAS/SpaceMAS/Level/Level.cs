using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Level.Background;
using SpaceMAS.Models;
using SpaceMAS.Models.Enemy;
using SpaceMAS.Models.Players;
using SpaceMAS.State;
using SpaceMAS.Utils;
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

        //Two lists because when iterating through and updating all the objects, new objects can be added to the list
        public List<GameObject> SafeToIterate { get; set; }
        public List<GameObject> AllDrawableGameObjects { get; set; }

        public QuadTree QuadTree { get; private set; }
        private Starfield StarField;


        public Level() {
            var graphicsDevice = GameServices.GetService<GraphicsDevice>();
            QuadTree = new QuadTree(0, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            Spawners = new List<Spawner>();
            SafeToIterate = new List<GameObject>();
            AllDrawableGameObjects = new List<GameObject>();

            StarField = new Starfield();
        }

        public void Initialize() {
            LevelIntro = new LevelIntro(this);
            AllDrawableGameObjects.AddRange(GameServices.GetService<List<Player>>());
        }

        public void AddSpawner(Spawner spawner) {
            Spawners.Add(spawner);
        }

        /**public void AddEnemy(Enemy enemy, int spawnerId) {
            Spawner spawner = Spawners.Find(p => p.Id == spawnerId);
            spawner.AddEnemy(enemy);
        }**/

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
            StarField.Update(gameTime);
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

            AllDrawableGameObjects.AddRange(SafeToIterate);

            foreach (Spawner spawner in Spawners) {
                spawner.Update(LevelPlayingTime, gameTime);
            }

            if (AllDrawableGameObjects.FindAll(o => o is Player).Count == AllDrawableGameObjects.Count + Spawners.Count) {
                GameServices.GetService<LevelController>().GoToNextLevel();
            }

            CleanUp();
        }

        public void Draw(SpriteBatch spriteBatch) {
            StarField.Draw(spriteBatch);
            QuadTree.Draw(spriteBatch);

            foreach (GameObject go in AllDrawableGameObjects)
                go.Draw(spriteBatch);
        }
    }
}
