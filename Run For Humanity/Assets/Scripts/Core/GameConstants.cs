namespace RunForHumanity.Core
{
    /// <summary>
    /// Central constants for the game
    /// SOLID: Single Responsibility Principle
    /// </summary>
    public static class GameConstants
    {
        // Gameplay
        public const float INITIAL_SPEED = 5f;
        public const float MAX_SPEED = 20f;
        public const float SPEED_INCREMENT = 0.05f;
        public const float LANE_WIDTH = 2f;
        public const int LANE_COUNT = 3;
        public const float JUMP_FORCE = 8f;
        public const float SLIDE_DURATION = 0.8f;
        public const float DASH_SPEED_MULTIPLIER = 2f;
        public const float DASH_DURATION = 0.3f;
        
        // Track Generation
        public const float CHUNK_LENGTH = 50f;
        public const int CHUNKS_AHEAD = 3;
        public const float DESPAWN_DISTANCE = 30f;
        
        // Monetization
        public const float DONATION_PERCENTAGE = 0.8f; // 80% to ONGs
        public const float SUBSCRIPTION_PRICE = 2.99f;
        public const float SUBSCRIPTION_BONUS = 0.1f; // 10% extra impact
        
        // UI
        public const float UI_ANIMATION_DURATION = 0.3f;
        public const float NOTIFICATION_DISPLAY_TIME = 3f;
        
        // Audio
        public const float MASTER_VOLUME = 1f;
        public const float MUSIC_VOLUME = 0.7f;
        public const float SFX_VOLUME = 0.8f;
        
        // Sensors
        public const float ACCELEROMETER_THRESHOLD = 2f;
        public const float GYROSCOPE_THRESHOLD = 0.5f;
        
        // Player Stats
        public const int MAX_LEVEL = 100;
        public const float COINS_TO_IMPACT_RATIO = 100f; // 100 coins = $1
    }
}
