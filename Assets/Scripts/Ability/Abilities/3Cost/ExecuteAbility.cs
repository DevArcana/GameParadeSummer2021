using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ExecuteAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Execute";
        public override string Tooltip => $"Deal {Damage} (2 + {DamageStrengthPercentage.ToPercentage()} STR) damage to a targeted enemy unit. If targeted unit has {ExecuteThreshold} (4 + {ExecuteStrengthPercentage.ToPercentage()} STR) or less health, execute it.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Execute,
            AbilityTag.EnemyTargeted,
            AbilityTag.SingleTarget
        };

        public float DamageStrengthPercentage = 0.25f;
        public float Damage => 2 + DamageStrengthPercentage * AbilityUser.strength;
        
        public float ExecuteStrengthPercentage = 1.0f;
        public float ExecuteThreshold => 4 + ExecuteStrengthPercentage * AbilityUser.strength;
        
        public ExecuteAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            if (targetEntity.health <= ExecuteThreshold)
            {
                targetEntity.Execute();
            }
            else
            {
                targetEntity.TakeDamage(Damage);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}