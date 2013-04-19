using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceMAS.Models.Players;
using System.Timers;

namespace SpaceMAS.Models.Components.BulletEffects
{
    class DisableEffect : BulletEffect
    {
        protected float Duration;
        private KillableGameObject Victim;

        public DisableEffect(float Duration)
        {
            this.Duration = Duration;
        }
        public void OnImpact(GameObject Object)
        {

            if (Object is KillableGameObject) {
                Victim = (KillableGameObject)Object;
                Victim.Disable();
                Timer timer = new System.Timers.Timer(Duration);
                timer.Elapsed += new ElapsedEventHandler(RemoveEffect);
                timer.AutoReset = false;
                timer.Start();
            }
        }

        private void RemoveEffect(Object Object, ElapsedEventArgs Args)
        {
            ((Timer)Object).Dispose();
            ((KillableGameObject)Victim).Enable();
        }

        public BulletEffect Clone()
        {
            BulletEffect clone = new DisableEffect(Duration);
            return clone;
        }
    }
}
