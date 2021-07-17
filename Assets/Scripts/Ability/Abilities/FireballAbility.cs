using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class FireballAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Fireball";
        public override string Tooltip => $"Deal {Damage} damage to a targeted enemy unit.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.EnemyTargeted,
            AbilityTag.Ranged
        };

        public int Damage = 3;
        
        public FireballAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}