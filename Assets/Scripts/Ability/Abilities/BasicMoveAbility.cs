using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class BasicMoveAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Move";
        public override string Tooltip => $"Move up to {Distance} (1 + {AgilityPercentage.ToPercentage()} Agility) tile(s) in any cardinal direction.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Mobility,
            AbilityTag.AreaTargeted
        };

        public float AgilityPercentage = 0.125f;
        public int Distance => (int) (1 + AgilityPercentage * AbilityUser.agility);

        public BasicMoveAbility(GridEntity user) : base(user)
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

            return arena.Move(AbilityUser, x, y, onFinish);
        }
    }
}