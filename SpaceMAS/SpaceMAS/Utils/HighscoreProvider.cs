using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using SpaceMAS.Models.Players;

namespace SpaceMAS.Utils
{
    class HighscoreProvider
    {
        public List<String> Highscore;

        private static HighscoreProvider _instance;

        public static HighscoreProvider Instance
        {
            get
            {
                if (_instance == null) _instance = new HighscoreProvider();
                return _instance;
            }

        }

        private HighscoreProvider ()
        {
            try
            {
                var contentManager = GameServices.GetService<ContentManager>();
                Highscore = new List<string>(System.IO.File.ReadAllLines(contentManager.RootDirectory + "/highscore.txt"));
               
            }
            catch (FileNotFoundException e)
            {
                Highscore = new List<string>();
            }
        }

        public void SaveHighscore()
        {
            var contentManager = GameServices.GetService<ContentManager>();
            System.IO.File.WriteAllLines(contentManager.RootDirectory + "/highscore.txt", Highscore);
        }
    }
}
