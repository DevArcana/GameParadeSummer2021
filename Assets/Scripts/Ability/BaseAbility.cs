using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability
{
    public abstract class BaseAbility
    {
        protected GridEntity AbilityUser { get; }
        
        public abstract int Cost { get; }

        public List<Vector2Int> GetArea()
        {
            var grid = GameArena.Instance.Grid;
            grid.WorldToGrid(AbilityUser.transform.position, out var x, out var y);
            return grid.GetAvailableNeighbours(x, y).ToList();
        }

        public abstract IEnumerator Execute(Vector3 position, Action onFinish);
        
        protected BaseAbility(GridEntity user)
        {
            AbilityUser = user;
        }
    }
}