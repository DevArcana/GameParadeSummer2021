using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        protected override void Start()
        {
            base.Start();
            TurnManager.Instance.TurnChanged += OnTurnChanged;
            health = 8;
            damage = 2;
        }

        public void OnTurnChanged(object sender, TurnManager.OnTurnChangedEventArgs args)
        {
            var manager = (TurnManager) sender;

            if (args.CurrentTurn == this)
            {
                var position = transform.position;
                
                GameArena.Instance.Grid.WorldToGrid(position, out var x, out var y);
            
                var moves = GameArena.Instance.Grid.GetAvailableNeighbours(x, y).ToList();
                var move = moves.ElementAt(Random.Range(0, moves.Count));
            
                GameArena.Instance.Move(this, GameArena.Instance.Grid.GridToWorld(move.x, move.y), out var cellPos);
                StartCoroutine(Move(new Vector3(cellPos.x + 0.5f, position.y, cellPos.z + 0.5f), () =>
                {
                    manager.FinishTurn();
                }));
            }
        }
    }
}