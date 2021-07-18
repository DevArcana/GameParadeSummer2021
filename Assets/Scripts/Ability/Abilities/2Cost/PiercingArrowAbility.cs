using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class PiercingArrowAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Piercing Arrow";
        public override string Tooltip => $"Fire an arrow in any cardinal direction that deals {Damage} (2 + {StrengthPercentage.ToPercentage()} Strength) damage to a targeted enemy unit. This damage ignores armour.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.EnemyTargeted,
            AbilityTag.Ranged
        };

        public float StrengthPercentage = 1.25f;
        public float Damage => 2 + StrengthPercentage * AbilityUser.strength;
        
        public PiercingArrowAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllCardinal(x, y, 4).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage, true);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}