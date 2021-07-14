using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }
        
        public int ActionPoints { get; private set; }

        public Queue<GridEntity> Entities { get; set; } = new Queue<GridEntity>();

        public GridEntity CurrentTurn => Entities.Any() ? Entities.Peek() : null;

        #region OnTurnChanged

        public class OnTurnChangedEventArgs : EventArgs
        {
            public GridEntity LastTurn;
            public GridEntity CurrentTurn;
        }
        
        public event EventHandler<OnTurnChangedEventArgs> TurnChanged;
        
        public void OnTurnChanged(GridEntity lastTurn, GridEntity currentTurn)
        {
            TurnChanged?.Invoke(this, new OnTurnChangedEventArgs {LastTurn = lastTurn, CurrentTurn = currentTurn});
        }

        #endregion

        #region OnActionPointsChanged

        public class OnActionPointsChangedEventArgs : EventArgs
        {
            public int ActionPointsSpent;
        }

        public event EventHandler<OnActionPointsChangedEventArgs> ActionPointsChanged;
        
        public void OnActionPointsChanged(int points)
        {
            ActionPointsChanged?.Invoke(this, new OnActionPointsChangedEventArgs {ActionPointsSpent = points});
        }

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ActionPoints = 2;
        }

        public bool IsPlayerTurn()
        {
            return Entities.Peek() is PlayerEntity;
        }

        private void NextTurn()
        {
            ActionPoints = 2;
            OnActionPointsChanged(-2);
            var last = Entities.Dequeue();
            Entities.Enqueue(last);
            OnTurnChanged(last, Entities.Peek());
        }
        
        public bool SpendPoint(int points = 1)
        {
            if (ActionPoints - points < 0) return false;
            
            ActionPoints -= points;
            OnActionPointsChanged(points);

            return true;
        }

        public void FinishTurn()
        {
            NextTurn();
        }

        public void Enqueue(GridEntity entity)
        {
            Entities.Enqueue(entity);

            if (Entities.Count == 1)
            {
                OnTurnChanged(null, entity);
            }
        }
    }
}