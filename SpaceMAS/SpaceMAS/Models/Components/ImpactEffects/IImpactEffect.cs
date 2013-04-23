namespace SpaceMAS.Models.Components.ImpactEffects
{
    public interface IImpactEffect
    {
        void OnImpact(GameObject o);
        IImpactEffect Clone();
    }
}
