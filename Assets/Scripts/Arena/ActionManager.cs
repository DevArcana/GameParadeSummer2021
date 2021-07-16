using System;
using System.Linq;
using UnityEngine;

namespace Arena
{
    public class ActionManager : MonoBehaviour
    {
        #region Singleton

        public static ActionManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        #endregion

        #region OnActionProcessed

        public class OnActionProcessedEventArgs : EventArgs
        {
            public GridEntity Entity { get; set; }
            public bool IsSuccess { get; set; }
        }

        public event EventHandler<OnActionProcessedEventArgs> ActionProcessed;

        private void OnActionProcessed(GridEntity entity, bool isSuccess)
        {
            ActionProcessed?.Invoke(this, new OnActionProcessedEventArgs {Entity = entity, IsSuccess = isSuccess});
        }

        #endregion

        public void TryMove(GridEntity entity, Vector3 position)
        {
            var turnManager = TurnManager.Instance;
            var gameArena = GameArena.Instance;

            var grid = gameArena.Grid;
            grid.WorldToGrid(position, out var x, out var y);

            if (!gameArena.CanMove(entity, x, y))
            {
                OnActionProcessed(entity, false);
                return;
            }

            if (!turnManager.TrySpendActionPoint())
            {
                OnActionProcessed(entity, false);
                return;
            }
            
            grid.WorldToGrid(entity.transform.position, out var entX, out var entY);
            var moves = GameArena.Instance.Grid.GetAvailableNeighbours(entX, entY).ToList();
            if (!moves.Contains(new Vector2Int(x, y)))
            {
                OnActionProcessed(entity, false);
                return;
            }

            StartCoroutine(gameArena.Move(entity, x, y, () =>
            {
                OnActionProcessed(entity, true);
                if (turnManager.ActionPoints == 0)
                {
                    turnManager.NextTurn();
                }
            }));
        }
    }
}
