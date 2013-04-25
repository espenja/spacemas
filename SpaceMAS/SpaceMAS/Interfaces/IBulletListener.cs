using SpaceMAS.Models;
using SpaceMAS.Models.Components;

namespace SpaceMAS.Interfaces
{
    public interface IBulletListener
    {
        void BulletImpact(Bullet bullet, GameObject Object);
    }
}
