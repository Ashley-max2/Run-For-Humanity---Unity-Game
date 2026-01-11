using System;
using UnityEngine;

namespace RunForHumanity.Data
{
    [Serializable]
    public class ShopData
    {
        public int coinMultiplierLevel = 1;
        
        public int speedBoostLevel = 0;
        public int magnetLevel = 0;
    }
    
    public static class ShopDataManager
    {
        private const string SAVE_KEY = "ShopData";
        private static ShopData currentData;
        
        public static ShopData LoadData()
        {
            if (currentData != null) return currentData;
            
            string json = PlayerPrefs.GetString(SAVE_KEY, "");
            
            if (string.IsNullOrEmpty(json))
            {
                currentData = new ShopData();
                Debug.Log("[ShopDataManager] Datos nuevos creados");
            }
            else
            {
                currentData = JsonUtility.FromJson<ShopData>(json);
                Debug.Log($"[ShopDataManager] Datos cargados - Multiplicador nivel: {currentData.coinMultiplierLevel}");
            }
            
            return currentData;
        }
        
        public static void SaveData()
        {
            if (currentData == null)
            {
                Debug.LogWarning("[ShopDataManager] No hay datos para guardar");
                return;
            }
            
            string json = JsonUtility.ToJson(currentData, true);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            
            Debug.Log($"[ShopDataManager] Datos guardados - Multiplicador nivel: {currentData.coinMultiplierLevel}");
        }
        
        public static int GetCoinMultiplierLevel()
        {
            if (currentData == null) LoadData();
            return currentData.coinMultiplierLevel;
        }
        
        public static bool UpgradeCoinMultiplier(int cost)
        {
            if (currentData == null) LoadData();
            
            if (CoinDataManager.SpendCoins(cost))
            {
                currentData.coinMultiplierLevel++;
                SaveData();
                Debug.Log($"[ShopDataManager] Multiplicador mejorado a nivel {currentData.coinMultiplierLevel}");
                return true;
            }
            
            return false;
        }
        
        // Precio crece exponencialmente
        public static int GetNextMultiplierPrice(int basePrice = 100, float growthMultiplier = 1.5f)
        {
            if (currentData == null) LoadData();
            
            int currentLevel = currentData.coinMultiplierLevel;
            float price = basePrice * Mathf.Pow(growthMultiplier, currentLevel - 1);
            
            return Mathf.RoundToInt(price);
        }
        
        public static void ResetData()
        {
            currentData = new ShopData();
            SaveData();
            Debug.Log("[ShopDataManager] Datos reseteados");
        }
    }
}
