using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;

namespace SpaceMAS.Models.Player {
    public class Player : KillableGameObject {

        public string Name { get; private set; }
        private HealthBar HealthBar { get; set; }

        public Player(string name, Vector2 position) {
            Name = name;
            Rotation = 0f;
            Position = position;
            
            MaxHealthPoints = 100;
            HealthPoints = 100;

            HealthBar = new HealthBar(this);
        }

        public override void Update(GameTime gameTime) {

            //Position = new Vector2(Position.X + 0.4f, Position.Y);

            HealthPoints -= 0.4f;
            HealthBar.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {

            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
