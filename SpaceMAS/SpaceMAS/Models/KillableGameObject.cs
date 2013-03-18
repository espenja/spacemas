using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Models {
    public class KillableGameObject : GameObject {

        public float HealthPoints { get; protected set; }
        public float MaxHealthPoints { get; protected set; }
        public bool Dead { get; protected set; }

    }
}
