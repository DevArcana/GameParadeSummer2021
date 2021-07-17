using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ExecuteAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Execute";
        public override string Tooltip => $"Deal {Damage} damage to a targeted enemy unit. If targeted unit has {ExecuteThreshold} or less health, execute it.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Execute,
            AbilityTag.EnemyTargeted,
            AbilityTag.SingleTarget
        };

        public int Damage = 3;
        public int ExecuteThreshold = 8;
        
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
                targetEntity.TakeDamage(targetEntity.health);
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