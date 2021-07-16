using System;
using System.Collections;
using Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Arena
{
    public class GameArena : MonoBehaviour
    {
        #region Singleton

        public static GameArena Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Grid = new Grid<GridEntity>(7, 7, 1.0f, transform.position - new Vector3(3.5f, 0.0f, 3.5f));
        }

        #endregion

        public Grid<GridEntity> Grid { get; private set; }

        public bool CanMove(GridEntity entity, int destX, int destY)
        {
            Grid.WorldToGrid(entity.transform.position, out var entityX, out var entityY);
            return Grid.IsWithinGrid(destX, destY) && Grid[destX, destY] is null && (destX != entityX || destY != entityY);
        }

        public IEnumerator Move(GridEntity entity, int x, int y, [CanBeNull] Action onFinish = null)
        {
            if (!CanMove(entity, x, y))
            {
                yield return new WaitForEndOfFrame();
            }

            var pos = Grid.GridToWorld(x, y);
            
            Grid[x, y] = entity;
            Grid.WorldToGrid(entity.transform.position, out x, out y);
            Grid[x, y] = null;

            yield return entity.Move(new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f), onFinish);
        }

        public void Register(GridEntity entity)
        {
            Grid.WorldToGrid(entity.transform.position, out var x, out var y);

            if (Grid.IsWithinGrid(x, y))
            {
                if (Grid[x, y] is null)
                {
                    Grid[x, y] = entity;
                    TurnManager.Instance.Enqueue(entity);
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

        public void Kill(GridEntity entity)
        {
            var position = entity.transform.position;
            Grid.WorldToGrid(position, out var x, out var y);
            Grid[x, y] = null;
            TurnManager.Instance.Dequeue(entity);
        }
    }
}