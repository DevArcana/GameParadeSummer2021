using System;
using System.Collections;
using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Ability.Abilities
{
    public class KamikazeAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Kamikaze";
        public override string Tooltip => $"Execute both yourself and a targeted enemy unit.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            AbilityTag.Damage,
            AbilityTag.Execute,
            AbilityTag.EnemyTargeted,
            AbilityTag.SingleTarget,
            AbilityTag.Sacrifice
        };
        
        public KamikazeAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return !(targetEntity is null) && targetEntity.GetType() != AbilityUser.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            targetEntity.Execute();
            AbilityUser.Execute();
            
            onFinish.Invoke();
            yield return null;
        }
    }
}