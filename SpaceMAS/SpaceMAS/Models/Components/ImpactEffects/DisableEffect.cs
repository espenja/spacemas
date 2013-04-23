using System;
using System.Timers;

namespace SpaceMAS.Models.Components.ImpactEffects {
    internal class DisableEffect : IImpactEffect {
        protected float Duration;
        private KillableGameObject Victim;

        public DisableEffect(float Duration) {
            this.Duration = Duration;
        }

        public void OnImpact(GameObject Object) {

            if (Object is KillableGameObject) {
                Victim = (KillableGameObject) Object;
                Victim.Disable();

                Timer timer = new Timer(Duration);
                timer.Elapsed += RemoveEffect;
                timer.AutoReset = false;
                timer.Start();
            }
        }

        private void RemoveEffect(Object Object, ElapsedEventArgs Args) {
            ((Timer) Object).Dispose();
            Victim.Enable();
        }

        public IImpactEffect Clone() {
            IImpactEffect clone = new DisableEffect(Duration);
            return clone;
        }
    }
}
