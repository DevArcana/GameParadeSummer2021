using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class AbsorbHealthAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Absorb Health";
        public override string Tooltip => $"Deal {Damage} (3 + {StrengthPercentage.ToPercentage()} Strength) damage to a targeted enemy unit and restore {Heal} (4 + {FocusPercentage.ToPercentage()} Focus) health to yourself.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Healing,
            AbilityTag.EnemyTargeted,
            AbilityTag.SingleTarget,
            AbilityTag.Ranged
        };

        public float StrengthPercentage = 0.75f;
        public float Damage => 3 + StrengthPercentage * AbilityUser.strength;

        public float FocusPercentage = 0.5f;
        public float Heal => 4 + FocusPercentage * AbilityUser.focus;
        
        public AbsorbHealthAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            return GameArena.Instance.Grid.GetWholeGrid().ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.TakeDamage(Damage);
            AbilityUser.Heal(Heal);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}