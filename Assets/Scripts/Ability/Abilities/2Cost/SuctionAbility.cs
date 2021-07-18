using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class SuctionAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Suction";
        public override string Tooltip => $"Deal {Damage} (2 + {StrengthPercentage.ToPercentage()} Strength) damage to a targeted enemy unit in a diamond-shaped area with radius 2 around you and increase your strength by {StrengthIncrease} (0.5 + {FocusPercentage.ToPercentage()} Focus).";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Buff,
            AbilityTag.EnemyTargeted
        };

        public float StrengthPercentage = 0.25f;
        public float Damage => 2 + StrengthPercentage * AbilityUser.strength;

        public float FocusPercentage = 0.25f;
        public float StrengthIncrease => 0.5f + FocusPercentage * AbilityUser.focus;
        
        public SuctionAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetFilledDiamondArea(x, y, 2).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            AbilityUser.strength += StrengthIncrease;
            
            onFinish.Invoke();
            yield return null;
        }
    }
}