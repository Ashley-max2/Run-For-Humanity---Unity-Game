namespace RunForHumanity.Core
{
    /// <summary>
    /// Interface for objects that need update loops
    /// SOLID: Interface Segregation Principle
    /// </summary>
    public interface IUpdatable
    {
        void OnUpdate(float deltaTime);
    }
}
