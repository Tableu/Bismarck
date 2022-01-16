using System;
using StarMap;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class StarSystemView : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public StarSystem SystemModel  { get; set; }

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = SystemModel.MainStar.MapImage;
        transform.localScale = new Vector3(SystemModel.StarSize, SystemModel.StarSize, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mouse over system: {SystemModel.SystemName}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Mouse click on system: {SystemModel.SystemName}");
    }
}
