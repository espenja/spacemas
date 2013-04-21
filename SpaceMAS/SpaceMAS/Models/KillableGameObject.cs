namespace SpaceMAS.Models {
    public abstract class KillableGameObject : GameObject {

        public float MaxHealthPoints { get; set; }
        private float healthPoints;
        public float HealthPoints
        {
            get { return healthPoints; }
            set
            {
                healthPoints = value;
                if (healthPoints <= 0) Die();
            }
        }
      
        public bool Dead { get; protected set; }
        public bool Disabled { get; set; }

        public abstract void Die();
        public abstract void Disable();
        public abstract void Enable();
    }
}
