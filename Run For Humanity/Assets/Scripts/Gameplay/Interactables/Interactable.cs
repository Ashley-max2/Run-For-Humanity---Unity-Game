using UnityEngine;
using RunForHumanity.Core.Events;

namespace RunForHumanity.Gameplay.Interactables
{
    // SOLID: Interface for interaction logic
    public interface IInteractable
    {
        void Interact();
    }

    public class Interactable : MonoBehaviour, IInteractable
    {
        protected bool collected = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!collected && other.CompareTag("Player"))
            {
                Interact();
            }
        }

        public virtual void Interact()
        {
            // Base implementation
        }
    }
}
