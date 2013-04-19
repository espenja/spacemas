using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMAS.Models.Components
{
    public interface BulletEffect
    {
        void OnImpact(GameObject o);
        BulletEffect Clone();
    }
}
