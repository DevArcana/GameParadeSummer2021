using System;
using UnityEngine;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }
        
        public int ActionPoints { get; private set; }
        
        public class OnTurnChangedEventArgs : EventArgs
        {
            public GridEntity CurrentTurn;
        }
        
        public event EventHandler<OnTurnChangedEventArgs> TurnChanged;
        
        public void OnTurnChanged(GridEntity currentTurn)
        {
            TurnChanged?.Invoke(this, new OnTurnChangedEventArgs {CurrentTurn = currentTurn});
        }

        public class OnActionPointsChangedEventArgs : EventArgs
        {
            public int ActionPointsSpent;
        }

        public event EventHandler<OnActionPointsChangedEventArgs> ActionPointsChanged;
        
        public void OnActionPointsChanged(int points)
        {
            ActionPointsChanged?.Invoke(this, new OnActionPointsChangedEventArgs {ActionPointsSpent = points});
        }

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
            return true;
        }

        private void NextTurn()
        {
            ActionPoints = 2;
            OnActionPointsChanged(-2);
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
    }
}