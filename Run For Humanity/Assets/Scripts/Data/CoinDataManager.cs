using UnityEngine;
using System.IO;

namespace RunForHumanity.Data
{
    /// <summary>
    /// Sistema para guardar y cargar datos de monedas en JSON
    /// </summary>
    public static class CoinDataManager
    {
        [System.Serializable]
        private class CoinData
        {
            public int totalCoins = 0;
        }

        private static string SavePath => Path.Combine(Application.persistentDataPath, "coinData.json");
        private static CoinData currentData;

        /// <summary>
        /// Carga los datos de monedas desde el archivo JSON
        /// </summary>
        public static void LoadData()
        {
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    currentData = JsonUtility.FromJson<CoinData>(json);
                    Debug.Log($"[CoinDataManager] Monedas cargadas: {currentData.totalCoins}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[CoinDataManager] Error al cargar datos: {e.Message}");
                    currentData = new CoinData();
                }
            }
            else
            {
                currentData = new CoinData();
                SaveData();
                Debug.Log("[CoinDataManager] Nuevo archivo de datos creado");
            }
        }

        /// <summary>
        /// Guarda los datos de monedas en el archivo JSON
        /// </summary>
        public static void SaveData()
        {
            try
            {
                string json = JsonUtility.ToJson(currentData, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"[CoinDataManager] Monedas guardadas: {currentData.totalCoins}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CoinDataManager] Error al guardar datos: {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene el total de monedas actual
        /// </summary>
        public static int GetTotalCoins()
        {
            if (currentData == null) LoadData();
            return currentData.totalCoins;
        }

        /// <summary>
        /// Añade monedas al total
        /// </summary>
        public static void AddCoins(int amount)
        {
            if (currentData == null) LoadData();
            
            currentData.totalCoins += amount;
            SaveData();
            
            Debug.Log($"[CoinDataManager] +{amount} monedas. Total: {currentData.totalCoins}");
        }

        /// <summary>
        /// Gasta monedas (para la tienda)
        /// </summary>
        public static bool SpendCoins(int amount)
        {
            if (currentData == null) LoadData();
            
            if (currentData.totalCoins >= amount)
            {
                currentData.totalCoins -= amount;
                SaveData();
                Debug.Log($"[CoinDataManager] -{amount} monedas. Total: {currentData.totalCoins}");
                return true;
            }
            
            Debug.LogWarning($"[CoinDataManager] No hay suficientes monedas. Requeridas: {amount}, Disponibles: {currentData.totalCoins}");
            return false;
        }

        /// <summary>
        /// Resetea las monedas a 0 (para testing)
        /// </summary>
        public static void ResetCoins()
        {
            currentData = new CoinData();
            SaveData();
            Debug.Log("[CoinDataManager] Monedas reseteadas a 0");
        }
    }
}
