using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SuctionAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Suction";
        public override string Tooltip => $"Deal {Damage} damage to a targeted enemy unit and increase your damage by {DamageIncrease}.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Buff,
            AbilityTag.EnemyTargeted
        };

        public int Damage = 2;
        public int DamageIncrease = 1;
        
        public SuctionAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            AbilityUser.strength += 1;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}