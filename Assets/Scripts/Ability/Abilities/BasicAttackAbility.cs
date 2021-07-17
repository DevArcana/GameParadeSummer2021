using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class MeleeAbility : BaseAbility
    {
        public override int Cost => 1;

        public override string Name => "Attack";
        public override string Tooltip => $"Deal {AbilityUser.damage} damage to an enemy next to the player in cardinal direction.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Melee,
            AbilityTag.EnemyTargeted
        };

        public MeleeAbility(GridEntity user) : base(user)
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
            targetEntity.TakeDamage(AbilityUser.damage);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}