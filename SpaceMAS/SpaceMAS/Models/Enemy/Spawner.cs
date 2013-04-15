using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceMAS.Models.Enemy
{
    public class Spawner : GameObject
    {

        private float NumberOfSpawns { get; private set; }
        private float SpawnRate { get; private set; }

        public Spawner(Vector2 position, float spawns, float rate)
        {
            Position = position;
            NumberOfSpawns = spawns;
            SpawnRate = rate;
        }

        public override void Update(GameTime gameTime)
        {
            //Spawn the enemies!
            
            base.Update(gameTime);
        }
    }
}
