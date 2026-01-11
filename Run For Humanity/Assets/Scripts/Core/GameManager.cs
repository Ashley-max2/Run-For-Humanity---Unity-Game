using UnityEngine;
using System;
using System.Collections.Generic;

namespace RunForHumanity.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Game State")]
        [SerializeField] private GameState _currentState = GameState.MainMenu;
        
        private List<IInitializable> _systems = new List<IInitializable>();
        private List<IUpdatable> _updatableSystems = new List<IUpdatable>();
        
        public event Action<GameState> OnGameStateChanged;
        public event Action<float> OnScoreChanged;
        public event Action<int> OnCoinsChanged;
        
        public GameState CurrentState => _currentState;
        public float CurrentScore { get; private set; }
        public int TotalCoins { get; private set; }
        public bool IsPaused { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeSystems();
        }
        
        private void InitializeSystems()
        {
            Debug.Log($"[GameManager] Initialized systems");
        }
        
        private void RegisterSystem(MonoBehaviour system)
        {
            if (system is IInitializable initializable)
            {
                _systems.Add(initializable);
            }
            
            if (system is IUpdatable updatable)
            {
                _updatableSystems.Add(updatable);
            }
        }
        
        private void Update()
        {
            if (IsPaused) return;
            
            float deltaTime = Time.deltaTime;
            foreach (var system in _updatableSystems)
            {
                system.OnUpdate(deltaTime);
            }
        }
        
        public void StartGame()
        {
            ChangeState(GameState.Playing);
            CurrentScore = 0;
            TotalCoins = 0;
            IsPaused = false;
        }
        
        public void PauseGame()
        {
            IsPaused = true;
            Time.timeScale = 0f;
            ChangeState(GameState.Paused);
        }
        
        public void ResumeGame()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            ChangeState(GameState.Playing);
        }
        
        public void GameOver()
        {
            ChangeState(GameState.GameOver);
        }
        
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            ChangeState(GameState.MainMenu);
        }
        
        public void AddScore(float points)
        {
            CurrentScore += points;
            OnScoreChanged?.Invoke(CurrentScore);
        }
        
        public void AddCoins(int amount)
        {
            TotalCoins += amount;
            OnCoinsChanged?.Invoke(TotalCoins);
        }
        
        private void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;
            
            GameState oldState = _currentState;
            _currentState = newState;
            
            Debug.Log($"[GameManager] State changed: {oldState} -> {newState}");
            OnGameStateChanged?.Invoke(newState);
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && _currentState == GameState.Playing)
            {
                PauseGame();
            }
        }
    }
    
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        ONGSelection,
        Shop,
        Social
    }
}
