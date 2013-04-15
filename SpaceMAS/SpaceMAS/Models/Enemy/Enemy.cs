namespace SpaceMAS.Models.Enemy {
    public class Enemy : KillableGameObject {

        public new float Health {
            get { return base.Health; }
            set { base.Health = value; }
        }
    }
}