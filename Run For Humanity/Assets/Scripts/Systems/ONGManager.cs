using System.Collections.Generic;
using UnityEngine;
using RunForHumanity.Data;

namespace RunForHumanity.Systems
{
    public class ONGManager : MonoBehaviour
    {
        public static ONGManager Instance;

        [Header("Database")]
        public List<ONGData> availableOngs = new List<ONGData>(); // Load via ScriptableObjects or JSON later

        [Header("User Preferences")]
        public UserDonationPreferences currentUserPreferences;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            LoadMockData();
        }

        void LoadMockData()
        {
            // Create some dummy ONGs for the prototype as per PDF examples
            availableOngs.Add(new ONGData 
            { 
                id = "ong_agua", 
                nombre = "Clean Water", 
                categoria = "Agua", 
                impacto = "1$ = 50L Agua", 
                porcentajeTransparencia = 95f 
            });
            
            availableOngs.Add(new ONGData 
            { 
                id = "ong_forest", 
                nombre = "Green World", 
                categoria = "Medioambiente", 
                impacto = "1$ = 1 Árbol", 
                porcentajeTransparencia = 90f 
            });

            // Default User Selection (50/50 split)
            currentUserPreferences = new UserDonationPreferences();
            currentUserPreferences.distribucion.Add(new ONGDistribution { ongId = "ong_agua", percentage = 50 });
            currentUserPreferences.distribucion.Add(new ONGDistribution { ongId = "ong_forest", percentage = 50 });
        }

        public ONGData GetONG(string id)
        {
            return availableOngs.Find(o => o.id == id);
        }

        public void UpdateDistribution(List<ONGDistribution> newDistribution)
        {
            float total = 0;
            foreach(var d in newDistribution) total += d.percentage;

            if (Mathf.Abs(total - 100f) > 0.1f)
            {
                Debug.LogWarning("Distribution must sum 100%");
                return;
            }

            currentUserPreferences.distribucion = newDistribution;
            Debug.Log("Donation preferences updated!");
            // TODO: Trigger Cloud Save
        }
    }
}
