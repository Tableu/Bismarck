using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MovePlayer(Vector3 target)
    {
        while (!transform.position.Equals(target))
        {
            transform.Translate(target);
            yield return null;
        }
    }
}
