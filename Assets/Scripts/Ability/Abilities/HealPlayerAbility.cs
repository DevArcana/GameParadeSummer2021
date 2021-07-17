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
        public override string Tooltip => $"Restore {Heal} (3 + {FocusPercentage.ToPercentage()} FOC) health to yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.SelfTargeted
        };

        public float FocusPercentage = 0.5f;
        public float Heal => 3 + FocusPercentage * AbilityUser.focus;
        
        public HealPlayerAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity == AbilityUser && targetEntity.health < targetEntity.maxHealth;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.Heal(Heal);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}