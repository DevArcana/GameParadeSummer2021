using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ExposeAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Expose";
        public override string Tooltip => $"Remove all armour of a targeted enemy unit in a square area around you.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public ExposeAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetFilledSquareArea(x, y, 1).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.armour = 0;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}