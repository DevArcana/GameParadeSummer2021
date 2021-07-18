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
        public override int Cost => 3;

        public override string Name => "Clone";
        public override string Tooltip => $"Spawn a clone next to you based on your attributes. This clone will have {HealthPercentage.ToPercentage()} ((40 + {HealthFocusPercentage.ToPercentage(false)} Focus)%) of your health and {AttributesPercentage.ToPercentage()} ((40 + {AttributesFocusPercentage.ToPercentage(false)} Focus)%) of your Strength, Focus and Agility. This ability cannot spawn a clone with 4 or less health.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float HealthFocusPercentage = 0.05f;
        public float HealthPercentage => Math.Max(0, 0.4f + HealthFocusPercentage * AbilityUser.focus);
        public float AttributesFocusPercentage = 0.04f;
        public float AttributesPercentage => Math.Max(0, 0.4f + AttributesFocusPercentage * AbilityUser.focus);
        
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
            return targetEntity is null && AbilityUser.maxHealth * HealthPercentage > 4;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            GameArena.Instance.SpawnAlly(position,
                HealthPercentage * AbilityUser.maxHealth,
                AttributesPercentage * AbilityUser.strength,
                AttributesPercentage * AbilityUser.focus,
                AttributesPercentage * AbilityUser.agility);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}