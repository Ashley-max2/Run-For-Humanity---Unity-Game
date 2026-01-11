using UnityEngine;
using System.IO;
using RunForHumanity.Data;

namespace RunForHumanity.Systems
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "playerdata.json";
        private string _savePath;
        
        public static SaveSystem Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SavePlayerData(PlayerData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(_savePath, json);
                Debug.Log($"[SaveSystem] Data saved to: {_savePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to save: {e.Message}");
            }
        }
        
        public PlayerData LoadPlayerData()
        {
            try
            {
                if (File.Exists(_savePath))
                {
                    string json = File.ReadAllText(_savePath);
                    PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log($"[SaveSystem] Data loaded from: {_savePath}");
                    return data;
                }
                else
                {
                    Debug.Log("[SaveSystem] No save file found, creating new player data");
                    return new PlayerData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to load: {e.Message}");
                return new PlayerData();
            }
        }
        
        public bool HasSaveData()
        {
            return File.Exists(_savePath);
        }
        
        public void DeleteSaveData()
        {
            try
            {
                if (File.Exists(_savePath))
                {
                    File.Delete(_savePath);
                    Debug.Log("[SaveSystem] Save data deleted");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] Failed to delete: {e.Message}");
            }
        }
    }
}
