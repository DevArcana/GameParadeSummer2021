using System;
using System.Collections;
using Arena;
using UnityEngine;

namespace Ability
{
    public class MovementAbility : BaseAbility
    {
        public override int Cost => 1;

        public MovementAbility(GridEntity user) : base(user)
        {
        }

        public override IEnumerator Execute(Vector3 position, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);

            return arena.Move(AbilityUser, x, y, onFinish);
        }
    }
}