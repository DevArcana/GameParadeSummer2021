using System;
using System.Collections;
using Arena;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ability.Abilities
{
    public class MeleeAbility : BaseAbility
    {
        public override int Cost => 1;

        public MeleeAbility(GridEntity user) : base(user)
        {
        }

        public override bool CanExecute(Vector3 position, GridEntity target)
        {
            return !(target is null) && AbilityUser.GetType() != target.GetType();
        }

        public override IEnumerator Execute(Vector3 position, GridEntity target, Action onFinish)
        {
            target.TakeDamage(AbilityUser.damage);
            
            onFinish.Invoke();
            yield return null;
        }
    }
}