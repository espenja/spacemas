
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMAS.Utils {
    public class Textures {

        private static Textures instance;

        public Texture2D WhitePixel { get; private set; }

        private Textures() {
            var graphicsDevice = GameServices.GetService<GraphicsDevice>();
            WhitePixel = new Texture2D(graphicsDevice, 1, 1);
            WhitePixel.SetData(new [] { Color.White });
        }

        public static Textures Instance {
            get { return instance ?? (instance = new Textures()); }
        }
    }
}
