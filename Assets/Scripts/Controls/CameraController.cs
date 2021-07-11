using System;
using UnityEngine;

namespace Controls
{
    public class CameraController : MonoBehaviour
    {
        public float turnMod = 0.3f;
        public float speed = 15.0f;
        public float smoothing = 0.05f;
        public float minDistance = 1.0f;
        public float maxDistance = 15.0f;

        private float _distanceSmooth;
        private float _distanceVelocity = 0.0f;
        private float _distance = 10.0f;
        private float _phi = 15.0f;
        private float _theta = 0.0f;        
        
        private Vector3 _origin;
        private Vector3 _smoothOrigin;
        private Vector3 _smoothingVelocity;
        private Vector3 _towardsCamera;
        private Vector3 _right;

        private Vector2 _lastMousePos;

        private void Start()
        {
            _origin = transform.position;
            _smoothingVelocity = Vector3.zero;

            _distanceSmooth = _distance;

            AdjustOriginToGround();
            _smoothOrigin = _origin;
            LookAtOrigin();
        }

        private void AdjustOriginToGround()
        {
            if (Physics.Raycast(new Ray(_origin, Vector3.down), out var hit))
            {
                _origin = hit.point;
            }
        }

        private void LookAtOrigin()
        {
            _towardsCamera = Quaternion.AngleAxis(_theta, Vector3.up) * Vector3.right;
            _right = Vector3.Cross(_towardsCamera, Vector3.up);
            
            transform.position = _smoothOrigin + Quaternion.AngleAxis(_phi, _right) * _towardsCamera * _distanceSmooth;
            transform.LookAt(_smoothOrigin);
        }

        private void Update()
        {
            _smoothOrigin = Vector3.SmoothDamp(_smoothOrigin, _origin, ref _smoothingVelocity, smoothing);
            _distanceSmooth = Mathf.SmoothDamp(_distanceSmooth, _distance, ref _distanceVelocity, smoothing);
            
            var updateCamera = _smoothOrigin != _origin || Math.Abs(_distanceSmooth - _distance) > 0.001f;

            var moveRight = Input.GetAxisRaw("Horizontal");
            var moveForward = Input.GetAxisRaw("Vertical");

            if (moveRight != 0.0f || moveForward != 0.0f)
            {
                var move = Vector3.Normalize(_towardsCamera * -moveForward + _right * moveRight);
                _origin += move * (speed * _distanceSmooth * Time.deltaTime);
                updateCamera = true;
            }

            var mouseScrollDelta = -Input.mouseScrollDelta.y;

            if (mouseScrollDelta != 0.0f)
            {
                _distance += mouseScrollDelta;
                _distance = Mathf.Clamp(_distance, minDistance, maxDistance);
                updateCamera = true;
            }
            
            if (Input.GetMouseButtonDown(2))
            {
                _lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                var mousePos = Input.mousePosition;
                var dx = mousePos.x - _lastMousePos.x;
                var dy = mousePos.y - _lastMousePos.y;
                _lastMousePos = mousePos;

                _theta += dx * turnMod;
                _phi += -dy * turnMod;

                if (_theta < 0.0f) _theta += 360.0f;
                if (_theta >= 360.0f) _theta -= 360.0f;
                
                _phi = Mathf.Clamp(_phi, 1.0f, 89.0f);
                updateCamera = true;
            }

            if (updateCamera)
            {
                LookAtOrigin();
            }
        }
    }
}
