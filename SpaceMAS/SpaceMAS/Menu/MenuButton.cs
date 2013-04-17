using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceMAS.Models;
using SpaceMAS.State;

namespace SpaceMAS.Menu {

    public class MenuButton {

        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public GameState ChangesToState { get; set; }
        public int ChangesToMenu { get; set; }

        private Texture2D texture;
        public Color Color { get; set; }
        private Rectangle rectangle;

        public Texture2D Texture {
            get { return texture; }
            set {
                texture = value;
                rectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public void UpdateBounds() {
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public void Highlight(bool highlighted) {
            Color = highlighted ? Color.LightGreen : Color.White;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color);
        }
    }
}
