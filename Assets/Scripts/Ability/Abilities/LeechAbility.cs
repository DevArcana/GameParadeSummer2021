using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class LeechAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Leech";
        public override string Tooltip => $"Deal {Damage} damage to a targeted allied unit and restore {HealAmount} health to yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.AllyTargeted
        };

        public int Damage = 2;
        public int HealAmount = 4;
        
        public LeechAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity != AbilityUser && targetEntity.GetType() == AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            AbilityUser.Heal(HealAmount);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}