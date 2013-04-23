using SpaceMAS.Models.Components;

namespace SpaceMAS.Models {
    public abstract class KillableGameObject : GameObject {

        public HealthBar HealthBar { get; set; }
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

        protected KillableGameObject()  {
            HealthBar = new HealthBar(this);
        }
      
        public bool Dead { get; set; }
        public bool Disabled { get; set; }

        public abstract void Die();
        public abstract void Disable();
        public abstract void Enable();
    }
}
