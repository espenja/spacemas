﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Models.Components;
using SpaceMAS.Settings;
using Microsoft.Xna.Framework.Input;
using System;

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

            Move(gameTime);
            HealthBar.Update(gameTime);
            base.Update(gameTime);
            
        }

        public override void Draw(SpriteBatch spriteBatch) {

            HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime)
        {         
            float ElapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 NewVelocity = new Vector2(Velocity.X, Velocity.Y);

            //Add userinput
            if (Keyboard.GetState().IsKeyDown(PlayerControls.TurnRight))
            {
                Rotation += RotationRate * ElapsedGameTime;
            }
            if (Keyboard.GetState().IsKeyDown(PlayerControls.TurnLeft))
            {
                Rotation -= RotationRate * ElapsedGameTime;
            }
            //TODO: Accelerate the direction the player is facing(need to consider rotation), not just X and Y axis as it is now
            if (Keyboard.GetState().IsKeyDown(PlayerControls.Decelerate))
            {
                NewVelocity.X -= AccelerationRate * ElapsedGameTime;
            }
            if (Keyboard.GetState().IsKeyDown(PlayerControls.Accelerate))
            {
                NewVelocity.X += AccelerationRate * ElapsedGameTime;
            }

            //Add natural deceleration
            if (Velocity.X > 0)
            {
                NewVelocity.X -= NaturalDecelerationRate * ElapsedGameTime;
            }
            else if (Velocity.X < 0)
            {
                NewVelocity.X += NaturalDecelerationRate * ElapsedGameTime;
            }

            if (Velocity.Y > 0)
            {
                NewVelocity.Y -= NaturalDecelerationRate * ElapsedGameTime;
            }
            else if (Velocity.Y < 0)
            {
                NewVelocity.Y += NaturalDecelerationRate * ElapsedGameTime;
            }
            
            Velocity = NewVelocity;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds; 
        }

        public bool ClickedPauseKey()
        {
            return Keyboard.GetState().IsKeyDown(PlayerControls.Pause);
        }
    }
}
