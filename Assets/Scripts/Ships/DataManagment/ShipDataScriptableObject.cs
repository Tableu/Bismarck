using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipData")]
[Serializable]
public class ShipDataScriptableObject : ScriptableObject
{
    public ShipData ShipData;

    public ShipData MakeShipData()
    {
        return ShipData.Copy();
    }
}