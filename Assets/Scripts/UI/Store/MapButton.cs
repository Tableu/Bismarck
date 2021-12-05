using UnityEngine;

public class MapButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMapButtonClick(bool close)
    {
        if (close)
        {
            TransitionManager.Instance.CloseMap();
        }
        else
        {
            TransitionManager.Instance.OpenMap();
        }
    }
}
