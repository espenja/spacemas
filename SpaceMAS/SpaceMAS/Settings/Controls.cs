using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Input;

namespace SpaceMAS.Settings {
    
    public class Controls {

        public Keys TurnLeft;
        public Keys TurnRight;
        public Keys Accelerate;
        public Keys Decelerate;
        public Keys Shoot;
        public Keys Shield;
        public Keys Pause;
        public Keys MenuSelect;
        public Keys MenuUp;
        public Keys MenuDown;

        public Controls(string player) {
            LoadPlayerControls(player);
        }

        public void LoadDefaults() {
            TurnLeft = Keys.Left;
            TurnRight = Keys.Right;
            Accelerate = Keys.Up;
            Decelerate = Keys.Down;
            Shoot = Keys.Space;
            Shield = Keys.LeftShift;
            Pause = Keys.P;
            MenuSelect = Keys.Enter;
            MenuDown = Keys.Down;
            MenuUp = Keys.Up;
        }

        //Converts all fields in this class to a List of Keys.
        //Useful for checking wether another player on the same machine is using the same keys.
        public List<Keys> AsList() {
            return (from fields in typeof(Controls).GetFields()
                    where typeof (Keys).IsAssignableFrom(fields.FieldType)
                    select (Keys) fields.GetValue(this)).ToList();
        }

        public void LoadPlayerControls(string playerName) {
            //Load controls for player
            LoadDefaults();
        }
    }
}
