using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class QueenStyleAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Queen Style";
        public override string Tooltip => $"Move any number of units in any direction, cardinal or ordinal, to a tile not occupied by an ally. If you move to a tile occupied by an enemy, execute it. After the move, gain {ArmourIncrease} ({FocusPercentage.ToPercentage()} Focus) armour.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float FocusPercentage = 1.0f;
        public float ArmourIncrease => 5 + FocusPercentage * AbilityUser.focus;
        
        public QueenStyleAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            var area = new List<Vector2Int>(grid.GetAllOrdinal(x, y, 6));
            area.AddRange(grid.GetAllCardinal(x, y, 6));
            return area;
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(position, out var x, out var y);
            var entity = grid[x, y];
            if (entity is null)
            {
                return true;
            }
            
            return entity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);
            var entity = grid[x, y];

            yield return arena.Move(AbilityUser, x, y);

            if (!(entity is null) && entity.GetType() != AbilityUser.GetType())
            {
                entity.Execute();
            }

            AbilityUser.armour += ArmourIncrease;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}