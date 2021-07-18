using System;
using System.Collections.Generic;
using System.Linq;
using Ability;
using Ability.Abilities;
using AI;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        private BasicMoveAbility _basicMove;
        private BasicAttackAbility _basicAttack;

        public Queue<Vector2Int> movesQueue;
        public Vector2Int? targetPos;
        
        protected override void Start()
        {
            base.Start();
            TurnManager.Instance.TurnStarted += OnTurnStarted;
            if (health == 0 || maxHealth == 0) health = maxHealth = 8;
            if (strength == 0) strength = 2;
            if (agility == 0) agility = 2;
            if (focus == 0) focus = 2;
            healthBar.SetHealth(health, maxHealth);

            _basicMove = new BasicMoveAbility(this);
            _basicAttack = new BasicAttackAbility(this);
        }

        public void OnTurnStarted(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            if (args.Entity == this)
            {
                MakeAction();
            }
        }

        private void MakeAction()
        {
            var gameArena = GameArena.Instance;
            
            var position = transform.position;
                
            gameArena.Grid.WorldToGrid(position, out var x, out var y);

            // var moves = gameArena.Grid.GetCardinalAtEdge(x, y, 1).ToList();
            // var move = moves.ElementAt(Random.Range(0, moves.Count));
            if (targetPos is null)
            {
                var pathfinding = new Pathfinding();
                targetPos = pathfinding.FindTarget(x, y);
                if (targetPos is null)
                {
                    TurnManager.Instance.NextTurn();
                    return;
                }
                movesQueue = pathfinding.FindPath(x, y, targetPos.Value.x, targetPos.Value.y);
            }
            
            // position = gameArena.Grid.GridToWorld(move.x, move.y);
            // gameArena.Grid.WorldToGrid(position, out x, out y);
            // var target = gameArena.Grid[x, y];

            Vector2Int move;
            if (!(movesQueue is null) && movesQueue.Count > 0)
            {
                move = movesQueue.Dequeue();
            }
            else if (Vector2Int.Distance(targetPos.Value, new Vector2Int(x, y)) == 1)
            {
                move = targetPos.Value;
            }
            else
            {
                TurnManager.Instance.NextTurn();
                return;
            }
            position = gameArena.Grid.GridToWorld(move.x, move.y);
            var target = gameArena.Grid[move.x, move.y];
            
            var abilities = AbilityManager.Instance;
            
            if (abilities.CanUse(_basicMove, position, target))
            {
                abilities.Use(_basicMove, position, target, OnActionSuccess, () => throw new InvalidOperationException("should never happen"));
            }
            else if (abilities.CanUse(_basicAttack, position, target))
            {
                abilities.Use(_basicAttack, position, target, OnActionSuccess, () => throw new InvalidOperationException("should never happen"));
            }
            else if (TurnManager.Instance.CurrentTurn == this && TurnManager.Instance.ActionPoints > 0)
            {
                MakeAction();
            }
        }

        private void OnActionSuccess()
        {
            if (TurnManager.Instance.ActionPoints > 0)
            {
                MakeAction();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}