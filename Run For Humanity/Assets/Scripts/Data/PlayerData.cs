using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunForHumanity.Data
{
    /// <summary>
    /// Player persistent data
    /// SOLID: Single Responsibility - Player data storage
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public string userId;
        public string playerName;
        public int level;
        public float totalDistance;
        public int totalCoins;
        public float totalDonated;
        public int gamesPlayed;
        public float bestDistance;
        public int bestCoins;
        public DateTime lastPlayed;
        public DateTime accountCreated;
        
        // Preferences
        public bool hasPremiumSubscription;
        public DateTime subscriptionExpiry;
        public List<string> unlockedSkins = new List<string>();
        public string currentSkin = "default";
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool vibrationsEnabled = true;
        public bool notificationsEnabled = true;
        
        // Social
        public List<string> friendIds = new List<string>();
        public string currentGroupId;
        public Dictionary<string, int> achievements = new Dictionary<string, int>();
        
        public PlayerData()
        {
            userId = System.Guid.NewGuid().ToString();
            playerName = "Runner";
            level = 1;
            accountCreated = DateTime.Now;
            lastPlayed = DateTime.Now;
        }
    }
}
