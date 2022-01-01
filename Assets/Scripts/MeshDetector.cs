using UnityEngine;
using UnityEngine.EventSystems;

// From https://answers.unity.com/questions/1736611/onmouse-events-eg-onmouseenter-not-working-with-ne.html
public class MeshDetector : MonoBehaviour
{
    void Start()
    {
        addPhysicsRaycaster();
    }
 
    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }
}
