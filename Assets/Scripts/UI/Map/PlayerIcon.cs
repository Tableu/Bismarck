using System.Collections;
using UnityEngine;

namespace UI.Map
{
    public class PlayerIcon : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
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
}