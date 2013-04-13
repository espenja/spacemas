using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Settings;
using SpaceMAS.Models;

namespace SpaceMAS.Menu
{
    class MenuButton
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle rectangle;
        private Color color;
        public Vector2 size;
        public bool isPressed = false;
        private bool isHighlighted = false;
        private Controls controls;
        public GameState changesToState;

        public MenuButton(Texture2D texture, Vector2 position, GraphicsDevice graphics, Controls controls, GameState changesToState)
        {
            this.texture = texture;
            this.position = position;
            this.controls = controls;
            size = new Vector2(graphics.Viewport.Height / 8, graphics.Viewport.Width / 20);
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.changesToState = changesToState;
        }

        private void Highlight()
        {
            isHighlighted = true;
            color = Color.White;
        }

        private void NotHighlight()
        {
            isHighlighted = false;
            color = Color.Red;
        }

        public void Update(MenuButton selectedButton)
        {
            if (isHighlighted && Keyboard.GetState().IsKeyDown(controls.MenuSelect)) isPressed = true;
            else isPressed = false;

            if (selectedButton == this) Highlight();
            else NotHighlight();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }



    }
}
