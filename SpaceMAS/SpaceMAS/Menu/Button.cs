using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Settings;
using SpaceMAS.Models;

namespace SpaceMAS.Menu {

    internal class MenuButton {

        private Texture2D texture;
        private Vector2 position;
        private Rectangle rectangle;
        private Color color;
        public Vector2 size;
        public bool isPressed = false;
        private bool isHighlighted;
        private Controls controls;
        public GameState changesToState;

        public MenuButton(Texture2D texture, Vector2 position, GraphicsDevice graphics, Controls controls, GameState changesToState) {
            this.texture = texture;

            this.position = new Vector2(graphics.Viewport.Width / 2f - texture.Width / 2f, position.Y);
            this.controls = controls;

            size = new Vector2(texture.Width, texture.Height);
            rectangle = new Rectangle((int) this.position.X, (int) this.position.Y, (int) size.X, (int) size.Y);

            this.changesToState = changesToState;
        }

        private void Highlight() {
            isHighlighted = true;
            color = Color.LightGreen;
        }

        private void NotHighlight() {
            isHighlighted = false;
            color = Color.White;
        }

        public void Update(MenuButton selectedButton) {
            if (isHighlighted && Keyboard.GetState().IsKeyDown(controls.MenuSelect)) isPressed = true;
            else isPressed = false;

            if (selectedButton == this) Highlight();
            else NotHighlight();
        }

        public void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
