using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace RunForHumanity.UI
{
    /// <summary>
    /// Componente que genera un gráfico circular tipo donut para visualizar porcentajes de distribución
    /// Usado principalmente en la pantalla de selección de ONGs
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DonutChart : MonoBehaviour
    {
        [Header("Chart Settings")]
        [SerializeField] private float outerRadius = 150f;
        [SerializeField] private float innerRadius = 100f;
        [SerializeField] private int segments = 60; // Segmentos de círculo para suavidad
        [SerializeField] private float startAngle = -90f; // Comenzar desde arriba
        
        [Header("Visual Settings")]
        [SerializeField] private Color[] segmentColors = new Color[]
        {
            new Color(0.2f, 0.6f, 1f, 1f),    // Azul
            new Color(1f, 0.4f, 0.4f, 1f),    // Rojo
            new Color(0.4f, 1f, 0.4f, 1f),    // Verde
            new Color(1f, 0.8f, 0.2f, 1f),    // Amarillo
            new Color(0.8f, 0.4f, 1f, 1f)     // Morado
        };
        
        [SerializeField] private Material chartMaterial;
        [SerializeField] private float animationDuration = 0.5f;
        
        [Header("Runtime Data")]
        [SerializeField] private List<float> percentages = new List<float>();
        [SerializeField] private List<string> labels = new List<string>();
        
        private List<GameObject> chartSegments = new List<GameObject>();
        private RectTransform rectTransform;
        private Canvas parentCanvas;
        
        #region Initialization
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            parentCanvas = GetComponentInParent<Canvas>();
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Actualiza el gráfico con nuevos datos
        /// </summary>
        /// <param name="newPercentages">Array de porcentajes (deben sumar 100)</param>
        /// <param name="newLabels">Array de etiquetas para cada segmento</param>
        public void UpdateChart(float[] newPercentages, string[] newLabels = null)
        {
            // Validar que los porcentajes sumen 100
            float total = 0f;
            foreach (float percentage in newPercentages)
            {
                total += percentage;
            }
            
            if (Mathf.Abs(total - 100f) > 0.1f)
            {
                Debug.LogWarning($"DonutChart: Los porcentajes no suman 100% (suma: {total}%). Se normalizarán.");
                NormalizePercentages(ref newPercentages);
            }
            
            percentages.Clear();
            percentages.AddRange(newPercentages);
            
            if (newLabels != null)
            {
                labels.Clear();
                labels.AddRange(newLabels);
            }
            
            GenerateChart();
        }
        
        /// <summary>
        /// Actualiza el gráfico con animación
        /// </summary>
        public void UpdateChartAnimated(float[] newPercentages, string[] newLabels = null)
        {
            // Primero limpiar el gráfico actual
            ClearChart();
            
            // Actualizar datos
            float total = 0f;
            foreach (float percentage in newPercentages)
            {
                total += percentage;
            }
            
            if (Mathf.Abs(total - 100f) > 0.1f)
            {
                NormalizePercentages(ref newPercentages);
            }
            
            percentages.Clear();
            percentages.AddRange(newPercentages);
            
            if (newLabels != null)
            {
                labels.Clear();
                labels.AddRange(newLabels);
            }
            
            // Generar con animación
            GenerateChartAnimated();
        }
        
        /// <summary>
        /// Limpia todos los segmentos del gráfico
        /// </summary>
        public void ClearChart()
        {
            foreach (GameObject segment in chartSegments)
            {
                if (segment != null)
                {
                    Destroy(segment);
                }
            }
            chartSegments.Clear();
        }
        
        /// <summary>
        /// Establece los colores de los segmentos
        /// </summary>
        public void SetColors(Color[] colors)
        {
            segmentColors = colors;
            if (chartSegments.Count > 0)
            {
                GenerateChart(); // Regenerar con nuevos colores
            }
        }
        
        #endregion
        
        #region Private Methods - Chart Generation
        
        private void GenerateChart()
        {
            ClearChart();
            
            if (percentages.Count == 0)
            {
                Debug.LogWarning("DonutChart: No hay datos para mostrar.");
                return;
            }
            
            float currentAngle = startAngle;
            
            for (int i = 0; i < percentages.Count; i++)
            {
                if (percentages[i] <= 0) continue;
                
                float angle = (percentages[i] / 100f) * 360f;
                Color segmentColor = segmentColors[i % segmentColors.Length];
                
                GameObject segment = CreateSegment(currentAngle, angle, segmentColor, i);
                chartSegments.Add(segment);
                
                currentAngle += angle;
            }
        }
        
        private void GenerateChartAnimated()
        {
            ClearChart();
            
            if (percentages.Count == 0)
            {
                Debug.LogWarning("DonutChart: No hay datos para mostrar.");
                return;
            }
            
            float currentAngle = startAngle;
            float delay = 0f;
            
            for (int i = 0; i < percentages.Count; i++)
            {
                if (percentages[i] <= 0) continue;
                
                float angle = (percentages[i] / 100f) * 360f;
                Color segmentColor = segmentColors[i % segmentColors.Length];
                
                GameObject segment = CreateSegmentAnimated(currentAngle, angle, segmentColor, i, delay);
                chartSegments.Add(segment);
                
                currentAngle += angle;
                delay += animationDuration * 0.2f; // Escalonar animaciones
            }
        }
        
        private GameObject CreateSegment(float startAngle, float angle, Color color, int index)
        {
            GameObject segmentObj = new GameObject($"Segment_{index}");
            segmentObj.transform.SetParent(transform, false);
            
            RectTransform segmentRect = segmentObj.AddComponent<RectTransform>();
            segmentRect.anchorMin = new Vector2(0.5f, 0.5f);
            segmentRect.anchorMax = new Vector2(0.5f, 0.5f);
            segmentRect.pivot = new Vector2(0.5f, 0.5f);
            segmentRect.sizeDelta = new Vector2(outerRadius * 2, outerRadius * 2);
            segmentRect.anchoredPosition = Vector2.zero;
            
            Image image = segmentObj.AddComponent<Image>();
            image.sprite = CreateSegmentSprite(startAngle, angle);
            image.color = color;
            
            if (chartMaterial != null)
            {
                image.material = chartMaterial;
            }
            
            return segmentObj;
        }
        
        private GameObject CreateSegmentAnimated(float startAngle, float angle, Color color, int index, float delay)
        {
            GameObject segment = CreateSegment(startAngle, angle, color, index);
            
            // Animar desde transparente
            Image image = segment.GetComponent<Image>();
            Color transparent = color;
            transparent.a = 0f;
            image.color = transparent;
            
            // Animar escala desde 0
            RectTransform rect = segment.GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            
            // Secuencia de animación
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(rect.DOScale(1f, animationDuration).SetEase(Ease.OutBack));
            sequence.Join(image.DOColor(color, animationDuration).SetEase(Ease.OutQuad));
            
            return segment;
        }
        
        private Sprite CreateSegmentSprite(float startAngle, float angle)
        {
            // Crear textura para el segmento
            int textureSize = 512;
            Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            
            // Limpiar textura
            Color[] pixels = new Color[textureSize * textureSize];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.clear;
            }
            
            // Dibujar segmento
            Vector2 center = new Vector2(textureSize / 2f, textureSize / 2f);
            float outerRadiusPixels = (outerRadius / outerRadius) * (textureSize / 2f);
            float innerRadiusPixels = (innerRadius / outerRadius) * (textureSize / 2f);
            
            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    Vector2 pixelPos = new Vector2(x, y);
                    Vector2 diff = pixelPos - center;
                    float distance = diff.magnitude;
                    float pixelAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    
                    // Normalizar ángulo
                    pixelAngle = (pixelAngle + 360f) % 360f;
                    float normalizedStartAngle = (startAngle + 360f) % 360f;
                    float normalizedEndAngle = (normalizedStartAngle + angle) % 360f;
                    
                    bool inAngleRange = false;
                    if (normalizedStartAngle < normalizedEndAngle)
                    {
                        inAngleRange = pixelAngle >= normalizedStartAngle && pixelAngle <= normalizedEndAngle;
                    }
                    else
                    {
                        inAngleRange = pixelAngle >= normalizedStartAngle || pixelAngle <= normalizedEndAngle;
                    }
                    
                    if (distance >= innerRadiusPixels && distance <= outerRadiusPixels && inAngleRange)
                    {
                        pixels[y * textureSize + x] = Color.white;
                    }
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            // Crear sprite desde textura
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, textureSize, textureSize),
                new Vector2(0.5f, 0.5f),
                textureSize / (outerRadius * 2f)
            );
            
            return sprite;
        }
        
        #endregion
        
        #region Utility Methods
        
        private void NormalizePercentages(ref float[] percentages)
        {
            float total = 0f;
            foreach (float percentage in percentages)
            {
                total += percentage;
            }
            
            if (total > 0)
            {
                for (int i = 0; i < percentages.Length; i++)
                {
                    percentages[i] = (percentages[i] / total) * 100f;
                }
            }
        }
        
        #endregion
        
        #region Editor Methods
        
#if UNITY_EDITOR
        [ContextMenu("Test Chart - 3 Segments")]
        private void TestChart3Segments()
        {
            float[] testPercentages = new float[] { 50f, 30f, 20f };
            string[] testLabels = new string[] { "ONG A", "ONG B", "ONG C" };
            UpdateChartAnimated(testPercentages, testLabels);
        }
        
        [ContextMenu("Test Chart - 4 Segments")]
        private void TestChart4Segments()
        {
            float[] testPercentages = new float[] { 40f, 30f, 20f, 10f };
            string[] testLabels = new string[] { "ONG A", "ONG B", "ONG C", "ONG D" };
            UpdateChartAnimated(testPercentages, testLabels);
        }
        
        [ContextMenu("Clear Chart")]
        private void TestClearChart()
        {
            ClearChart();
        }
#endif
        
        #endregion
    }
}
