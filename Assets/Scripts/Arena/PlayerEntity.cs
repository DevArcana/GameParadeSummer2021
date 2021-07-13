using UnityEngine;

namespace Arena
{
    public class PlayerEntity : GridEntity
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!TurnManager.Instance.IsPlayerTurn())
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (GameArena.Instance.Move(this, hit.point, out var cellPos))
                    {
                        Move(new Vector3(cellPos.x + 0.5f, transform.position.y, cellPos.z + 0.5f));
                        TurnManager.Instance.NextTurn();
                    }
                }
            }
        }
    }
}