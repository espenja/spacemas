namespace SpaceMAS.Models {
    public class KillableGameObject : GameObject {

        public float HealthPoints { get; protected set; }
        public float MaxHealthPoints { get; protected set; }
        public bool Dead { get; protected set; }

        public virtual void Die()
        {
            //Default die action for KillableGameObjects
            AccelerationRate = 0f;
            NaturalDecelerationRate = 800f;
        }
    }
}
