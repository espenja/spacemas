using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace SpaceMAS.Settings {
    
    public class Controls {

        public Keys TurnLeft;
        public Keys TurnRight;
        public Keys Accelerate;
        public Keys Decelerate;
        public Keys Shoot;
        public static Keys Pause;
        public Keys MenuSelect;
        public Keys MenuUp;
        public Keys MenuDown;
        public static Keys Back;

        public Controls() {
        }

        //Converts all fields in this class to a List of Keys.
        //Useful for checking wether another player on the same machine is using the same keys.
        public List<Keys> AsList() {
            return (from fields in typeof(Controls).GetFields()
                    where typeof (Keys).IsAssignableFrom(fields.FieldType)
                    select (Keys) fields.GetValue(this)).ToList();
        }

        public void LoadPlayer1Controls() {
            TurnLeft = Keys.Left;
            TurnRight = Keys.Right;
            Accelerate = Keys.Up;
            Decelerate = Keys.Down;
            Shoot = Keys.RightControl;
            Pause = Keys.P;
            MenuSelect = Keys.Enter;
            MenuDown = Keys.Down;
            MenuUp = Keys.Up;
            Back = Keys.Escape;
        }

        public void LoadPlayer2Controls()
        {
            TurnLeft = Keys.A;
            TurnRight = Keys.D;
            Accelerate = Keys.W;
            Decelerate = Keys.S;
            Shoot = Keys.LeftControl;
            Pause = Keys.P;
            MenuSelect = Keys.Space;
            MenuDown = Keys.S;
            MenuUp = Keys.W;
            Back = Keys.Escape;
        }
    }
}
