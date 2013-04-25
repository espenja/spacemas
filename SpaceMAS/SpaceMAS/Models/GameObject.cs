using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Graphics;
using SpaceMAS.Settings;
using SpaceMAS.Utils;
using SpaceMAS.Utils.Collition;

namespace SpaceMAS.Models {

    public class GameObject {

        //Data related variables
        private Texture2D texture;
        private Color[] colorData;
        private Matrix transform;

        //Position and size related variables
        private Vector2 position;
        public Color Color { get; set; }
        public Vector2 Origin { get; protected set; }
        public Vector2 Velocity { get; set; }
        public float AccelerationRate { get; set; }
        public float RotationRate { get; protected set; }
        public float Rotation { get; protected set; }
        public float scale = 0.5f;
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Rectangle BoundingBox { get; protected set; }

        public void LoadTexture(string textureName) {
            ContentManager contentManager = GameServices.GetService<ContentManager>();
            Texture = contentManager.Load<Texture2D>(GeneralSettings.TexturesPath + textureName);
        }

        public Vector2 Position {
            get { return position; }
            set {
                position = value;
                BoundingBox = new Rectangle((int) position.X - Width / 2, (int) position.Y - Height / 2, Width, Height);
            }
        }

        public Texture2D Texture {
            get { return texture; }
            set {
                texture = value;

                //Capture color data of texture for pixel collition detection algorithm
                colorData = new Color[texture.Width * texture.Height];
                texture.GetData(colorData);

                //Set origin to the center of the sprite for rotation around the center
                Origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
                Height = (int)(Texture.Height * scale);
                Width = (int)(Texture.Width * scale);
            }
        }

        public float Scale {
            get { return scale; }
            set {
                scale = value;
                if(Texture == null) {
                    Height = 0;
                    Width = 0;
                }
                else {
                    Height = (int) (Texture.Height * scale);
                    Width = (int) (Texture.Width * scale);
                }
            }
        }

        public Matrix Transform {
            get { return transform; }
        }

        public void UpdateTransform() {

            transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(new Vector3(Position, 0.0f));

        }

        public virtual void Update(GameTime gameTime) {
            UpdateTransform();
        }

        public virtual void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation + MathHelper.PiOver2, Origin, Scale, SpriteEffects.None, GameDrawOrder.GAME_LEVEL_TOP);
        }

        public bool IntersectPixels(GameObject otherObject) {
            return CollitionDetection.IntersectPixels(transform, texture.Width, texture.Height, colorData,
                                   otherObject.transform, otherObject.texture.Width, otherObject.texture.Height, otherObject.colorData, this, otherObject);
        }
    }
}
