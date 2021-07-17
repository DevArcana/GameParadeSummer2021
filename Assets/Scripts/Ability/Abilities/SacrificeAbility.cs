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
        public override string Tooltip => $"Execute yourself and increase strength of all allied units by {StrengthIncrease} (0.5 + {FocusPercentage.ToPercentage()} FOC)";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Sacrifice,
            AbilityTag.NoTarget,
            AbilityTag.Buff
        };

        public float FocusPercentage = 0.25f;
        public float StrengthIncrease => 0.5f + FocusPercentage * AbilityUser.focus;
        
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
            AbilityUser.Execute();
            
            foreach (var ally in TurnManager.Instance.EnqueuedEntities.Where(x => x != AbilityUser && x.GetType() == AbilityUser.GetType()))
            {
                ally.strength += StrengthIncrease;
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}