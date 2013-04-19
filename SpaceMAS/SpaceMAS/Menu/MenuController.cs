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
            mainMenu.CreateButton("play", "Play", 0, GameState.PLAYING, -1);
            mainMenu.CreateButton("options", "Options", 1, GameState.MENU, 1);
            mainMenu.CreateButton("highscore", "Highscore", 2, GameState.MENU, 2);
            mainMenu.CreateButton("quit", "Quit", 3, GameState.QUIT, -1);

            //Options menu with ID = 1
            Menu optionsMenu = new Menu(this, 1, 0);
            optionsMenu.CreateButton("options", "Some stuff", 0, GameState.MENU, -1);
            optionsMenu.CreateButton("options", "Back", 1, GameState.MENU, 0);
            
            //Highscore menu with ID = 2
            Menu highscoreMenu = new Menu(this, 2, 0);
            highscoreMenu.CreateButton("highscore", "Some stuff", 0, GameState.HIGHSCORE, -1);
            highscoreMenu.CreateButton("highscore", "Back", 1, GameState.MENU, 0);

            //Pause menu with ID = 3
            Menu pauseMenu = new Menu(this, 3, -1);
            pauseMenu.CreateButton("play", "Resume", 0, GameState.PLAYING, -1);
            pauseMenu.CreateButton("options", "Options", 1, GameState.MENU, 1);
            pauseMenu.CreateButton("highscore", "Highscore", 2, GameState.MENU, 2);
            pauseMenu.CreateButton("quit", "Quit", 3, GameState.QUIT, -1);

            Menus.Add(mainMenu);
            Menus.Add(optionsMenu);
            Menus.Add(highscoreMenu);
            Menus.Add(pauseMenu);

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
