using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    [Serializable]
    public class SpeedSegment
    {
        public float speed;
        public float duration;
    }

    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;
        [SerializeField] private List<SpeedSegment> speedSegments;

        public float speed;
        public Vector2 _direction;
        private SpeedSegment _currentSegment;
        private int _speedSegmentIndex;
        private int _speedSegmentMax;
        private bool _stopTimer;
        private float startTime;

        protected void Start()
        {
            if (speedSegments != null && speedSegments.Count != 0)
            {
                startTime = Time.fixedTime;
                _speedSegmentIndex = 0;
                _speedSegmentMax = speedSegments.Count;
                _stopTimer = false;
                _currentSegment = speedSegments[0];
                speed = _currentSegment.speed;
            }
            else
            {
                _stopTimer = true;
            }
        }
        
        protected void FixedUpdate()
        {
            transform.Translate(new Vector2(_direction.x, _direction.y) * speed * Time.fixedDeltaTime);

            if (!_stopTimer)
            {
                if (Time.fixedTime - startTime > _currentSegment.duration)
                {
                    _speedSegmentIndex++;
                    if (_speedSegmentIndex >= _speedSegmentMax)
                    {
                        _stopTimer = true;
                        return;
                    }

                    startTime = Time.fixedTime;
                    _currentSegment = speedSegments[_speedSegmentIndex];
                    speed = _currentSegment.speed;
                }
            }
        }

        public void Init(Vector2 direction, float zRotation)
        {
            _direction = direction;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(scale.x * Mathf.Sign(direction.x), scale.y, scale.z);
            Vector3 rotation = visuals.transform.rotation.eulerAngles;
            visuals.transform.rotation = Quaternion.Euler(rotation.x, rotation.y,
                (rotation.z - zRotation) * Mathf.Sign(direction.x));
        }
    }
}