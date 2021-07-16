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
            TurnManager.Instance.TurnStarted += OnTurnStarted;
            health = maxHealth = 8;
            damage = 2;
            healthBar.SetHealth(health, maxHealth);
        }

        public void OnTurnStarted(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            if (args.Entity == this)
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
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}