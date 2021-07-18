using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Ability;
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
                var turnManager = TurnManager.Instance;
                foreach (var ally in turnManager.EnqueuedEntities.Where(x => x is PlayerEntity))
                {
                    ally.health = ally.maxHealth;
                    ally.healthBar.SetHealth(ally.maxHealth, ally.maxHealth);
                }
                turnManager.ResetActionPoints();
                
                var waveManager = (WaveManager) sender;
                SpawnEnemies(waveManager.CurrentWave);
                
                turnManager.CurrentTurn.abilitySlots.PopulateAbilities(turnManager.CurrentTurn);
                turnManager.CurrentTurn.abilitySlots.Deselect();
            }
        }

        private void SpawnEnemies(int wave)
        {
            var newEnemies = new List<(int, int)>();
            var health = 2.0f + wave;
            var armour = Math.Max(0, -3.0f + wave);
            var strength = 1.25f + 0.25f * wave;
            var focus = 0.5f + 0.5f * wave;
            var agility = 0.5f + 0.5f * wave;

            var enemiesCount = wave / 2 + 1;
            
            for (var i = 0; i < enemiesCount; i++)
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
                var entity = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<EnemyEntity>();
                entity.SetAttributes(health, armour, strength, focus, agility);
            }
        }

        public void SpawnAlly(Vector3 position, float health, float strength, float focus, float agility)
        {
            Grid.WorldToGrid(position, out var x, out var y);
            position = Grid.GridToWorld(x, y) + new Vector3(0.5f, 0.0f, 0.5f);

            var entity = Instantiate(allyPrefab, position, Quaternion.identity).GetComponent<PlayerEntity>();
            entity.SetAttributes(health, 0, strength, focus, agility);
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

        public IEnumerator Swap(GridEntity firstEntity, GridEntity secondEntity, [CanBeNull] Action onFinish = null)
        {
            Grid.WorldToGrid(firstEntity.transform.position, out var firstX, out var firstY);
            Grid.WorldToGrid(secondEntity.transform.position, out var secondX, out var secondY);

            var firstNewPos = Grid.GridToWorld(secondX, secondY);
            var secondNewPos = Grid.GridToWorld(firstX, firstY);
            
            Grid[firstX, firstY] = secondEntity;
            Grid[secondX, secondY] = firstEntity;

            var coroutines = new List<IEnumerator>
            {
                firstEntity.Move(new Vector3(firstNewPos.x + 0.5f, firstNewPos.y, firstNewPos.z + 0.5f)),
                secondEntity.Move(new Vector3(secondNewPos.x + 0.5f, secondNewPos.y, secondNewPos.z + 0.5f))
            };

            yield return this.WaitAllCoroutine(coroutines, onFinish);
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