﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SalvationAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Salvation";
        public override string Tooltip => $"Restore {Heal} (3 + {FocusPercentage.ToPercentage()} FOC) health to all allied units.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.AreaOfEffect,
            AbilityTag.NoTarget
        };

        public float FocusPercentage = 0.5f;
        public float Heal => 3 + FocusPercentage * AbilityUser.focus;
        
        public SalvationAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return TurnManager.Instance.EnqueuedEntities.Any(x =>
                x.GetType() == AbilityUser.GetType() && x.health < x.maxHealth);
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            foreach (var ally in TurnManager.Instance.EnqueuedEntities.Where(x => x.GetType() == AbilityUser.GetType()))
            {
                ally.Heal(Heal);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}