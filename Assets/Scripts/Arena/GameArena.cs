using System;
using Grid;
using UnityEngine;

namespace Arena
{
    public class GameArena : MonoBehaviour
    {
        public static GameArena Instance { get; private set; }

        public Grid<GridEntity> Grid { get; private set; }
        
        private Camera _camera;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            Grid = new Grid<GridEntity>(7, 7, 1.0f, transform.position - new Vector3(3.5f, 0.0f, 3.5f));
        }

        public bool CanMove(GridEntity entity, Vector3 position)
        {
            Grid.WorldToGrid(position, out var x, out var y);
            return Grid.IsWithinGrid(x, y);
        }

        public bool Move(GridEntity entity, Vector3 position, out Vector3 targetCellPos)
        {
            targetCellPos = Vector3.zero;
            
            if (!CanMove(entity, position))
            {
                return false;
            }

            Grid.WorldToGrid(position, out var x, out var y);

            if (!(Grid[x, y] is null))
            {
                targetCellPos = Grid.GridToWorld(x, y);
                return true;
            }
            
            Grid[x, y] = entity;
            targetCellPos = Grid.GridToWorld(x, y);

            Grid.WorldToGrid(entity.transform.position, out x, out y);
            Grid[x, y] = null;
            
            return true;
        }

        private void OnDrawGizmos()
        {
            if (Grid is null)
            {
                return;
            }

            var x = -1;
            var y = -1;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                Grid.WorldToGrid(hit.point, out x, out y);
            }

            foreach (var (gx, gy, g) in Grid)
            {
                var pos = Grid.GridToWorld(gx, gy) + new Vector3(0.5f, 0.0f, 0.5f);
                
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
            Grid.WorldToGrid(entity.transform.position, out var x, out var y);

            if (Grid.IsWithinGrid(x, y))
            {
                if (Grid[x, y] is null)
                {
                    Grid[x, y] = entity;
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