using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;
using Microsoft.Xna.Framework.Input;

namespace SpaceMAS.Models.Players {
    public class Player : KillableGameObject {

        public string Name { get; private set; }
        private HealthBar HealthBar { get; set; }
        public Controls PlayerControls { get; set; }
        public bool Disabled { get; set; }

        public Player(string name, Vector2 position) {
            Name = name;
            Rotation = 0f;
            Position = position;
            AccelerationRate = 800f;
            NaturalDecelerationRate = 100f;
            RotationRate = 3f;
            Scale = 0.5f;

            MaxHealthPoints = 100;
            HealthPoints = 100;

            HealthBar = new HealthBar(this);
            PlayerControls = ControlsController.GetControls(name);
        }

        public override void Update(GameTime gameTime) {

            //If the player is dead, then movement should not occur
            if (!Dead) {

                //A disabled player should not be allowed to move, but he can still be killed
                if(!Disabled)
                    Move(gameTime);
                HealthBar.Update(gameTime);
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
            if (state.IsKeyDown(PlayerControls.TurnRight))
                Rotation += RotationRate * ElapsedGameTime;

            if (state.IsKeyDown(PlayerControls.TurnLeft)) 
                Rotation -= RotationRate * ElapsedGameTime;
            
            //TODO: Accelerate the direction the player is facing(need to consider rotation), not just X and Y axis as it is now
            if (state.IsKeyDown(PlayerControls.Decelerate))
                NewVelocity.X -= AccelerationRate * ElapsedGameTime;

            if (state.IsKeyDown(PlayerControls.Accelerate))
                NewVelocity.X += AccelerationRate * ElapsedGameTime;

            //Add natural deceleration
            if (Velocity.X > 0) 
                NewVelocity.X -= NaturalDecelerationRate * ElapsedGameTime;

            else if (Velocity.X < 0) {
                NewVelocity.X += NaturalDecelerationRate * ElapsedGameTime;
            }

            if (Velocity.Y > 0) 
                NewVelocity.Y -= NaturalDecelerationRate * ElapsedGameTime;

            else if (Velocity.Y < 0) 
                NewVelocity.Y += NaturalDecelerationRate * ElapsedGameTime;
            

            Velocity = NewVelocity;
            Position += Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;

            //Stop at screen edges


        }

        public bool ClickedPauseKey() {
            return Keyboard.GetState().IsKeyDown(Controls.Pause);
        }

        public override void Die() {
            //Player specific die actions, for example that the player does not disappear but instead is "greyed out" and stationary/uncontrollable
            DisablePlayer();
        }

        //Disables the player so that he cannot move and bring the spaceship to a halt quickly
        private void DisablePlayer() {
            AccelerationRate = 0f;
            NaturalDecelerationRate = 800f;
            Disabled = true;
        }

        //Enables the player to move again
        private void EnablePlayer() {
            AccelerationRate = 800f;
            NaturalDecelerationRate = 100f;
            Disabled = false;
        }
    }
}
