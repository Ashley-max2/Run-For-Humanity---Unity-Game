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
        
        // Donation Preferences
        public List<ONGDistribution> ongDistribution = new List<ONGDistribution>();
        public float pendingDonationAmount;
        public DateTime nextDonationDate;
        public List<DonationCertificate> certificates = new List<DonationCertificate>();
        
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
            
            // Default ONG distribution (equal 3 ONGs)
            ongDistribution.Add(new ONGDistribution("water_org", 33.33f));
            ongDistribution.Add(new ONGDistribution("education_fund", 33.33f));
            ongDistribution.Add(new ONGDistribution("health_world", 33.34f));
        }
    }
    
    [Serializable]
    public class DonationCertificate
    {
        public string certificateId;
        public DateTime date;
        public float amount;
        public Dictionary<string, float> ongBreakdown;
        public string impactDescription;
        
        public DonationCertificate(float amount, Dictionary<string, float> breakdown)
        {
            certificateId = System.Guid.NewGuid().ToString();
            date = DateTime.Now;
            this.amount = amount;
            this.ongBreakdown = breakdown;
        }
    }
}
