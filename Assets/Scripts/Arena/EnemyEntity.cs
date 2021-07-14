using UnityEngine;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        private void Start()
        {
            TurnManager.Instance.TurnChanged += OnTurnChanged;
        }

        public void OnTurnChanged(object sender, TurnManager.OnTurnChangedEventArgs args)
        {
            var manager = (TurnManager) sender;

            if (args.CurrentTurn == this)
            {
                GameArena.Instance.Grid.WorldToGrid(transform.position, out var x, out var y);
                
                // StartCoroutine(Move(new Vector3(cellPos.x + 0.5f, transform.position.y, cellPos.z + 0.5f), () =>
                // {
                //     TurnManager.Instance.FinishTurn();
                // }));
            }
        }
    }
}