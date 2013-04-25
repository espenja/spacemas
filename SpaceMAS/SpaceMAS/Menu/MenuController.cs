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
            mainMenu.CreateButton("play", "Play", 0, GameState.LEVEL_INTRO, 3);
            mainMenu.CreateButton("controls", "Controls", 1, GameState.CONTROLS, -1);
            mainMenu.CreateButton("highscore", "Highscore", 2, GameState.MENU, 2);
            mainMenu.CreateButton("quit", "Quit", 3, GameState.QUIT, -1);

            Menu shopMenu = new Menu(this, 1, -1);
            shopMenu.CreateButton("damage", "Damage + 10", 0, GameState.SHOP_DAMAGE, -1);
            shopMenu.CreateButton("health", "Health + 10", 1, GameState.SHOP_HEALTH, -1);
            shopMenu.CreateButton("acceleration", "Acceleration + 1", 2, GameState.SHOP_ACCELERATION, -1);
            shopMenu.CreateButton("bulletspeed", "Bullet speed + 50", 3, GameState.SHOP_BULLETSPEED, -1);
            shopMenu.CreateButton("heal", "Full health", 4, GameState.SHOP_HEAL, -1);
            shopMenu.CreateButton("done", "Done", 5, GameState.SHOP_DONE, 3);
            
            Menu highscoreMenu = new Menu(this, 2, 0);
            highscoreMenu.CreateButton("highscore", "Some stuff", 0, GameState.HIGHSCORE, -1);
            highscoreMenu.CreateButton("highscore", "Back", 1, GameState.MENU, 0);

            Menu levelChangeMenu = new Menu(this, 3, -1);
            levelChangeMenu.CreateButton("easy", "Easy", 0, GameState.PLAYING_EASY, 1);
            levelChangeMenu.CreateButton("normal", "Normal", 1, GameState.PLAYING_NORMAL, 1);
            levelChangeMenu.CreateButton("hard", "Hard", 2, GameState.PLAYING_HARD, 1);
            

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
