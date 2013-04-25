using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceMAS.Models.Players;
using SpaceMAS.Settings;
using SpaceMAS.State;
using SpaceMAS.Utils;


namespace SpaceMAS.Menu {
    public class Menu {

        public int MenuID { get; private set; }

        protected MenuController MenuController { get; private set; }
        private List<MenuButton> MenuButtons = new List<MenuButton>();
        private int PreviousMenu { get; set; }
        private MenuButton SelectedButton;
        private const float ButtonSpacing = 25f;
        private const float buttonHeight = 53f;

        private double LastActionTime;

        public Menu(MenuController menuController, int menuID, int previousMenu) {
            MenuController = menuController;
            PreviousMenu = previousMenu;
            MenuID = menuID;
        }

        public void CreateButton(string textureName, string text, int buttonIndex, GameState changesToStateProvider, int changesToMenu) {

            var graphics = GameServices.GetService<GraphicsDevice>();
            var contentManager = GameServices.GetService<ContentManager>();

            var texture = contentManager.Load<Texture2D>(GeneralSettings.ButtonsPath + textureName);
            var button = new MenuButton {
                                                   Texture = texture,
                                                   Text = text,
                                                   ChangesToState = changesToStateProvider,
                                                   Id = buttonIndex,
                                                   Position = new Vector2(graphics.Viewport.Width / 2f - texture.Width / 2f, graphics.Viewport.Height / 2f),
                                                   Color = Color.White,
                                                   ChangesToMenu = changesToMenu
                                               };

            
            MenuButtons.Add(button);

            MenuButtons = MenuButtons.OrderBy(b => b.Id).ToList();
            SetButtonPositions();
        }

        private void SetButtonPositions() {
            var graphicsDevice = GameServices.GetService<GraphicsDevice>();

            float totalHeight = buttonHeight * MenuButtons.Count + (MenuButtons.Count - 1) * ButtonSpacing;

            foreach (MenuButton button in MenuButtons) {
                float positionX = graphicsDevice.Viewport.Width / 2f - button.Texture.Width / 2f;
                float positionY = graphicsDevice.Viewport.Height / 2f - totalHeight / 2f + buttonHeight * button.Id + ButtonSpacing * button.Id;//graphics.Viewport.Height / 2f - 100 + ButtonSpacing * button.Id + buttonHeight;
                button.Position = new Vector2(positionX, positionY);
                button.UpdateBounds();
            }

            SelectFirstButton();
        }

        public virtual void Update(GameTime gameTime) {

            LastActionTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState state = Keyboard.GetState();
            foreach (Player player in GameServices.GetService<List<Player>>())
            {
                if (LastActionTime > 200f)
                {
                    if (state.IsKeyDown(player.PlayerControls.MenuDown))
                    {
                        SelectNextButton();
                        LastActionTime = 0;
                    }
                    else if (state.IsKeyDown(player.PlayerControls.MenuUp))
                    {
                        SelectPreviousButton();
                        LastActionTime = 0;
                    }
                    else if (state.IsKeyDown(player.PlayerControls.MenuSelect))
                    {
                        StateProvider.Instance.State = SelectedButton.ChangesToState;
                        MenuController.ChangeMenu(SelectedButton.ChangesToMenu);
                        LastActionTime = 0;
                    }
                    else if (state.IsKeyDown(Controls.Back))
                    {
                        StateProvider.Instance.State = GameState.MENU;

                        if (PreviousMenu != -1)
                            MenuController.ChangeMenu(PreviousMenu);
                        LastActionTime = 0;
                    }
                }
            }
            
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            foreach (MenuButton button in MenuButtons)
                button.Draw(spriteBatch);
        }

        private void SelectNextButton() {
            if (SelectedButton.Id == MenuButtons.Count - 1)
                SelectButton(MenuButtons[0]);
            else
                SelectButton(MenuButtons[SelectedButton.Id + 1]);
        }

        private void SelectPreviousButton() {
            if (SelectedButton.Id == 0)
                SelectButton(MenuButtons[MenuButtons.Count - 1]);
            else
                SelectButton(MenuButtons[SelectedButton.Id - 1]);
        }

        public void SelectFirstButton() {

            if (SelectedButton == null)
                SelectedButton = MenuButtons[0];

            SelectedButton.Highlight(false);
            MenuButtons[0].Highlight(true);
            SelectedButton = MenuButtons[0];
        }

        public void SelectButton(MenuButton button) {
            SelectedButton.Highlight(false);
            SelectedButton = button;
            SelectedButton.Highlight(true);
        }

        public void PressButton(MenuButton button) {
        }
    }
}
