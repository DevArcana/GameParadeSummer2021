using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        protected override void Start()
        {
            base.Start();
            ActionManager.Instance.ActionProcessed += OnActionProcessed;
            TurnManager.Instance.TurnChanged += OnTurnChanged;
            health = maxHealth = 8;
            damage = 2;
            healthBar.SetHealth(health, maxHealth);
        }

        public void OnTurnChanged(object sender, TurnManager.OnTurnChangedEventArgs args)
        {
            if (args.CurrentTurn == this)
            {
                MakeAction();
            }
        }

        private void MakeAction()
        {
            var gameArena = GameArena.Instance;
            
            var position = transform.position;
                
            gameArena.Grid.WorldToGrid(position, out var x, out var y);
            
            var moves = gameArena.Grid.GetAvailableNeighbours(x, y).ToList();
            var move = moves.ElementAt(Random.Range(0, moves.Count));

            ActionManager.Instance.TryMove(this, gameArena.Grid.GridToWorld(move.x, move.y));
            
            // GameArena.Instance.Move(this, GameArena.Instance.Grid.GridToWorld(move.x, move.y), out var cellPos);
            // StartCoroutine(Move(new Vector3(cellPos.x + 0.5f, position.y, cellPos.z + 0.5f), () =>
            // {
            //     if (TurnManager.Instance.ActionPoints == 0)
            //     {
            //         TurnManager.Instance.FinishTurn();
            //     }
            //     else
            //     {
            //         MakeAction();
            //     }
            // }));
        }

        private void OnActionProcessed(object sender, ActionManager.OnActionProcessedEventArgs args)
        {
            if (args.Entity != this)
            {
                return;
            }
            
            if (!args.IsSuccess || args.IsSuccess && TurnManager.Instance.CurrentTurn == this)
            {
                MakeAction();
            }
        }
    }
}