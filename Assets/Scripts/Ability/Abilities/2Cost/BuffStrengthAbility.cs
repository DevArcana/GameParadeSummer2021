using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class BuffStrengthAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Buff Strength";
        public override string Tooltip => $"Permanently increase your Strength by {StrengthIncrease} (0.5 + {FocusPercentage.ToPercentage()} Focus)";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float FocusPercentage = 0.25f;
        public float StrengthIncrease => 0.5f + FocusPercentage * AbilityUser.focus;
        
        public BuffStrengthAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return new List<Vector2Int> {new Vector2Int(x, y)};
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            AbilityUser.strength += StrengthIncrease;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}