using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    public bool occupied;
    private Image _image;
    private Button _button;

    private LayerMask _layerMask;
    // Start is called before the first frame update
    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _image = gameObject.GetComponent<Image>();
        _button.onClick.AddListener(OnClick);
        _layerMask = LayerMask.GetMask("Player", "Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, _layerMask);
        _image.enabled = !hit;
        _button.enabled = !hit;
    }

    private void OnClick()
    {
        InputManager.Instance.GridItemSelected(gameObject);
    }
}
