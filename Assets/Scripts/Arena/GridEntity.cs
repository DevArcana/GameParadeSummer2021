using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Arena
{
    [RequireComponent(typeof(AudioSource))]
    public class GridEntity : MonoBehaviour
    {
        public AudioClip[] moveSounds;
        private AudioSource _audio;
        
        public float smoothTime = 1.0f;
        private Vector3 _velocity;
        
        protected virtual void Start()
        {
            _audio = GetComponent<AudioSource>();
            GameArena.Instance.Register(this);
        }

        public IEnumerator Move(Vector3 pos, [CanBeNull] Action finish = null)
        {
            if (!(moveSounds is null) && moveSounds.Any())
            {
                _audio.PlayOneShot(moveSounds.OrderBy(x => Guid.NewGuid()).FirstOrDefault(x => !(x is null)));
            }
            
            while (transform.position != pos)
            {
                transform.position = Vector3.SmoothDamp(transform.position, pos, ref _velocity, smoothTime);
                yield return new WaitForEndOfFrame();
            }

            finish?.Invoke();
        }
    }
}