using System;
using UnityEngine;

namespace RunForHumanity.Data
{
    /// <summary>
    /// Current game session data
    /// SOLID: Single Responsibility - Session data storage
    /// </summary>
    [Serializable]
    public class GameSessionData
    {
        public float distance;
        public int coinsCollected;
        public int powerUpsUsed;
        public int obstaclesHit;
        public float playTime;
        public DateTime sessionStart;
        public float moneyGenerated;
        
        public void Reset()
        {
            distance = 0;
            coinsCollected = 0;
            powerUpsUsed = 0;
            obstaclesHit = 0;
            playTime = 0;
            sessionStart = DateTime.Now;
            moneyGenerated = 0;
        }
        
        public void CalculateRevenue()
        {
            // Revenue from coins collected
            moneyGenerated = coinsCollected / 100f; // 100 coins = $1
        }
    }
    
    /// <summary>
    /// Configuration for power-ups
    /// </summary>
    [Serializable]
    public class PowerUpData
    {
        public string id;
        public string name;
        public PowerUpType type;
        public float duration;
        public float multiplier;
        public Sprite icon;
        
        public enum PowerUpType
        {
            CoinMagnet,
            Shield,
            SpeedBoost,
            ScoreMultiplier,
            DoubleCoins
        }
    }
    
    /// <summary>
    /// Configuration for obstacles
    /// </summary>
    [Serializable]
    public class ObstacleData
    {
        public string id;
        public string name;
        public ObstacleType type;
        public int damage;
        public float spawnWeight;
        
        public enum ObstacleType
        {
            Low,      // Requires jump
            High,     // Requires slide
            Middle,   // Requires lane change
            Moving    // Dynamic obstacle
        }
    }
}
