using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Arena
{
    public class GridEntity : MonoBehaviour
    {
        public float smoothTime = 1.0f;
        private Vector3 _velocity;
        private void Start()
        {
            GameArena.Instance.Register(this);
        }

        public IEnumerator Move(Vector3 pos, [CanBeNull] Action finish = null)
        {
            while (transform.position != pos)
            {
                transform.position = Vector3.SmoothDamp(transform.position, pos, ref _velocity, smoothTime * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            finish?.Invoke();
        }
    }
}