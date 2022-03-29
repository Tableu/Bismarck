using UnityEngine;

namespace UI
{
    /// <summary>
    ///     A generic class for UI components that binds to a specified object, reading data and following its position
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BindableUIComponent<T> : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        [SerializeField] protected Vector2 displacement;
        [SerializeField] protected RectTransform rectTransform;

        protected virtual void Update()
        {
            if (target != null)
            {
                gameObject.transform.position = (Vector2) target.position + displacement;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogError("BindableUIComponent: target is null");
            }
        }

        public abstract void Bind(T target);
    }
}