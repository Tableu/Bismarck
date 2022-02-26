using UnityEngine;

namespace Scene
{
    /// <summary>
    ///     Defines the position difference between ShipVisuals in the world
    /// </summary>
    public class ShipVisualsManager : MonoBehaviour
    {
        private static ShipVisualsManager _instance;

        public static ShipVisualsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ShipVisualsManager>();
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        _instance = gameObject.AddComponent<ShipVisualsManager>();
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null || _instance == this)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _availablePos = -padding;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        private Vector2 _availablePos;
        [SerializeField] private Vector2 padding = new Vector2(20, 20);

        public Vector2 GetPosition(GameObject shipVisual)
        {
            Vector2 pos = _availablePos;
            SpriteRenderer sr = shipVisual.GetComponent<SpriteRenderer>();
            Bounds bounds = sr.sprite.bounds;
            _availablePos = new Vector2(pos.x - bounds.size.x - padding.x, pos.y);
            return pos;
        }

        public Transform GetParent()
        {
            return transform;
        }
    }
}