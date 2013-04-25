using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceMAS.Graphics;
using SpaceMAS.Settings;
using SpaceMAS.State;
using SpaceMAS.Utils;

namespace SpaceMAS.Menu {

    public class MenuButton {

        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public GameState ChangesToState { get; set; }
        public int ChangesToMenu { get; set; }
        private Texture2D texture;
        private SpriteFont Font;
        public Color Color { get; set; }
        private Color FontColor { get; set; }

        public Texture2D Texture {
            get { return texture; }
            set {
                texture = value;
            }
        }

        public MenuButton() {
            var contentManger = GameServices.GetService<ContentManager>();
            Font = contentManger.Load<SpriteFont>(GeneralSettings.FontsPath + "HandofSeanMenu");
            FontColor = Color.White;
        }

        public void UpdateBounds() {
        }

        public void Highlight(bool highlighted) {
            Color = highlighted ? Color.LightGreen : Color.White;
            FontColor = highlighted ? Color.LightGreen : Color.White;
        }

        public void Draw(SpriteBatch spriteBatch) {

            var size = Font.MeasureString(Text);
            var fontPosition = new Vector2(Position.X + Texture.Width / 2f, Position.Y + Texture.Height / 2f);

            spriteBatch.Draw(Texture, Position, null, Color, 0, Vector2.Zero, 1, SpriteEffects.None, GameDrawOrder.FOREGROUND_BOTTOM);
            spriteBatch.DrawString(Font, Text, fontPosition, FontColor, 0, size / 2, 1, SpriteEffects.None, GameDrawOrder.FOREGROUND_TOP);
            spriteBatch.DrawString(Font, Text, new Vector2(fontPosition.X + 2, fontPosition.Y + 2), Color.Black, 0, size / 2, 1, SpriteEffects.None, GameDrawOrder.FOREGROUND_TOP + 0.000001f);
        }
    }
}
