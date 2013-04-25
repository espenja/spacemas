using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.State;

namespace SpaceMAS.Menu {
    public class MenuController {

        private readonly List<Menu> Menus;
        public Menu CurrentMenu { get; private set; }

        public MenuController() {
            Menus = new List<Menu>();
            CreateMenus();
        }

        private void CreateMenus() {

            //Main menu with ID = 0
            Menu mainMenu = new Menu(this, 0, -1);
            mainMenu.CreateButton("button", "Play", 0, GameState.LEVEL_INTRO, 3);
            mainMenu.CreateButton("button", "Controls", 1, GameState.CONTROLS, -1);
            mainMenu.CreateButton("button", "Highscore", 2, GameState.MENU, 2);
            mainMenu.CreateButton("button", "Quit", 3, GameState.QUIT, -1);

            Menu shopMenu = new Menu(this, 1, -1);
            shopMenu.CreateButton("button", "Damage + 10", 0, GameState.SHOP_DAMAGE, -1);
            shopMenu.CreateButton("button", "Health + 10", 1, GameState.SHOP_HEALTH, -1);
            shopMenu.CreateButton("button", "Acceleration + 1", 2, GameState.SHOP_ACCELERATION, -1);
            shopMenu.CreateButton("button", "Bullet speed + 50", 3, GameState.SHOP_BULLETSPEED, -1);
            shopMenu.CreateButton("button", "Full health", 4, GameState.SHOP_HEAL, -1);
            shopMenu.CreateButton("button", "Done", 5, GameState.SHOP_DONE, 3);
            
            Menu highscoreMenu = new Menu(this, 2, 0);
            highscoreMenu.CreateButton("button", "Some stuff", 0, GameState.HIGHSCORE, -1);
            highscoreMenu.CreateButton("button", "Back", 1, GameState.MENU, 0);

            Menu levelChangeMenu = new Menu(this, 3, -1);
            levelChangeMenu.CreateButton("button", "Easy", 0, GameState.PLAYING_EASY, 1);
            levelChangeMenu.CreateButton("button", "Normal", 1, GameState.PLAYING_NORMAL, 1);
            levelChangeMenu.CreateButton("button", "Hard", 2, GameState.PLAYING_HARD, 1);
            

            Menus.Add(mainMenu);
            Menus.Add(highscoreMenu);
            Menus.Add(levelChangeMenu);
            Menus.Add(shopMenu);

            CurrentMenu = mainMenu;
        }

        public void ChangeMenu(int menuID) {

            Menu CurrentMenuTemp = CurrentMenu;

            CurrentMenu = Menus.Find(m => m.MenuID == menuID);
            if (CurrentMenu != null)
                CurrentMenu.SelectFirstButton();
            else
                CurrentMenu = CurrentMenuTemp;
        }

        public void Update(GameTime gameTime) {
            CurrentMenu.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            CurrentMenu.Draw(spriteBatch);
        }
    }
}
