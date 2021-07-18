using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SwitchAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Switch";
        public override string Tooltip => $"Swap places with any unit on the board.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };
        
        public SwitchAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            var area = new List<Vector2Int>(grid.GetAllAllies(new Vector2Int(x, y)));
            area.AddRange(grid.GetAllEnemies());
            return area;
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null);
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(AbilityUser.transform.position, out var userX, out var userY);
            grid.WorldToGrid(position, out var targetX, out var targetY);

            yield return arena.Move(AbilityUser, targetX, targetY);
            yield return arena.Move(targetEntity, userX, userY);
            grid[targetX, targetY] = AbilityUser;

            onFinish.Invoke();
            yield return null;
        }
    }
}