using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class CloneAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Clone";
        public override string Tooltip => $"Spawn a clone next to you based on your attributes. This clone will have {HealthDecreasePercentage} ((60 - {HealthFocusPercentage.ToPercentage()})%) less health and {AttributesDecreasePercentage} ((60 - {AttributesFocusPercentage.ToPercentage()})%) worse attributes. This ability cannot spawn a clone with 4 or less health.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float HealthFocusPercentage = 0.05f;
        public float HealthDecreasePercentage => Math.Max(0, 0.6f - HealthFocusPercentage * AbilityUser.focus);
        public float AttributesFocusPercentage = 0.1f;
        public float AttributesDecreasePercentage => Math.Max(0, 0.6f - AttributesFocusPercentage * AbilityUser.focus);
        
        public CloneAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetCardinalAtEdge(x, y, 1).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return targetEntity is null;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            GameArena.Instance.SpawnAlly(position,
                HealthDecreasePercentage * AbilityUser.maxHealth,
                AttributesDecreasePercentage * AbilityUser.strength,
                AttributesDecreasePercentage * AbilityUser.focus,
                AttributesDecreasePercentage * AbilityUser.agility);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}