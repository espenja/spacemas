using System;
using System.Timers;

namespace SpaceMAS.Models.Components.BulletEffects {
    internal class DisableEffect : BulletEffect {
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

        public BulletEffect Clone() {
            BulletEffect clone = new DisableEffect(Duration);
            return clone;
        }
    }
}
