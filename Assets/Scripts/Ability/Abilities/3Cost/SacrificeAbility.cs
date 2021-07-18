using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SacrificeAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Sacrifice";
        public override string Tooltip => $"Execute yourself and increase Strength, Focus and Agility of all allied units by {AttributesIncreasePercentage.ToPercentage(false)} ((30 + {FocusPercentage.ToPercentage(false)} Focus)%) of your Strength, Focus and Agility.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Sacrifice,
            AbilityTag.NoTarget,
            AbilityTag.Buff
        };

        public float FocusPercentage = 0.05f;
        public float AttributesIncreasePercentage => 0.3f + FocusPercentage * AbilityUser.focus;
        
        public SacrificeAbility(GridEntity user) : base(user)
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
            return TurnManager.Instance.EnqueuedEntities.Any(x =>
                x != AbilityUser && x.GetType() == AbilityUser.GetType());
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            AbilityUser.Execute();
            
            foreach (var ally in TurnManager.Instance.EnqueuedEntities.Where(x => x != AbilityUser && x.GetType() == AbilityUser.GetType()))
            {
                ally.strength += AbilityUser.strength * AttributesIncreasePercentage;
                ally.focus += AbilityUser.focus * AttributesIncreasePercentage;
                ally.agility += AbilityUser.agility * AttributesIncreasePercentage;
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}