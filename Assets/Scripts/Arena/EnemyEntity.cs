using System;
using System.Linq;
using Ability;
using Ability.Abilities;
using Random = UnityEngine.Random;

namespace Arena
{
    public class EnemyEntity : GridEntity
    {
        private MovementAbility _movement;
        private MeleeAbility _melee;
        
        protected override void Start()
        {
            base.Start();
            TurnManager.Instance.TurnStarted += OnTurnStarted;
            health = maxHealth = 8;
            damage = 2;
            healthBar.SetHealth(health, maxHealth);

            _movement = new MovementAbility(this);
            _melee = new MeleeAbility(this);
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

            var moves = gameArena.Grid.GetAvailableNeighbours(x, y).ToList();
            var move = moves.ElementAt(Random.Range(0, moves.Count));

            position = gameArena.Grid.GridToWorld(move.x, move.y);

            gameArena.Grid.WorldToGrid(position, out x, out y);
            var target = gameArena.Grid[x, y];
            var abilities = AbilityManager.Instance;
            
            if (abilities.CanUse(_movement, position, target))
            {
                abilities.Use(_movement, position, target, OnActionSuccess, () => throw new InvalidOperationException("should never happen"));
            }
            else if (abilities.CanUse(_melee, position, target))
            {
                abilities.Use(_melee, position, target, OnActionSuccess, () => throw new InvalidOperationException("should never happen"));
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