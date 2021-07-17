using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class HealPlayerAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Heal Player";
        public override string Tooltip => $"Restore {HealAmount} health to yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.SelfTargeted
        };

        public const int HealAmount = 4;
        
        public HealPlayerAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity == AbilityUser && targetEntity.health < targetEntity.maxHealth;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.Heal(HealAmount);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}