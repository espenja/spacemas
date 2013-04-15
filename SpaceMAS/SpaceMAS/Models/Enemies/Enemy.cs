using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Models.Enemies {
    public class Enemy : KillableGameObject {
    
        public Enemy(int MaxHealthPoints) {
            this.MaxHealthPoints = MaxHealthPoints;
        }
    }
}
