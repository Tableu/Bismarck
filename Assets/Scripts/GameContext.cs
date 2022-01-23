using System;

namespace DefaultNamespace
{
    public class GameContext
    {
        public enum GameState
        {
            StoreMode,
            BattleMode
        }

        private GameState _gameState = GameState.StoreMode;

        private GameContext()
        {
        }

        public static GameContext Instance { get; } = new GameContext();

        public GameState CurrentState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnContextChange?.Invoke();
            }
        }

        public event Action OnContextChange;
    }
}