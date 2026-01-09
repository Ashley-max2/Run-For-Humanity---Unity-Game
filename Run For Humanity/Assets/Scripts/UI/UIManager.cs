using UnityEngine;
using TMPro;
using DG.Tweening;
using RunForHumanity.Core.Events;

namespace RunForHumanity.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Views")]
        public GameObject menuView;
        public GameObject hudView;
        public GameObject gameOverView;

        [Header("HUD Elements")]
        public TextMeshProUGUI coinText;
        public TextMeshProUGUI distanceText;

        private int collectedCoins = 0;

        void Start()
        {
            EventManager.OnGameStart += ShowHUD;
            EventManager.OnGameOver += ShowGameOver;
            EventManager.OnCoinCollected += UpdateCoinDisplay;

            ShowMenu();
        }

        void OnDestroy()
        {
            EventManager.OnGameStart -= ShowHUD;
            EventManager.OnGameOver -= ShowGameOver;
            EventManager.OnCoinCollected -= UpdateCoinDisplay;
        }

        public void PlayButton()
        {
            EventManager.TriggerGameStart();
        }

        void ShowMenu()
        {
            SetViewActive(menuView, true);
            SetViewActive(hudView, false);
            SetViewActive(gameOverView, false);
        }

        void ShowHUD()
        {
            SetViewActive(menuView, false);
            SetViewActive(hudView, true);
            
            // Pop in animation
            if (hudView != null)
            {
                hudView.transform.localScale = Vector3.zero;
                hudView.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
            }
        }

        void ShowGameOver()
        {
            SetViewActive(hudView, false);
            SetViewActive(gameOverView, true);
            
            // Shake effect
            if (gameOverView != null)
            {
                gameOverView.transform.DOShakePosition(1f, 10f);
            }
        }

        void UpdateCoinDisplay(int amount)
        {
            collectedCoins += amount;
            
            if (coinText != null)
            {
                coinText.text = collectedCoins.ToString();
                coinText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
            }
        }

        private void SetViewActive(GameObject view, bool active)
        {
            if (view != null)
            {
                view.SetActive(active);
            }
        }
    }
}
