using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class StrengthenAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Strengthen";
        public override string Tooltip => $"Increase armour of a targeted allied unit, other than yourself, by {ArmourIncrease} (4 + {FocusPercentage.ToPercentage()} Focus)";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public float FocusPercentage = 0.5f;
        public float ArmourIncrease => 4 + FocusPercentage * AbilityUser.focus;
        
        public StrengthenAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllAllies(new Vector2Int(x, y)).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            foreach (var unit in TurnManager.Instance.EnqueuedEntities.Where(x => x is PlayerEntity))
            {
                unit.armour += ArmourIncrease;
            }
            
            onFinish.Invoke();
            yield return null;
        }
    }
}