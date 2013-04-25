using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SpaceMAS.Utils {
    internal class HighscoreProvider {
        public List<String> Highscore;

        private static HighscoreProvider _instance;

        public static HighscoreProvider Instance {
            get { return _instance ?? (_instance = new HighscoreProvider()); }
        }

        private HighscoreProvider() {
            try {
                var contentManager = GameServices.GetService<ContentManager>();
                Highscore = new List<string>(File.ReadAllLines(contentManager.RootDirectory + "/highscore.txt"));

            }
            catch (FileNotFoundException e) {
                Highscore = new List<string>();
                Console.WriteLine(e.Message);
            }
        }

        public void SaveHighscore() {
            var contentManager = GameServices.GetService<ContentManager>();
            File.WriteAllLines(contentManager.RootDirectory + "/highscore.txt", Highscore);
        }
    }
}
