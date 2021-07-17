using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SacrificeAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Sacrifice";
        public override string Tooltip => $"Execute yourself and increase damage of all allied units by {DamageIncrease}";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Sacrifice,
            AbilityTag.NoTarget,
            AbilityTag.Buff
        };

        public int DamageIncrease = 1;
        
        public SacrificeAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return TurnManager.Instance.EnqueuedEntities.Any(x =>
                x != AbilityUser && x.GetType() == AbilityUser.GetType());
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            AbilityUser.TakeDamage(AbilityUser.health);
            
            foreach (var ally in TurnManager.Instance.EnqueuedEntities.Where(x => x != AbilityUser && x.GetType() == AbilityUser.GetType()))
            {
                ally.strength += 1;
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}