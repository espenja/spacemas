using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Models.Components
{
    public interface BulletListener
    {
        void BulletImpact(Bullet Bullet, GameObject Object);
    }
}
