using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class MassacreAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Massacre";
        public override string Tooltip => $"Deal {Damage} (2 + {StrengthPercentage.ToPercentage()} Strength) damage to all enemy units.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public float StrengthPercentage = 0.5f;
        public float Damage => 2 + StrengthPercentage * AbilityUser.strength;
        
        public MassacreAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllEnemies().ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            foreach (var enemy in TurnManager.Instance.EnqueuedEntities.Where(x => x.GetType() != AbilityUser.GetType()))
            {
                enemy.TakeDamage(Damage);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}