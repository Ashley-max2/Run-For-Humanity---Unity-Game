using UnityEngine;
using RunForHumanity.Core.Events;

namespace RunForHumanity.Systems
{
    public class RevenueTracking : MonoBehaviour
    {
        public enum Fuente { AdRewarded, CompraInApp, Suscripcion, GameplayCoin } // Entendiendose Coin como micro-donacion simulada

        public static RevenueTracking Instance;

        private float totalRaisedSession = 0f;

        void Awake()
        {
            if (Instance == null) Instance = this;
            EventManager.OnCoinCollected += (amount) => RegistrarIngreso(amount * 0.01f, Fuente.GameplayCoin); // 1 coin = $0.01 impact mock
        }
        
        void OnDestroy()
        {
             EventManager.OnCoinCollected -= (amount) => RegistrarIngreso(amount * 0.01f, Fuente.GameplayCoin);
        }

        public void RegistrarIngreso(float monto, Fuente fuente)
        {
            totalRaisedSession += monto;
            
            DistributeToONGs(monto);
            
            Debug.Log($"Revenue Tracked: ${monto} via {fuente}. Total Session: ${totalRaisedSession}");
        }

        private void DistributeToONGs(float amount)
        {
            if (ONGManager.Instance == null) return;

            var prefs = ONGManager.Instance.currentUserPreferences;
            if (prefs == null || prefs.distribucion == null) return;

            foreach (var dist in prefs.distribucion)
            {
                float share = amount * (dist.percentage / 100f);
                // In a real app, we would update a local DB of "Pending Donations" per ONG
                // Debug.Log($"Allocating ${share} to {ONGManager.Instance.GetONG(dist.ongId).nombre}");
            }
            
            // UI Update Event could fly here ("You just saved 0.5 Trees!")
        }
    }
}
