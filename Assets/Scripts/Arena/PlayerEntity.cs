﻿using Ability;
using Ability.Abilities;
using UnityEngine;

namespace Arena
{
    public class PlayerEntity : GridEntity
    {
        private Camera _camera;

        private bool _canMove = false;

        private MovementAbility _movement;
        private MeleeAbility _melee;
        
        protected override void Start()
        {
            base.Start();
            ActionManager.Instance.ActionProcessed += OnActionProcessed;
            _camera = Camera.main;
            _canMove = true;
            health = maxHealth = 20;
            damage = 4;
            healthBar.SetHealth(health, maxHealth);

            _movement = new MovementAbility(this);
            _melee = new MeleeAbility(this);
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
                    var parent = hit.transform.parent;
                    AbilityManager.Instance.Use(_movement, hit.point, parent != null ? parent.GetComponent<GridEntity>() : null, () =>
                    {
                        _canMove = true;
                    }, () =>
                    {
                        AbilityManager.Instance.Use(_melee, hit.point, parent != null ? parent.GetComponent<GridEntity>() : null, () =>
                        {
                            _canMove = true;
                        }, () =>
                        {
                            _canMove = true;
                        });
                    });
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