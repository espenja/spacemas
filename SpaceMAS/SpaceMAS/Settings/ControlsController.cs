using SpaceMAS.Models.Players;

namespace SpaceMAS.Settings {
    public static class ControlsController
    {

        private static Player _player1;
        private static Player _player2;

        public static Controls GetControls(Player player) {
            if (_player1 == player) return _player1.PlayerControls;
            if (_player2 == player) return _player2.PlayerControls;

            var c = new Controls();

            if (_player1 == null)
            {
                _player1 = player;
                c.LoadPlayer1Controls();
                return c;
            }
            else
            {
                c.LoadPlayer2Controls();
                _player2 = player;
                return c;
            }

        }
    }
}
