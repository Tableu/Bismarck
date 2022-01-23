using System;

/// <summary>
/// Used to provide global access t game context information, such as if the game is in the store or battle scene.
/// </summary>
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