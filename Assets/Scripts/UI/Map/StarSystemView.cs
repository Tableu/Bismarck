using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarSystemView : MonoBehaviour, IPointerEnterHandler
{
    public int SystemID { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mouse over system{SystemID}");
    }
}
