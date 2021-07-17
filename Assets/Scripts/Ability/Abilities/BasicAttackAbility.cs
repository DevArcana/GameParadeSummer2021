using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class BasicAttackAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Attack";
        public override string Tooltip => $"Deal {Damage} ({StrengthPercentage.ToPercentage()} STR) damage to an enemy next to you in any cardinal direction.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Melee,
            AbilityTag.EnemyTargeted
        };

        public float StrengthPercentage = 1.0f;
        private static readonly int Attack = Animator.StringToHash("Attack");
        public float Damage => StrengthPercentage * AbilityUser.strength;

        public BasicAttackAbility(GridEntity user) : base(user)
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
            return !(targetEntity is null) && AbilityUser.GetType() != targetEntity.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            AbilityUser.animator.SetTrigger(Attack);
            targetEntity.TakeDamage(Damage);
            onFinish.Invoke();
            yield return null;
        }
    }
}