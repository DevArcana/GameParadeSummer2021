using System;
using Grid;
using UnityEngine;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }
        
        public class OnTurnChangedEventArgs : EventArgs
        {
            public GridEntity CurrentTurn;
        }
        
        public event EventHandler<OnTurnChangedEventArgs> TurnChanged;

        public void OnTurnChanged(GridEntity currentTurn)
        {
            TurnChanged?.Invoke(this, new OnTurnChangedEventArgs {CurrentTurn = currentTurn});
        }

        private void Awake()
        {
            Instance = this;
        }
        
        public bool IsPlayerTurn()
        {
            return true;
        }

        public void NextTurn()
        {
            
        }
    }
}