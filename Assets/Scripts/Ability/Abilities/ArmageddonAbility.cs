﻿using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ArmageddonAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Armageddon";
        public override string Tooltip => $"Deal {Damage} damage to ALL units - yourself, ally and enemy.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public int Damage = 6;
        
        public ArmageddonAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            foreach (var unit in TurnManager.Instance.EnqueuedEntities)
            {
                unit.TakeDamage(Damage);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}