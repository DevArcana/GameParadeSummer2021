using Ability;
using UnityEngine;

namespace Arena
{
    public class PlayerEntity : GridEntity
    {
        private Camera _camera;

        private bool _canMove = false;

        private BaseAbility ability;
        
        protected override void Start()
        {
            base.Start();
            ActionManager.Instance.ActionProcessed += OnActionProcessed;
            _camera = Camera.main;
            _canMove = true;
            health = maxHealth = 20;
            damage = 4;
            healthBar.SetHealth(health, maxHealth);

            ability = new MovementAbility(this);
        }

        private void Update()
        {
            if (!_canMove || TurnManager.Instance.CurrentTurn != this)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    _canMove = false;
                    AbilityManager.Instance.Use(ability, hit.point, () =>
                    {
                        _canMove = true;
                    }, () =>
                    {
                        _canMove = true;
                    });
                    // ActionManager.Instance.TryMove(this, hit.point);
                }
            }
        }

        private void OnActionProcessed(object sender, ActionManager.OnActionProcessedEventArgs args)
        {
            if (args.Entity == this)
            {
                _canMove = true;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}