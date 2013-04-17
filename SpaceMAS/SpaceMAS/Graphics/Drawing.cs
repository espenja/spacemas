using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMAS.Graphics {
    public class Drawing {

        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color blendColor, float rotation, Vector2 origin,
                                SpriteEffects spriteEffect, float layer) {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, blendColor, rotation, origin, spriteEffect, layer);
        }

        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Color blendColor, float layer) {
            spriteBatch.Draw(texture, destinationRectangle, null, blendColor, 0.0f, Vector2.Zero, SpriteEffects.None, layer);
        }

        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Color blendColor, float alpha, float layer) {
            spriteBatch.Draw(texture, destinationRectangle, null, new Color(blendColor.R, blendColor.G, blendColor.B, alpha), 0.0f, Vector2.Zero, SpriteEffects.None, layer);
        }

        public static void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color blendColor, float scale, float layer) {
            spriteBatch.DrawString(spriteFont, text, position, blendColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color blendColor, float alpha, float scale, float layer) {
            spriteBatch.DrawString(spriteFont, text, position, new Color(blendColor.R, blendColor.G, blendColor.B, alpha), 0.0f, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawStringWithBorder(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color blendColor, float alpha, Color borderColor, float borderAlpha, int BorderSize, float scale, float layer) {
            spriteBatch.DrawString(spriteFont, text, position - new Vector2(BorderSize, 0), new Color(borderColor.R, borderColor.G, borderColor.B, borderAlpha), 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(spriteFont, text, position - new Vector2(0, BorderSize), new Color(borderColor.R, borderColor.G, borderColor.B, borderAlpha), 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(BorderSize, 0), new Color(borderColor.R, borderColor.G, borderColor.B, borderAlpha), 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(0, BorderSize), new Color(borderColor.R, borderColor.G, borderColor.B, borderAlpha), 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(spriteFont, text, position, new Color(blendColor.R, blendColor.G, blendColor.B, alpha), 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
        }
    }
}