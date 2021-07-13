using System;
using Grid;
using UnityEngine;

namespace Arena
{
    public class GameArena : MonoBehaviour
    {
        public static GameArena Instance { get; private set; }

        private Grid<GridEntity> _grid;
        private Camera _camera;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            _grid = new Grid<GridEntity>(7, 7, 1.0f, transform.position - new Vector3(3.5f, 0.0f, 3.5f));
        }

        public bool Move(GridEntity entity, Vector3 pos, out Vector3 targetCellPos)
        {
            targetCellPos = Vector3.zero;
            
            _grid.WorldToGrid(pos, out var x, out var y);

            if (!_grid.IsWithinGrid(x, y) || !(_grid[x, y] is null))
            {
                return false;
            }

            if (!TurnManager.Instance.SpendPoint())
            {
                return false;
            }
            
            _grid[x, y] = entity;
            targetCellPos = _grid.GridToWorld(x, y);

            _grid.WorldToGrid(entity.transform.position, out x, out y);
            _grid[x, y] = null;
            
            return true;
        }

        private void OnDrawGizmos()
        {
            if (_grid is null)
            {
                return;
            }

            var x = -1;
            var y = -1;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                _grid.WorldToGrid(hit.point, out x, out y);
            }

            foreach (var (gx, gy, g) in _grid)
            {
                var pos = _grid.GridToWorld(gx, gy) + new Vector3(0.5f, 0.0f, 0.5f);
                
                if (gx == x && gy == y)
                {
                    Gizmos.DrawCube(pos, Vector3.one);
                }
                else
                {
                    Gizmos.DrawWireCube(pos, Vector3.one);
                }
            }
        }

        public void Register(GridEntity entity)
        {
            _grid.WorldToGrid(entity.transform.position, out var x, out var y);

            if (_grid.IsWithinGrid(x, y))
            {
                if (_grid[x, y] is null)
                {
                    _grid[x, y] = entity;
                }
                else
                {
                    throw new Exception("multiple grid entities on the same tile");
                }
            }
            else
            {
                throw new Exception("grid entity outside grid");
            }
        }
    }
}