using UnityEngine;
public abstract class ShipController : MonoBehaviour
{
    public abstract void Init(ShipData shipData);
    public abstract void Highlight();
    public abstract void DeHighlight();
}
