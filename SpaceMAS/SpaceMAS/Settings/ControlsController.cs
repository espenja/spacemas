using System.Collections.Generic;

namespace SpaceMAS.Settings {
    public static class ControlsController {

        private static readonly Dictionary<string, Controls> Controls = new Dictionary<string, Controls>();

        public static Controls GetControls(string player) {
            if (Controls.ContainsKey(player))
                return Controls[player];

            Controls[player] = new Controls(player);
            return Controls[player];
        }

        public static void SetControls(string player, Controls controls) {
            Controls[player] = controls;
        }

        public static void LoadControls() {
            //Load controls serialized to file plz
        }
    }
}
