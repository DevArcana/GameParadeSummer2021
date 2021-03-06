using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class LeechAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Leech";
        public override string Tooltip => $"Deal {Damage} (3 - {DamageFocusPercentage.ToPercentage()} Focus) damage to a targeted allied unit (ignores armour) and restore {Heal} (4 + {HealFocusPercentage.ToPercentage()} Focus) health to yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Healing,
            AbilityTag.AllyTargeted
        };

        public float DamageFocusPercentage = 0.5f;
        public float Damage => Math.Max(0, 3 - DamageFocusPercentage * AbilityUser.focus);

        public float HealFocusPercentage = 0.5f;
        public float Heal => 4 + HealFocusPercentage * AbilityUser.focus;
        
        public LeechAbility(GridEntity user) : base(user)
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
            return !(targetEntity is null) && targetEntity != AbilityUser && targetEntity.GetType() == AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage, true);
            AbilityUser.Heal(Heal);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}