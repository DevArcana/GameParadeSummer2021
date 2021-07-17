using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class StraightDashAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Straight Dash";
        public override string Tooltip => $"Dash {DashDistance} units in a cardinal direction. Dash through any enemies";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public int DashDistance = 3;
        
        public StraightDashAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetCardinalAtEdge(x, y, DashDistance).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return targetEntity is null;
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
