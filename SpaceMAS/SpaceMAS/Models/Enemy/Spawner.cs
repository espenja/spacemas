using Microsoft.Xna.Framework;

namespace SpaceMAS.Models.Enemy
{
    public class Spawner : GameObject
    {

        private float NumberOfSpawns { get; set; }
        private float SpawnRate { get; set; }

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
