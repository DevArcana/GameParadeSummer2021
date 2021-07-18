using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class StandUnitedAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Stand United";
        public override string Tooltip => $"Gain {PerAllyStrengthIncrease} (0.25 + {FocusPercentage.ToPercentage()} Focus) Strength per each ally on the board, excluding yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float FocusPercentage = 0.125f;
        public float PerAllyStrengthIncrease => 0.25f + FocusPercentage * AbilityUser.focus;
        
        public StandUnitedAbility(GridEntity user) : base(user)
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
            return TurnManager.Instance.EnqueuedEntities.Count(x => x is PlayerEntity) > 1;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            AbilityUser.strength += (TurnManager.Instance.EnqueuedEntities.Count(x => x is PlayerEntity) - 1) * PerAllyStrengthIncrease;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}