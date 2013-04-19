namespace SpaceMAS.Models {
    public abstract class KillableGameObject : GameObject {

        public float HealthPoints { get; set; }
        public float MaxHealthPoints { get; set; }
        public bool Dead { get; protected set; }

        public virtual void Die()
        {
            //Default die action for KillableGameObjects
            AccelerationRate = 0f;
        }

        public abstract void Disable();
        public abstract void Enable();
    }
}
