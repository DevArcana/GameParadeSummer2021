using System;
using System.Collections;
using System.Linq;
using Interface;
using JetBrains.Annotations;
using UnityEngine;

namespace Arena
{
    [RequireComponent(typeof(AudioSource))]
    public class GridEntity : MonoBehaviour
    {
        public AudioClip[] moveSounds;
        private AudioSource _audio;
        
        public HealthBarUI healthBar;

        public float health;
        public float maxHealth;
        public float damage;
        
        public float smoothTime = 1.0f;
        private Vector3 _velocity;
        
        protected virtual void Start()
        {
            _audio = GetComponent<AudioSource>();
            GameArena.Instance.Register(this);
        }

        protected virtual void OnDestroy()
        {
            GameArena.Instance.Kill(this);
        }
        
        public IEnumerator Move(Vector3 pos, [CanBeNull] Action finish = null)
        {
            if (!(moveSounds is null) && moveSounds.Any())
            {
                _audio.PlayOneShot(moveSounds.OrderBy(x => Guid.NewGuid()).FirstOrDefault(x => !(x is null)));
            }
            
            GameArena.Instance.Grid.WorldToGrid(pos, out var x, out var y);
            var entity = GameArena.Instance.Grid[x, y];
            if (entity.GetType() != this.GetType())
            {
                entity.health -= this.damage;
                entity.healthBar.SetHealth(entity.health, entity.maxHealth);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                while (transform.position != pos)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, pos, ref _velocity, smoothTime);
                    yield return new WaitForEndOfFrame();
                }
            }

            finish?.Invoke();
        }
    }
}