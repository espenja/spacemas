using SpaceMAS.Models;

namespace SpaceMAS.Interfaces
{
    public interface IImpactEffect
    {
        void OnImpact(GameObject o);
        IImpactEffect Clone();
    }
}
