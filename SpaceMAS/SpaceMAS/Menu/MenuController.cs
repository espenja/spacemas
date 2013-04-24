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
            
            Menu highscoreMenu = new Menu(this, 2, 0);
            highscoreMenu.CreateButton("highscore", "Some stuff", 0, GameState.HIGHSCORE, -1);
            highscoreMenu.CreateButton("highscore", "Back", 1, GameState.MENU, 0);

            Menu levelChangeMenu = new Menu(this, 3, -1);
            levelChangeMenu.CreateButton("easy", "Easy", 0, GameState.PLAYING_EASY, -1);
            levelChangeMenu.CreateButton("normal", "Normal", 1, GameState.PLAYING_NORMAL, -1);
            levelChangeMenu.CreateButton("hard", "Hard", 2, GameState.PLAYING_HARD, -1);
            

            Menus.Add(mainMenu);
            Menus.Add(highscoreMenu);
            Menus.Add(levelChangeMenu);

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
