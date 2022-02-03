using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarMap;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Map
{
    public class MapView : MonoBehaviour
    {
        private const float JumpTime = 1;
        [SerializeField] private GameObject _starSystemPrefab;
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private GameObject _playerIconPrefab;
        [SerializeField] private MapData _mapData;
        [SerializeField] private MapContext _context;
        [SerializeField] private Camera _mapCamera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private PlayerInputScriptableObject _playerInput;

        private readonly List<GameObject> _systemViews = new List<GameObject>();

        private Action<InputAction.CallbackContext> _hideMap;
        private GameObject _icon;

        public Camera MapCamera => _mapCamera;

        private void Awake()
        {
            _hideMap = _ => _context.HideMapView();
            foreach (var system in _mapData.StarSystems)
            {
                var systemViewGo = Instantiate(_starSystemPrefab, (Vector3)system.Coordinates + Vector3.back,
                    Quaternion.identity, transform);
                var systemView = systemViewGo.GetComponent<StarSystemView>();
                systemView.SystemModel = system;
                systemView.Parent = this;
                _systemViews.Add(systemViewGo);
            }

            foreach (var systemPair in _mapData.SystemPairs)
            {
                var srcSystem = systemPair.System1;
                var dstSystem = systemPair.System2;

                var srcPos = srcSystem.Coordinates;
                var dstPos = dstSystem.Coordinates;
                var lane = Instantiate(_linePrefab, Vector3.down, Quaternion.identity, transform);
                var lr = lane.GetComponent<LineRenderer>();
                lr.SetPosition(0, srcPos);
                lr.SetPosition(1, dstPos);
            }

            _icon = Instantiate(_playerIconPrefab, _context.CurrentSystem.Coordinates, Quaternion.identity,
                transform);
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.UI.Cancel.performed += _hideMap;
            _mapCamera.enabled = true;
            _virtualCamera.enabled = true;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.UI.Cancel.performed -= _hideMap;
            // lifetime check for when scene is unloaded/destroyed
            if (_mapCamera != null)
            {
                _mapCamera.enabled = false;
            }

            if (_virtualCamera != null)
            {
                _virtualCamera.enabled = false;
            }
        }

        public IEnumerator MoveIcon(StarSystem destination)
        {
            if (!_context.IsAccessible(destination))
            {
                yield break;
            }

            var srcPos = _icon.transform.position;
            var dstPos = new Vector3(destination.Coordinates.x, destination.Coordinates.y, srcPos.z);
            float elapsedTime = 0;

            while (elapsedTime < JumpTime)
            {
                _icon.transform.position = Vector3.Lerp(srcPos, dstPos, elapsedTime / JumpTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _context.LoadSystem(destination);
            Debug.Log($"Jumped to {destination.SystemName}");
        }
    }
}
