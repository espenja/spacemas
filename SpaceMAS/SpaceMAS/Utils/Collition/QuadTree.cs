using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using SpaceMAS.Models;

namespace SpaceMAS.Utils.Collition {
    public class QuadTree {
        private const int MaxObjects = 1;
        private const int MaxLevels = 5;

        private int Level { get; set; }
        private List<GameObject> GameObjects { get; set; }
        private Rectangle Bounds { get; set; }
        private QuadTree[] Nodes { get; set; }

        private Texture2D Texture { get; set; }


        public QuadTree(int level, Rectangle bounds) {
            Level = level;
            GameObjects = new List<GameObject>();
            Nodes = new QuadTree[4];
            Bounds = bounds;

            Texture = GameServices.GetService<SpaceMAS>().TextureForDrawingLines;
        }

        public void clear() {

            GameObjects.Clear();

            for (int i = 0; i < Nodes.Count(); i++) {
                if (Nodes[i] == null) continue;
                Nodes[i].clear();
                Nodes[i] = null;
            }
        }

        private void split() {

            int subWidth = Bounds.Width / 2;
            int subHeight = Bounds.Height / 2;
            int x = Bounds.X;
            int y = Bounds.Y;

            Nodes[0] = new QuadTree(Level + 1, new Rectangle(Bounds.Left, Bounds.Top, subWidth, subHeight));
            Nodes[1] = new QuadTree(Level + 1, new Rectangle(Bounds.Left + subWidth, Bounds.Top, subWidth, subHeight));
            Nodes[2] = new QuadTree(Level + 1, new Rectangle(Bounds.Left, Bounds.Top + subHeight, subWidth, subHeight));
            Nodes[3] = new QuadTree(Level + 1, new Rectangle(Bounds.Left + subWidth, Bounds.Top + subHeight, subWidth, subHeight));
        }

        private List<int> GetIndex(GameObject gameObject) {

            List<int> indexes = new List<int>();

            double verticalMidpoint = Bounds.X + (Bounds.Width / 2);
            double horizontalMidpoint = Bounds.Y + (Bounds.Height / 2);

            bool topQuadrant = gameObject.BoundingBox.Top <= horizontalMidpoint;
            bool bottomQuadrant = gameObject.BoundingBox.Bottom >= horizontalMidpoint;
            bool topAndBottomQuadrant = (gameObject.BoundingBox.Top < horizontalMidpoint) && (gameObject.BoundingBox.Bottom > horizontalMidpoint);

            if (topAndBottomQuadrant) {
                topQuadrant = false;
                bottomQuadrant = false;
            }

            //Left and right quad
            if(gameObject.BoundingBox.Left <= verticalMidpoint
                && gameObject.BoundingBox.Right >= Bounds.Left 
                && gameObject.BoundingBox.Right >= verticalMidpoint
                && gameObject.BoundingBox.Left <= Bounds.Right) {

                if(topQuadrant) {
                    indexes.Add(0);
                    indexes.Add(1);
                }
                else if(bottomQuadrant) {
                    indexes.Add(2);
                    indexes.Add(3);
                }
                else if(topAndBottomQuadrant) {
                    
                    indexes.Add(0);
                    indexes.Add(1);
                    indexes.Add(2);
                    indexes.Add(3);
                }
            }
            //Right quad
            else if(gameObject.BoundingBox.Left > verticalMidpoint) {
                if(topQuadrant) {
                    indexes.Add(1);
                }
                else if(bottomQuadrant) {
                    indexes.Add(3);
                }
                else if(topAndBottomQuadrant) {
                    indexes.Add(1);
                    indexes.Add(3);
                }
            }
            //Left quad
            else if(gameObject.BoundingBox.X + gameObject.Width < verticalMidpoint) {
                if(topQuadrant) {
                    indexes.Add(0);
                }
                else if(bottomQuadrant) {
                    indexes.Add(2);
                }
                else if(topAndBottomQuadrant) {
                    indexes.Add(0);
                    indexes.Add(2);
                }
            }
            else {
                indexes.Add(-1);
            }
            return indexes;
        }

        public void insert(GameObject gameObject) {

            if (Nodes[0] != null) {
                List<int> indexes = GetIndex(gameObject);

                for (int i = 0; i < indexes.Count; i++) {
                    int index = indexes[i];
                    if (index != -1) {
                        Nodes[index].insert(gameObject);
                        return;
                    }
                }
            }

            GameObjects.Add(gameObject);

            if (GameObjects.Count > MaxObjects && Level < MaxLevels) {
                if (Nodes[0] == null) {
                    split();
                }

                int i = 0;

                while (i < GameObjects.Count) {
                    List<int> indexes = GetIndex(GameObjects[i]);
                    GameObject obj = GameObjects[i];

                    for (int j = 0; j < indexes.Count; j++) {
                        int index = indexes[j];
                        if (index != -1) {
                            Nodes[index].insert(obj);
                            GameObjects.Remove(obj);
                        }
                        else {
                            i++;
                        }
                    }
                }
            }
        }

        public List<GameObject> retrieve(List<GameObject> retrieveObjects, GameObject gameObject) {

            List<int> indexes = GetIndex(gameObject);

            for (int i = 0; i < indexes.Count; i++) {
                int index = indexes[i];

                if (index != -1 && Nodes[0] != null)
                    Nodes[index].retrieve(retrieveObjects, gameObject);

                retrieveObjects.AddRange(GameObjects);
            }

            return retrieveObjects;
        }

        public void Draw(SpriteBatch spriteBatch) {

            SpaceMAS s = GameServices.GetService<SpaceMAS>();

            if (Nodes[0] != null)
                foreach (QuadTree quadTree in Nodes)
                    quadTree.Draw(spriteBatch);

            spriteBatch.Draw(Texture, new Rectangle(Bounds.Left, Bounds.Top, Bounds.Width, 1), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.Left, Bounds.Bottom, Bounds.Width, 1), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.Left, Bounds.Top, 1, Bounds.Height), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.Right, Bounds.Top, 1, Bounds.Height + 1), Color.White);

            //spriteBatch.DrawString(s.Font, Level.ToString(), new Vector2(Bounds.Left + 2 * ((Level+1) * 10), Bounds.Top + 2 * ((Level+1) + 10) ), Color.White);
        }
    }
}
