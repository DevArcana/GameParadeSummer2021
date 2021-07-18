using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using JetBrains.Annotations;
using UnityEngine;

namespace Grid
{
    public class Grid<T> : IEnumerable<(int, int, T)>, IEnumerable
    {
        public readonly int width, height;
        private readonly float _cellSize;
        private readonly T[,] _data;
        private readonly Vector3 _origin;

        public class OnGridChangedEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        public event EventHandler<OnGridChangedEventArgs> OnGridChanged;
    
        public void NotifyGridChanged(int x, int y)
        {
            OnGridChanged?.Invoke(this, new OnGridChangedEventArgs {X = x, Y = y});
        }

        public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<T>, int, int, T> defaultValueFunc = null)
        {
            this.width = width;
            this.height = height;
            _cellSize = cellSize;
            _origin = origin;
            _data = new T[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _data[x, y] = defaultValueFunc is null ? default : defaultValueFunc(this, x, y);
                }
            }
        }

        public bool IsWithinGrid(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

        public bool IsWithinGrid(Vector3 pos)
        {
            WorldToGrid(pos, out var x, out var y);
            return IsWithinGrid(x, y);
        }

        public IEnumerable<T> GetEnemiesInArea(IEnumerable<Vector2Int> area)
        {
            return area.Select(x => _data[x.x, x.y]).Where(x => !(x is null) && x is EnemyEntity);
        }

        public IEnumerable<Vector2Int> GetCardinalAtEdge(int x, int y, int axisDistance)
        {
            var tiles = new List<Vector2Int>();
            
            if (IsWithinGrid(x - axisDistance, y))
                tiles.Add(new Vector2Int(x - axisDistance, y));
            if (IsWithinGrid(x + axisDistance, y))
                tiles.Add(new Vector2Int(x + axisDistance, y));
            if (IsWithinGrid(x, y - axisDistance))
                tiles.Add(new Vector2Int(x, y - axisDistance));
            if (IsWithinGrid(x, y + axisDistance))
                tiles.Add(new Vector2Int(x, y + axisDistance));

            return tiles;
        }

        public IEnumerable<Vector2Int> GetAllCardinal(int x, int y, int axisDistance)
        {
            var tiles = new List<Vector2Int>();

            for (var i = 1; i <= axisDistance; i++)
            {
                tiles.AddRange(GetCardinalAtEdge(x, y, i));
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetOrdinalAtEdge(int x, int y, int diagonalDistance)
        {
            var tiles = new List<Vector2Int>();
            
            if (IsWithinGrid(x - diagonalDistance, y - diagonalDistance))
                tiles.Add(new Vector2Int(x - diagonalDistance, y - diagonalDistance));
            if (IsWithinGrid(x + diagonalDistance, y - diagonalDistance))
                tiles.Add(new Vector2Int(x + diagonalDistance, y - diagonalDistance));
            if (IsWithinGrid(x - diagonalDistance, y + diagonalDistance))
                tiles.Add(new Vector2Int(x - diagonalDistance, y + diagonalDistance));
            if (IsWithinGrid(x + diagonalDistance, y + diagonalDistance))
                tiles.Add(new Vector2Int(x + diagonalDistance, y + diagonalDistance));

            return tiles;
        }

        public IEnumerable<Vector2Int> GetAllOrdinal(int x, int y, int diagonalDistance)
        {
            var tiles = new List<Vector2Int>();

            for (var i = 1; i <= diagonalDistance; i++)
            {
                tiles.AddRange(GetOrdinalAtEdge(x, y, i));
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetFilledSquareArea(int x, int y, int radius)
        {
            var tiles = new List<Vector2Int>();

            for (var tx = x - radius; tx <= x + radius; tx++)
            {
                for (var ty = y - radius; ty <= y + radius; ty++)
                {
                    if (IsWithinGrid(tx, ty))
                        tiles.Add(new Vector2Int(tx, ty));
                }
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetFilledDiamondArea(int x, int y, int radius)
        {
            var tiles = new List<Vector2Int>();

            for (var tx = x - radius; tx <= x + radius; tx++)
            {
                for (var ty = y - radius; ty <= y + radius; ty++)
                {
                    if (IsWithinGrid(tx, ty) && Math.Abs(x - tx) + Math.Abs(y - ty) <= radius)
                        tiles.Add(new Vector2Int(tx, ty));
                }
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetWholeGrid()
        {
            var tiles = new List<Vector2Int>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    tiles.Add(new Vector2Int(x, y));
                }
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetAllEnemies()
        {
            var tiles = new List<Vector2Int>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var entity = _data[x, y];
                    if (!(entity is null) && entity is EnemyEntity)
                    {
                        tiles.Add(new Vector2Int(x, y));
                    }
                }
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetAllAllies(Vector2Int positionToExclude = default)
        {
            var tiles = new List<Vector2Int>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var entity = _data[x, y];
                    if (!(x == positionToExclude.x && y == positionToExclude.y) && !(entity is null) && entity is PlayerEntity)
                    {
                        tiles.Add(new Vector2Int(x, y));
                    }
                }
            }

            return tiles;
        }

        public IEnumerable<Vector2Int> GetAllEntities()
        {
            var tiles = new List<Vector2Int>(GetAllEnemies());
            tiles.AddRange(GetAllAllies(new Vector2Int(-1, -1)));
            return tiles;
        }

        public void WorldToGrid(Vector3 worldPos, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPos - _origin).x / _cellSize);
            y = Mathf.FloorToInt((worldPos - _origin).z / _cellSize);
        }

        public Vector3 GridToWorld(int x, int y) => new Vector3(x, 0, y) * _cellSize + _origin;

        public T GetValue(int x, int y)
        {
            if (!IsWithinGrid(x, y))
            {
                return default;
            }
        
            return _data[x, y];
        }
    
        public bool SetValue(int x, int y, T value)
        {
            if (!IsWithinGrid(x, y))
            {
                return false;
            }

            _data[x, y] = value;
            NotifyGridChanged(x, y);
        
            return true;
        }
    
        public bool SetValue(Vector3 pos, T value)
        {
            WorldToGrid(pos, out var x, out var y);
            return SetValue(x, y, value);
        }

        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }

        public IEnumerator<(int, int, T)> GetEnumerator()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    yield return (x, y, _data[x, y]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public (int, int) GetPosition(T entity)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (entity.Equals(this[i, j]))
                    {
                        return (i, j);
                    }
                }
            }

            return (0, 0);
        }
    }
}
