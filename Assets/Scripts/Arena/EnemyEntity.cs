using System;
using System.Linq;
using Ability;
using Ability.Abilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        private BasicMoveAbility _basicMove;
        private BasicAttackAbility _basicAttack;
        
        protected override void Start()
        {
            base.Start();
            TurnManager.Instance.TurnStarted += OnTurnStarted;
            health = maxHealth = 8;
            strength = 2;
            agility = 2;
            focus = 2;
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

            var moves = gameArena.Grid.GetCardinalAtEdge(x, y, 1).ToList();
            var move = moves.ElementAt(Random.Range(0, moves.Count));

            position = gameArena.Grid.GridToWorld(move.x, move.y);

            gameArena.Grid.WorldToGrid(position, out x, out y);
            var target = gameArena.Grid[x, y];
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