using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class HealAllyAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Heal Ally";
        public override string Tooltip => $"Restore {Heal} (2 + {FocusPercentage.ToPercentage()} FOC) health to an allied unit other than yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.SelfTargeted
        };

        public float FocusPercentage = 0.5f;
        public float Heal => 2 + FocusPercentage * AbilityUser.focus;
        
        public HealAllyAbility(GridEntity user) : base(user)
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
            return !(targetEntity is null) && targetEntity != AbilityUser && targetEntity.GetType() == AbilityUser.GetType() && targetEntity.health < targetEntity.maxHealth;
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.Heal(Heal);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}