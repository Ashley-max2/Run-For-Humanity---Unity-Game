using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RunForHumanity.Systems;
using RunForHumanity.Data;
using System.Collections.Generic;

namespace RunForHumanity.UI
{
    public class ONGSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        public Transform ongListContainer;
        public GameObject ongItemPrefab;
        public TextMeshProUGUI totalPercentageText;
        
        private List<ONGDistribution> tempDistribution = new List<ONGDistribution>();

        void Start()
        {
            LoadONGs();
        }

        void LoadONGs()
        {
            if (ONGManager.Instance == null) return;

            foreach (var ong in ONGManager.Instance.availableOngs)
            {
                GameObject item = Instantiate(ongItemPrefab, ongListContainer);
                // Configure Item UI (Name, Slider, etc.) - This requires a specific script on the prefab
                // For now, we assume simple debug population
                item.name = ong.nombre;
                
                // Setup slider listener to UpdateDistribution()
            }
            
            // Clone current prefs to temp
            tempDistribution = new List<ONGDistribution>(ONGManager.Instance.currentUserPreferences.distribucion);
            UpdateTotalVisuals();
        }

        public void OnSliderChanged(string ongId, float value)
        {
            // Logic to balance other sliders so total is always 100% is complex UX key feature.
            // Simplified: Allow free adjustment, warn if != 100.
            
            var target = tempDistribution.Find(x => x.ongId == ongId);
            if(target != null) target.percentage = value;
            else tempDistribution.Add(new ONGDistribution{ongId = ongId, percentage = value});

            UpdateTotalVisuals();
        }

        void UpdateTotalVisuals()
        {
            float total = 0;
            foreach (var d in tempDistribution) total += d.percentage;
            
            if(totalPercentageText) 
                totalPercentageText.text = $"{total:F1}%";

            if(totalPercentageText)
                totalPercentageText.color = Mathf.Abs(total - 100f) < 0.1f ? Color.green : Color.red;
        }

        public void SaveChanges()
        {
             float total = 0;
            foreach (var d in tempDistribution) total += d.percentage;

            if (Mathf.Abs(total - 100f) > 0.1f)
            {
                Debug.LogError("Total must be 100%");
                return;
            }

            ONGManager.Instance.UpdateDistribution(tempDistribution);
        }
    }
}
