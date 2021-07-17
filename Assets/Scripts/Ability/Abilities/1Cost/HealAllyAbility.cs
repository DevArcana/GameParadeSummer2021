using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class HealAllyAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Heal Ally";
        public override string Tooltip => $"Restore {Heal} (2 + {FocusPercentage.ToPercentage()} FOC) health to an allied unit other than yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.SelfTargeted
        };

        public float FocusPercentage = 0.5f;
        public float Heal => 2 + FocusPercentage * AbilityUser.focus;
        
        public HealAllyAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity != AbilityUser && targetEntity.GetType() == AbilityUser.GetType() && targetEntity.health < targetEntity.maxHealth;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.Heal(Heal);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}