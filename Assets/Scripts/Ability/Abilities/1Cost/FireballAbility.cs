using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class FireballAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Fireball";
        public override string Tooltip => $"Deal {Damage} (1 + {StrengthPercentage.ToPercentage()} STR) damage to a targeted enemy unit.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.EnemyTargeted,
            AbilityTag.Ranged
        };

        public float StrengthPercentage = 0.5f;
        public float Damage => 1 + StrengthPercentage * AbilityUser.strength;
        
        public FireballAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAllCardinal(x, y, 3).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}