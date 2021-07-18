using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SalvationAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Salvation";
        public override string Tooltip => $"Restore {Heal} (4 + {FocusPercentage.ToPercentage()} Focus) health to all allied units.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.AreaOfEffect,
            AbilityTag.NoTarget
        };

        public float FocusPercentage = 0.5f;
        public float Heal => 4 + FocusPercentage * AbilityUser.focus;
        
        public SalvationAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllAllies(new Vector2Int(-1, -1)).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return TurnManager.Instance.EnqueuedEntities.Any(x =>
                x.GetType() == AbilityUser.GetType() && x.health < x.maxHealth);
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            foreach (var ally in TurnManager.Instance.EnqueuedEntities.Where(x => x.GetType() == AbilityUser.GetType()))
            {
                ally.Heal(Heal);
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}