using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class ReinforceAbility : BaseAbility
    {
        public override int Cost => 3;

        public override string Name => "Reinforce";
        public override string Tooltip => $"Increase armour of all allies, including yourself, by {ArmourIncrease} (3 + {FocusPercentage.ToPercentage()} Focus)";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.NoTarget,
            AbilityTag.AreaOfEffect
        };

        public float FocusPercentage = 0.5f;
        public float ArmourIncrease => 3 + FocusPercentage * AbilityUser.focus;
        
        public ReinforceAbility(GridEntity user) : base(user)
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