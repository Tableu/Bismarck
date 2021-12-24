using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput/PlayerInput")]
public class PlayerInputScriptableObject : ScriptableObject
{
    public PlayerInputActions PlayerInputActions;

    public ContactFilter2D PlayerFilter; 
    private void OnEnable()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("PlayerShips"),
            useLayerMask = true
        };
    }
}
