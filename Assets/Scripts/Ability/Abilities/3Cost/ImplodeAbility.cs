using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ImplodeAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Implode";
        public override string Tooltip => $"Deal 4 damage to all enemies in a diamond-shaped area around you. Damage is increased by {PerEnemyDamage} (1 + {StrengthPercentage.ToPercentage()} Strength) for each enemy in the area.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public float StrengthPercentage = 0.5f;
        public float PerEnemyDamage => 1 + StrengthPercentage * AbilityUser.strength;
        
        public ImplodeAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetFilledDiamondArea(x, y, 2).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);

            var enemies = grid.GetEnemiesInArea(grid.GetFilledDiamondArea(x, y, 2)).ToList();
            var count = enemies.Count();
            
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(4 + count * PerEnemyDamage);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}