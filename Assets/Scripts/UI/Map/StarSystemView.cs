using StarMap;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class StarSystemView : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public StarSystem SystemModel { get; set; }
        public MapView Parent { private get; set; }

        public void Start()
        {
            GetComponent<SpriteRenderer>().sprite = SystemModel.MainStar.MapImage;
            transform.localScale = new Vector3(SystemModel.StarSize, SystemModel.StarSize, 1);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(Parent.MoveIcon(SystemModel));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"Mouse over system: {SystemModel.SystemName}");
        }
    }
}