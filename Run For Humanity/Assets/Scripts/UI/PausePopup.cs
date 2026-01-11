using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace RunForHumanity.UI
{
    public class PausePopup : MonoBehaviour
    {
        [System.Serializable]
        public class Mission
        {
            public string description;
            public int targetValue;
            public int coinReward;
            public bool completed;
        }

        [Header("UI References")]
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private Transform missionsContainer;
        [SerializeField] private GameObject missionPrefab;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;

        [Header("Mission Settings")]
        [SerializeField] private int baseMissionTarget = 100;
        [SerializeField] private float growthMultiplier = 1.5f;
        [SerializeField] private int baseCoinReward = 50;

        private List<Mission> missions = new List<Mission>();
        private GameplayUIController gameplayUI;

        private void Awake()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
        }

        private void Start()
        {
            gameplayUI = FindObjectOfType<GameplayUIController>();
            
            if (resumeButton != null)
                resumeButton.onClick.AddListener(Resume);
            
            if (homeButton != null)
                homeButton.onClick.AddListener(GoHome);

            GenerateMissions();
        }

        private void GenerateMissions()
        {
            missions.Clear();
            
            for (int i = 0; i < 3; i++)
            {
                Mission mission = new Mission
                {
                    description = GetMissionDescription(i),
                    targetValue = Mathf.RoundToInt(baseMissionTarget * Mathf.Pow(growthMultiplier, i)),
                    coinReward = Mathf.RoundToInt(baseCoinReward * Mathf.Pow(growthMultiplier, i)),
                    completed = false
                };
                missions.Add(mission);
            }
        }

        private string GetMissionDescription(int index)
        {
            string[] descriptions = new string[]
            {
                "Recorre {0} metros",
                "Recorre {0} metros",
                "Recorre {0} metros"
            };
            
            int target = Mathf.RoundToInt(baseMissionTarget * Mathf.Pow(growthMultiplier, index));
            return string.Format(descriptions[index % descriptions.Length], target);
        }

        public void Show()
        {
            if (popupPanel != null)
                popupPanel.SetActive(true);
            
            Time.timeScale = 0f;
            UpdateMissionsDisplay();
        }

        public void Hide()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
            
            Time.timeScale = 1f;
            Debug.Log("[PausePopup] Ocultado, Time.timeScale = 1");
        }

        private void UpdateMissionsDisplay()
        {
            if (missionsContainer == null || missionPrefab == null)
                return;

            foreach (Transform child in missionsContainer)
            {
                Destroy(child.gameObject);
            }

            float currentMeters = gameplayUI != null ? gameplayUI.GetCurrentMeters() : 0f;

            foreach (Mission mission in missions)
            {
                GameObject missionObj = Instantiate(missionPrefab, missionsContainer);
                
                TextMeshProUGUI missionText = missionObj.GetComponentInChildren<TextMeshProUGUI>();
                if (missionText != null)
                {
                    string status = currentMeters >= mission.targetValue ? "✓" : "";
                    missionText.text = $"{status} {mission.description} - Recompensa: {mission.coinReward} monedas";
                    
                    if (currentMeters >= mission.targetValue && !mission.completed)
                    {
                        mission.completed = true;
                        missionText.color = Color.green;
                    }
                }
            }
        }

        public void Resume()
        {
            Hide();
        }

        public void GoHome()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        [ContextMenu("Show Pause Popup")]
        public void ShowFromMenu()
        {
            Show();
        }
    }
}
