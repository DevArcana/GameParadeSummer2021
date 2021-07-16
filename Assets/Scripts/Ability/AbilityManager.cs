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

        public void Use(BaseAbility ability, Vector3 position, GridEntity target, Action onSuccess, Action onFail)
        {
            var turnManager = TurnManager.Instance;
            var gameArena = GameArena.Instance;
            var grid = gameArena.Grid;

            grid.WorldToGrid(position, out var x, out var y);
            if (!ability.GetArea().Contains(new Vector2Int(x, y)))
            {
                onFail();
                return;
            }
            
            if (!ability.CanExecute(position, target))
            {
                onFail();
                return;
            }

            if (!turnManager.TrySpendActionPoints(ability.Cost))
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