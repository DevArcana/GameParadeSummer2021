using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SlamDashAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Slam Dash";
        public override string Tooltip => $"Dash up to {Distance} (3 + {AgilityPercentage.ToPercentage()} Agility) units in any cardinal direction, ignoring any enemies on the way. After landing, deal {Damage} (3 + {StrengthPercentage.ToPercentage()} Strength) to all enemies in a square area around you.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float AgilityPercentage = 0.5f;
        public int Distance => (int) (3 + AgilityPercentage * AbilityUser.agility);

        public float StrengthPercentage = 0.75f;
        public float Damage => 3 + StrengthPercentage * AbilityUser.strength;
        
        public SlamDashAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllCardinal(x, y, Distance).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            var arena = GameArena.Instance;
            arena.Grid.WorldToGrid(position, out var x, out var y);
            return arena.CanMove(AbilityUser, x, y);
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