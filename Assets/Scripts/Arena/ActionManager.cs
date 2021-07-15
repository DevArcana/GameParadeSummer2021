using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public enum EntityAction
    {
        Move,
        Attack
    }

    public class ActionManager : MonoBehaviour
    {
        public static ActionManager Instance { get; private set; }

        public static Dictionary<EntityAction, int> PointsPerAction { get; private set; }

        #region OnActionProcessed

        public class OnActionProcessedEventArgs : EventArgs
        {
            public GridEntity Entity;
            public bool IsSuccess;
        }

        public event EventHandler<OnActionProcessedEventArgs> ActionProcessed;

        public void OnActionProcessed(GridEntity entity, bool isSuccess)
        {
            ActionProcessed?.Invoke(this, new OnActionProcessedEventArgs {Entity = entity, IsSuccess = isSuccess});
        }

        #endregion
        
        private void Awake()
        {
            Instance = this;
            PointsPerAction = new Dictionary<EntityAction, int>
            {
                {EntityAction.Move, 1},
                {EntityAction.Attack, 1}
            };
        }

        public void TryMove(GridEntity entity, Vector3 position)
        {
            var turnManager = TurnManager.Instance;
            var gameArena = GameArena.Instance;
            
            if (!turnManager.CanSpendActionPoints(PointsPerAction[EntityAction.Move]))
            {
                OnActionProcessed(entity, false);
            }
            
            if (!gameArena.CanMove(entity, position))
            {
                OnActionProcessed(entity, false);
            }

            gameArena.Move(entity, position, out var cellPosition);
            turnManager.SpendActionPoints(PointsPerAction[EntityAction.Move]);
            
            StartCoroutine(entity.Move(new Vector3(cellPosition.x + 0.5f, transform.position.y, cellPosition.z + 0.5f), () =>
            {
                OnActionProcessed(entity, true);
                if (turnManager.ActionPoints == 0)
                {
                    turnManager.FinishTurn();
                }
            }));
        }
    }
}
