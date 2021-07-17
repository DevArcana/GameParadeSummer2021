// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Arena;
// using UnityEngine;
//
// namespace Ability.Abilities
// {
//     public class SwitchAbility : BaseAbility
//     {
//         public override int Cost => 2;
//
//         public override string Name => "Switch";
//         public override string Tooltip => $"Swap places with any unit on the board.";
//         public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
//         {
//             
//         };
//         
//         public SwitchAbility(GridEntity user) : base(user)
//         {
//         }
//
//         public override bool CanExecute(Vector3 position, GridEntity targetEntity)
//         {
//             return !(targetEntity is null) && targetEntity != AbilityUser;
//         }
//
//         public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
//         {
//             yield return GameArena.Instance.Swap(AbilityUser, targetEntity);
//         }
//     }
// }