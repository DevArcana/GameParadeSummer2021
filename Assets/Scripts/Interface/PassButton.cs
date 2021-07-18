using Arena;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class PassButton : MonoBehaviour
    {
        public Button passButton;
        private void Start()
        {
            var turnManager = TurnManager.Instance;
            passButton.onClick.AddListener(() => { turnManager.NextTurn(); });
            turnManager.TurnStarted += OnTurnChange;
        }
        
        private void OnTurnChange(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            passButton.interactable = args.Entity is PlayerEntity;
        }
    }
}