using UnityEngine;
using TMPro;
using RunForHumanity.Data;

namespace RunForHumanity.Gameplay
{
    // Contador de monedas en UI con animación
    public class CoinUICounter : MonoBehaviour
    {
        [Header("Referencias UI")]
        [SerializeField] private TextMeshProUGUI coinText;
        
        [Header("Formato")]
        [SerializeField] private string prefix = "";
        [SerializeField] private string suffix = " Coins";
        [SerializeField] private bool useThousandsSeparator = true;
        
        [Header("Animación")]
        [SerializeField] private bool animateOnChange = true;
        [SerializeField] private float animationDuration = 0.3f;
        
        private int currentDisplayedCoins = 0;
        private int targetCoins = 0;
        private float animationTimer = 0f;
        private int animationStartValue = 0;

        private void Start()
        {
            CoinDataManager.LoadData();
            
            if (coinText == null)
            {
                coinText = GetComponent<TextMeshProUGUI>();
            }
            
            UpdateDisplay();
        }

        private void Update()
        {
            if (animateOnChange && animationTimer < animationDuration)
            {
                animationTimer += Time.deltaTime;
                float progress = Mathf.Clamp01(animationTimer / animationDuration);
                
                // Interpolación suave
                currentDisplayedCoins = Mathf.RoundToInt(Mathf.Lerp(animationStartValue, targetCoins, progress));
                UpdateText();
            }
        }

        /// <summary>
        /// Actualiza el contador con el valor actual de monedas
        /// </summary>
        public void UpdateDisplay()
        {
            int newTotal = CoinDataManager.GetTotalCoins();
            
            if (animateOnChange && newTotal != targetCoins)
            {
                animationStartValue = currentDisplayedCoins;
                targetCoins = newTotal;
                animationTimer = 0f;
            }
            else
            {
                currentDisplayedCoins = newTotal;
                targetCoins = newTotal;
                UpdateText();
            }
        }

        /// <summary>
        /// Actualiza el texto en la UI
        /// </summary>
        void UpdateText()
        {
            if (coinText == null) return;
            
            string formattedNumber;
            if (useThousandsSeparator)
            {
                formattedNumber = currentDisplayedCoins.ToString("N0");
            }
            else
            {
                formattedNumber = currentDisplayedCoins.ToString();
            }
            
            coinText.text = $"{prefix}{formattedNumber}{suffix}";
        }

        /// <summary>
        /// Forzar actualización inmediata sin animación
        /// </summary>
        public void ForceUpdate()
        {
            currentDisplayedCoins = CoinDataManager.GetTotalCoins();
            targetCoins = currentDisplayedCoins;
            UpdateText();
        }
    }
}
