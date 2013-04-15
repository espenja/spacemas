using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Utils;

namespace SpaceMAS.Models.Enemies.Spawner {
    public class SquareEnemySpawner : EnemySpawner {
        public SquareEnemySpawner(Vector2 position) {
            Position = position;
            Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("square");
        }


    }
}
