using System;

namespace RunForHumanity.Core
{
    /// <summary>
    /// Interface for objects that need initialization
    /// SOLID: Interface Segregation Principle
    /// </summary>
    public interface IInitializable
    {
        void Initialize();
        bool IsInitialized { get; }
    }
}
