namespace RunForHumanity.Core
{
    /// <summary>
    /// Interface for poolable objects
    /// SOLID: Interface Segregation Principle
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}
