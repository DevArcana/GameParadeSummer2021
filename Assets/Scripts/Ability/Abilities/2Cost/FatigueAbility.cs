using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class FatigueAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Fatigue";
        public override string Tooltip => $"Pick any enemy unit in a square area around you and decrease their Strength by {StrengthDecreasePercentage.ToPercentage()} ((40 + {FocusPercentage.ToPercentage(false)} Focus)%)";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float FocusPercentage = 0.05f;
        public float StrengthDecreasePercentage => Math.Min(1, 0.4f + FocusPercentage * AbilityUser.focus);
        
        public FatigueAbility(GridEntity user) : base(user)
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
            targetEntity.strength *= 1 - StrengthDecreasePercentage;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}