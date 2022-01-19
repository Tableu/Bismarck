using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSelectionBox : MonoBehaviour
{
    public Transform Canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 canvasScale = Canvas.transform.localScale;
        transform.localScale = new Vector3(1/canvasScale.x, 1/canvasScale.y, 1/canvasScale.z);
    }
}
