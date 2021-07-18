using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;
using UnityEngine.Rendering.LWRP;

namespace Ability.Abilities
{
    public class TeleportAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Teleport";
        public override string Tooltip => $"Move to any unoccupied tile on the board.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public TeleportAbility(GridEntity user) : base(user)
        {
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