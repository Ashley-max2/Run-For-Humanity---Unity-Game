using UnityEngine;
using UnityEngine.EventSystems;

namespace RunForHumanity.Core
{
    // Crea EventSystem automáticamente si no existe
    public class AutoEventSystem : MonoBehaviour
    {
        private void Awake()
        {
            EnsureEventSystem();
        }

        private void EnsureEventSystem()
        {
            EventSystem existingEventSystem = FindObjectOfType<EventSystem>();

            if (existingEventSystem == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<EventSystem>();
                eventSystemObj.AddComponent<StandaloneInputModule>();
                
                Debug.Log("[AutoEventSystem] EventSystem creado automáticamente");
            }
            else
            {
                Debug.Log("[AutoEventSystem] EventSystem ya existe");
            }
        }
    }
}
