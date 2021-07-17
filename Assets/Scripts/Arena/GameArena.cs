using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    public class GameArena : MonoBehaviour
    {
        #region Singleton

        private bool _destroyed = false;
        
        public static GameArena Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Grid = new Grid<GridEntity>(7, 7, 1.0f, transform.position - new Vector3(3.5f, 0.0f, 3.5f));
        }

        #endregion

        public Transform enemyPrefab;
        public Transform allyPrefab;

        private void Start()
        {
            WaveManager.Instance.WaveChanged += OnWaveChange;
            SpawnEnemies(1);
        }

        public Grid<GridEntity> Grid { get; private set; }

        public bool CanMove(GridEntity entity, int destX, int destY)
        {
            Grid.WorldToGrid(entity.transform.position, out var entityX, out var entityY);
            return Grid.IsWithinGrid(destX, destY) && Grid[destX, destY] is null && (destX != entityX || destY != entityY);
        }

        private void OnWaveChange(object sender, EventArgs args)
        {
            if (!_destroyed)
            {
                var waveManager = (WaveManager) sender;
                var count = waveManager.CurrentWave;
                SpawnEnemies(count);
            }
        }

        private void SpawnEnemies(int count)
        {
            var newEnemies = new List<(int, int)>();
            
            for (var i = 0; i < count; i++)
            {
                var x = Random.Range(0, 7);
                var y = Random.Range(0, 7);

                while (!(Grid[x, y] is null) || newEnemies.Contains((x, y)))
                {
                    x = Random.Range(0, 7);
                    y = Random.Range(0, 7);
                }
                
                newEnemies.Add((x, y));
                var position = Grid.GridToWorld(x, y) + new Vector3(0.5f, 0.0f, 0.5f);
                Instantiate(enemyPrefab, position, Quaternion.identity);
            }
        }

        public void SpawnAlly(Vector3 position, float health, float strength, float focus, float agility)
        {
            Grid.WorldToGrid(position, out var x, out var y);
            position = Grid.GridToWorld(x, y) + new Vector3(0.5f, 0.0f, 0.5f);

            var obj = Instantiate(allyPrefab, position, Quaternion.identity).GetComponent<PlayerEntity>();
            obj.maxHealth = health;
            obj.health = health;
            obj.healthBar.SetHealth(health, health);
            obj.strength = strength;
            obj.focus = focus;
            obj.agility = agility;
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

        private void OnDestroy()
        {
            _destroyed = true;
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