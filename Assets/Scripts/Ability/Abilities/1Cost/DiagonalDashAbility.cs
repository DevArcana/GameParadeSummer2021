using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class DiagonalDashAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Diagonal Dash";
        public override string Tooltip => $"Dash up to {Distance} (2 + {AgilityPercentage.ToPercentage()} AGL) units in any ordinal direction, ignoring any enemies on the way.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float AgilityPercentage = 0.5f;
        public int Distance => (int) (2 + AgilityPercentage * AbilityUser.agility);
        
        public DiagonalDashAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllOrdinal(x, y, Distance).ToList();
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
