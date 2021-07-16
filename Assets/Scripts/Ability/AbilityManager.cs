using System;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability
{
    public class AbilityManager : MonoBehaviour
    {
        #region Singleton

        public static AbilityManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public bool CanUse(BaseAbility ability, Vector3 position, GridEntity target)
        {
            var gameArena = GameArena.Instance;
            var grid = gameArena.Grid;

            grid.WorldToGrid(position, out var x, out var y);
            if (!ability.GetArea().Contains(new Vector2Int(x, y)))
            {
                return false;
            }
            
            if (!ability.CanExecute(position, target))
            {
                return false;
            }

            return true;
        }
        
        public void Use(BaseAbility ability, Vector3 position, GridEntity target, Action onSuccess, Action onFail)
        {
            var turnManager = TurnManager.Instance;

            if (!CanUse(ability, position, target))
            {
                onFail();
                return;
            }

            StartCoroutine(ability.Execute(position, target, () =>
            {
                onSuccess();
                if (turnManager.ActionPoints == 0)
                {
                    turnManager.NextTurn();
                }
            }));
        }
    }
}