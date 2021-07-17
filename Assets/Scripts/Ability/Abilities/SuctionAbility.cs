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
        public override string Tooltip => $"Deal {Damage} (2 + {StrengthPercentage.ToPercentage()} STR) damage to a targeted enemy unit and increase your strength by {StrengthIncrease} (0.5 + {FocusPercentage.ToPercentage()} FOC).";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Buff,
            AbilityTag.EnemyTargeted
        };

        public float StrengthPercentage = 0.25f;
        public float Damage => 2 + StrengthPercentage * AbilityUser.strength;

        public float FocusPercentage = 0.25f;
        public float StrengthIncrease => 0.5f + FocusPercentage * AbilityUser.focus;
        
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
            AbilityUser.strength += StrengthIncrease;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}