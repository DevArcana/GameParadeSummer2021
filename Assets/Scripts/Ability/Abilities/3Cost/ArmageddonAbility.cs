using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ArmageddonAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Armageddon";
        public override string Tooltip => $"Deal {Damage} (4 + {StrengthPercentage.ToPercentage()} STR) damage to ALL units - yourself, ally and enemy.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public float StrengthPercentage = 0.5f;
        public float Damage => 4 + StrengthPercentage * AbilityUser.strength;
        
        public ArmageddonAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllEntities().ToList();
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