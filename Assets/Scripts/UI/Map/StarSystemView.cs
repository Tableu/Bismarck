using System;
using StarMap;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarSystemView : MonoBehaviour, IPointerEnterHandler
{
    public StarSystem SystemModel  { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mouse over system: {SystemModel.SystemName}");
    }
}
