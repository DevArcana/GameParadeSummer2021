using UnityEngine;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }

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