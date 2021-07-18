using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;
using UnityEngine.Rendering.LWRP;

namespace Ability.Abilities
{
    public class JumpAbility : BaseAbility
    {
        public override int Cost => 2;

        public override string Name => "Jump";
        public override string Tooltip => $"Move to any unoccupied tile in a square area of radius {Radius} (2 + {AgilityPercentage.ToPercentage()} Agility) around you.";
        public override HashSet<AbilityTag> Tags => new HashSet<AbilityTag>
        {
            
        };

        public float AgilityPercentage = 0.25f;
        public int Radius => (int) (2 + AgilityPercentage * AbilityUser.agility);
        
        public JumpAbility(GridEntity user) : base(user)
        {
        }

        public override List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetFilledSquareArea(x, y, Radius).ToList();
        }

        public override bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            var arena = GameArena.Instance;
            arena.Grid.WorldToGrid(position, out var x, out var y);
            return arena.CanMove(AbilityUser, x, y);
        }

        public override IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish)
        {
            var arena = GameArena.Instance;
            var grid = arena.Grid;
            
            grid.WorldToGrid(position, out var x, out var y);

            return arena.Move(AbilityUser, x, y, onFinish);
        }
    }
}