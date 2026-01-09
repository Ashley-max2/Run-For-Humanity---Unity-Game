using System;

namespace RunForHumanity.Core.Events
{
    public static class EventManager
    {
        // Gameplay Events
        public static event Action OnGameStart;
        public static event Action OnGameOver;
        public static event Action<int> OnCoinCollected;
        public static event Action OnPlayerJump;
        public static event Action OnPlayerHit;
        
        // System Events
        public static event Action<float> OnTrackSpeedChanged;

        public static void TriggerGameStart() => OnGameStart?.Invoke();
        public static void TriggerGameOver() => OnGameOver?.Invoke();
        public static void TriggerCoinCollected(int amount) => OnCoinCollected?.Invoke(amount);
        public static void TriggerPlayerJump() => OnPlayerJump?.Invoke();
        public static void TriggerPlayerHit() => OnPlayerHit?.Invoke();
        public static void TriggerTrackSpeedChanged(float newSpeed) => OnTrackSpeedChanged?.Invoke(newSpeed);
    }
}
