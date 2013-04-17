namespace SpaceMAS.State {
    public class StateProvider {

        private static StateProvider instance;
        public GameState State { get; set; }

        public static StateProvider Instance {
            get {
                if (instance == null) {
                    instance = new StateProvider { State = GameState.MENU };
                }
                return instance;
            }
        }
    }
}