using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;
using Microsoft.Xna.Framework.Input;
using System;
using SpaceMAS.Models.Components.BulletEffects;
using SpaceMAS.Utils;
using Microsoft.Xna.Framework.Content;

namespace SpaceMAS.Models.Players {
    public class Player : KillableGameObject, BulletListener {

        public string Name { get; private set; }
        private HealthBar HealthBar { get; set; }
        public Controls PlayerControls { get; set; }
        private Weapon Weapon { get; set; }
        private int Money { get; set; }

        public Player(string name, Vector2 position, Texture2D Texture) {
            this.Texture = Texture;

            Name = name;
            Rotation = 0.0f;
            Position = position;
            AccelerationRate = 8.0f;
            RotationRate = 8.0f;
            Scale = 0.5f;

            MaxHealthPoints = 100;
            HealthPoints = 100;
            Money = 0;

            ContentManager cm = GameServices.GetService<ContentManager>();
            Bullet weaponBullet = new Bullet(-30f, 850f, new DisableEffect(2000f), cm.Load<Texture2D>("Textures/bullet"));
            Weapon = new Weapon(weaponBullet, 150f, 2000, this);

            HealthBar = new HealthBar(this);
            PlayerControls = ControlsController.GetControls(name);
        }

        public override void Update(GameTime gameTime) {

            //If the player is dead, then movement should not occur
            if (!Dead) {

                //A disabled player should not be allowed to move, but he can still be killed
                if (!Disabled)
                    Move(gameTime);
                HealthBar.Update(gameTime);
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(PlayerControls.Shoot)) {
                    Weapon.Shoot(gameTime);
                }
                else if (state.IsKeyDown(Keys.S)) {
                    Velocity = new Vector2(0, 0);
                }
            }

            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch) {
            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime) {
            float ElapsedGameTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 NewVelocity = new Vector2(Velocity.X, Velocity.Y);

            KeyboardState state = Keyboard.GetState();


            //Add userinput
            if (state.IsKeyDown(PlayerControls.TurnRight)) Rotation += RotationRate * ElapsedGameTime / 2;
            if (state.IsKeyDown(PlayerControls.TurnLeft)) Rotation -= RotationRate * ElapsedGameTime / 2;

            if (state.IsKeyDown(PlayerControls.Decelerate)) {
                NewVelocity.X -= (float) Math.Cos(Rotation) * AccelerationRate * ElapsedGameTime;
                NewVelocity.Y -= (float) Math.Sin(Rotation) * AccelerationRate * ElapsedGameTime;
            }
            if (state.IsKeyDown(PlayerControls.Accelerate)) {
                NewVelocity.X += (float) Math.Cos(Rotation) * AccelerationRate * ElapsedGameTime;
                NewVelocity.Y += (float) Math.Sin(Rotation) * AccelerationRate * ElapsedGameTime;
            }


            Velocity = NewVelocity;
            Position += Velocity;

            //TODO: Stop at screen edges?
            if (Position.X > GeneralSettings.screenWidth) {
                Position = new Vector2(GeneralSettings.screenWidth, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
            }
            else if (Position.X < 0) {
                Position = new Vector2(0, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
            }
            if (Position.Y > GeneralSettings.screenHeight) {
                Position = new Vector2(Position.X, GeneralSettings.screenHeight);
                Velocity = new Vector2(Velocity.X, 0);
            }
            else if (Position.Y < 0) {
                Position = new Vector2(Position.X, 0);
                Velocity = new Vector2(Velocity.X, 0);
            }

        }

        public bool ClickedPauseKey() {
            return Keyboard.GetState().IsKeyDown(Controls.Pause);
        }

        public override void Die() {
            //Player specific die actions, for example that the player does not disappear but instead is "greyed out" and stationary/uncontrollable
            Dead = true;
        }

        public void BulletImpact(Bullet Bullet, GameObject Object) {
            if (Object is Enemy.Enemy) {
                Money += ((Enemy.Enemy) Object).Bounty;
            }
        }

        public override void Disable() {
            Disabled = true;
            Velocity = Vector2.Zero;
        }

        public override void Enable() {
            Disabled = false;
        }
    }
}
