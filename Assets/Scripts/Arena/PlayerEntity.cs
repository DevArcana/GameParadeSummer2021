using UnityEngine;

namespace Arena
{
    public class PlayerEntity : GridEntity
    {
        private Camera _camera;

        private bool _canMove = false;
        
        private void Start()
        {
            _camera = Camera.main;
            _canMove = true;
        }

        private void Update()
        {
            if (!_canMove || !TurnManager.Instance.IsPlayerTurn())
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (GameArena.Instance.Move(this, hit.point, out var cellPos))
                    {
                        _canMove = false;
                        StartCoroutine(Move(new Vector3(cellPos.x + 0.5f, transform.position.y, cellPos.z + 0.5f), () =>
                        {
                            _canMove = true;
                            TurnManager.Instance.NextTurn();
                        }));
                    }
                }
            }
        }
    }
}