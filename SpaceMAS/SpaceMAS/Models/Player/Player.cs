using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;
using Microsoft.Xna.Framework.Input;

namespace SpaceMAS.Models.Player {
    public class Player : KillableGameObject {

        public string Name { get; private set; }
        private HealthBar HealthBar { get; set; }
        public Controls PlayerControls { get; set; }

        public Player(string name, Vector2 position) {
            Name = name;
            Rotation = 0f;
            Position = position;
            AccelerationRate = 800f;
            NaturalDecelerationRate = 100f;
            RotationRate = 3f;
            
            MaxHealthPoints = 100;
            HealthPoints = 100;

            HealthBar = new HealthBar(this);
            PlayerControls = Controller.GetControls(name);
        }

        public override void Update(GameTime gameTime) {

            //If the player is dead, then movement should not occur
            if (!Dead)
            {
                Move(gameTime);
                HealthBar.Update(gameTime);
            }
            
            base.Update(gameTime);
            
        }

        public override void Draw(SpriteBatch spriteBatch) {

            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime)
        {         
            var elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var newVelocity = new Vector2(Velocity.X, Velocity.Y);

            //Add userinput
            if (Keyboard.GetState().IsKeyDown(PlayerControls.TurnRight))
            {
                Rotation += RotationRate * elapsedGameTime;
            }
            if (Keyboard.GetState().IsKeyDown(PlayerControls.TurnLeft))
            {
                Rotation -= RotationRate * elapsedGameTime;
            }
            //TODO: Accelerate the direction the player is facing(need to consider rotation), not just X and Y axis as it is now
            if (Keyboard.GetState().IsKeyDown(PlayerControls.Decelerate))
            {
                newVelocity.X -= AccelerationRate * elapsedGameTime;
            }
            if (Keyboard.GetState().IsKeyDown(PlayerControls.Accelerate))
            {
                newVelocity.X += AccelerationRate * elapsedGameTime;
            }

            //Add natural deceleration
            if (Velocity.X > 0)
            {
                newVelocity.X -= NaturalDecelerationRate * elapsedGameTime;
            }
            else if (Velocity.X < 0)
            {
                newVelocity.X += NaturalDecelerationRate * elapsedGameTime;
            }

            if (Velocity.Y > 0)
            {
                newVelocity.Y -= NaturalDecelerationRate * elapsedGameTime;
            }
            else if (Velocity.Y < 0)
            {
                newVelocity.Y += NaturalDecelerationRate * elapsedGameTime;
            }
            
            Velocity = newVelocity;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds; 
        }

        public bool ClickedPauseKey()
        {
            return Keyboard.GetState().IsKeyDown(PlayerControls.Pause);
        }

        public override void Die()
        {
            //Player specific die actions, for example that the player does not disappear but instead is "greyed out" and stationary/uncontrollable
            DisablePlayer();
        }

        //Disables the player so that he cannot move and bring the spaceship to a halt quickly
        private void DisablePlayer()
        {
            AccelerationRate = 0f;
            NaturalDecelerationRate = 800f;
        }

        //Enables the player to move again
        private void EnablePlayer()
        {
            AccelerationRate = 800f;
            NaturalDecelerationRate = 100f;
        }
    }
}
