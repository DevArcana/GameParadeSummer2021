using System;
using System.Collections;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class MovementAbility : BaseAbility
    {
        public override int Cost => 1;

        public MovementAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity target)
        {
            var arena = GameArena.Instance;
            arena.Grid.WorldToGrid(position, out var x, out var y);
            return arena.CanMove(AbilityUser, x, y);
        }

        public override IEnumerator Execute(Vector3 position, GridEntity target, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);

            return arena.Move(AbilityUser, x, y, onFinish);
        }
    }
}