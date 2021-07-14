using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        private bool _canMove = false;
        
        protected override void Start()
        {
            base.Start();
            TurnManager.Instance.TurnChanged += OnTurnChanged;
        }

        private void Update()
        {
            if (!_canMove || TurnManager.Instance.CurrentTurn != this)
            {
                return;
            }
            
            var position = transform.position;
                
            GameArena.Instance.Grid.WorldToGrid(position, out var x, out var y);
            
            var moves = GameArena.Instance.Grid.GetAvailableNeighbours(x, y).ToList();
            var move = moves.ElementAt(Random.Range(0, moves.Count));

            if (GameArena.Instance.Move(this, GameArena.Instance.Grid.GridToWorld(move.x, move.y), out var cellPos))
            {
                _canMove = false;
                StartCoroutine(Move(new Vector3(cellPos.x + 0.5f, position.y, cellPos.z + 0.5f), () =>
                {
                    _canMove = true;

                    if (TurnManager.Instance.ActionPoints == 0)
                    {
                        TurnManager.Instance.FinishTurn();
                    }
                }));
            }
        }

        public void OnTurnChanged(object sender, TurnManager.OnTurnChangedEventArgs args)
        {
            _canMove = args.CurrentTurn == this;
        }
    }
}