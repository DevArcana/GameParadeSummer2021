using System;
using System.Collections;
using System.Linq;
using Ability;
using Ability.Abilities;
using Interface;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    [RequireComponent(typeof(AudioSource))]
    public class GridEntity : MonoBehaviour
    {
        public Animator animator;
        public AudioClip[] moveSounds;
        private AudioSource _audio;

        public string EntityName { get; set; } = Utilities.RandomizeName();
        
        public HealthBarUI healthBar;

        #region Attributes
        
        public float strength;
        public float focus;
        public float agility;
        
        public float health;
        public float maxHealth;
        public float armour;

        #endregion
        
        public float smoothTime = 1.0f;
        private Vector3 _velocity;

        protected BasicMoveAbility moveAbility;
        protected BasicAttackAbility attackAbility;
        public readonly AbilitySlots abilitySlots = new AbilitySlots();

        protected virtual void Start()
        {
            _audio = GetComponent<AudioSource>();
            GameArena.Instance.Register(this);

            moveAbility = new BasicMoveAbility(this);
            attackAbility = new BasicAttackAbility(this);
            
            abilitySlots.PopulateAbilities(this);
        }

        protected virtual void OnDestroy()
        {
            var arena = GameArena.Instance;
            if (arena != null)
            {
                arena.Kill(this);
            }
        }
        
        public IEnumerator Move(Vector3 pos, [CanBeNull] Action finish = null)
        {
            if (!(moveSounds is null) && moveSounds.Any())
            {
                _audio.PlayOneShot(moveSounds.OrderBy(x => Guid.NewGuid()).FirstOrDefault(x => !(x is null)));
            }
            
            GameArena.Instance.Grid.WorldToGrid(pos, out var x, out var y);
            while (transform.position != pos)
            {
                transform.position = Vector3.SmoothDamp(transform.position, pos, ref _velocity, smoothTime);
                yield return new WaitForEndOfFrame();
            }

            finish?.Invoke();
        }

        public void TakeDamage(float amount, bool ignoreArmour = false)
        {
            if (armour > 0 && !ignoreArmour)
            {
                if (amount > armour)
                {
                    health -= (amount - armour);
                    armour = 0;
                }
                else
                {
                    armour -= amount;
                }
            }
            else
            {
                health -= amount;
            }

            if (health <= 0)
            {
                Destroy(gameObject);
                return;
            }
            
            healthBar.SetHealth(health, maxHealth);
        }

        public void Execute()
        {
            health = 0;
            Destroy(gameObject);
        }

        public void Heal(float amount)
        {
            health = Math.Min(health + amount, maxHealth);
            
            healthBar.SetHealth(health, maxHealth);
        }
    }
}