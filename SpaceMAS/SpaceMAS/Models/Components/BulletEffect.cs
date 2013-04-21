namespace SpaceMAS.Models.Components
{
    public interface BulletEffect
    {
        void OnImpact(GameObject o);
        BulletEffect Clone();
    }
}
