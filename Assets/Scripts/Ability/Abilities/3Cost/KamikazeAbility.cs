using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class KamikazeAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Kamikaze";
        public override string Tooltip => $"Dash to a targeted enemy unit. Then, explode, executing yourself and dealing {Damage} ({StrengthPercentage.ToPercentage()} Strength + {FocusPercentage.ToPercentage()} Focus) damage to all enemies in a square area around you.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Execute,
            AbilityTag.EnemyTargeted,
            AbilityTag.SingleTarget,
            AbilityTag.Sacrifice
        };

        public float StrengthPercentage = 1.5f;
        public float FocusPercentage = 1.0f;
        public float Damage => StrengthPercentage * AbilityUser.strength + FocusPercentage * AbilityUser.focus;
        
        public KamikazeAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            return GameArena.Instance.Grid.GetAllEnemies().ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);

            yield return arena.Move(AbilityUser, x, y);

            foreach (var enemy in grid.GetEnemiesInArea(grid.GetFilledSquareArea(x, y, 1)))
            {
                enemy.TakeDamage(Damage);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}