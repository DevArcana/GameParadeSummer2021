using Ability;
using Ability.Abilities;
using UnityEngine;
using Visuals;

namespace Arena
{
    public class PlayerEntity : GridEntity
    {
        private Camera _camera;

        private bool _canMove;

        protected override void Start()
        {
            base.Start();
            
            _camera = Camera.main;
            _canMove = true;
            health = maxHealth = 20;
            strength = 4;
            agility = 2;
            focus = 2;
            healthBar.SetHealth(health, maxHealth);

            TurnManager.Instance.TurnStarted += OnTurnStart;
            TurnManager.Instance.TurnEnded += OnTurnEnd;
            
            abilitySlots.AbilitySelectionChanged += OnAbilitySelectionChanged;
            abilitySlots.Deselect();
        }
        
        private void OnAbilitySelectionChanged(object sender, AbilitySlots.AbilitySelectionChangedEventArgs args)
        {
            var slot = args.Slot;
            AbilityAreaDisplay.Instance.DisplayAreaFor(slot == -1 ? moveAbility : abilitySlots.SelectedAbility);
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
                    var target = parent != null ? parent.GetComponent<GridEntity>() : null;

                    var selectedAbility = abilitySlots.SelectedAbility;

                    if (selectedAbility != null)
                    {
                        AbilityManager.Instance.Use(selectedAbility, hit.point, target, () =>
                        {
                            _canMove = true;
                            abilitySlots.SetAbility(abilitySlots.SelectedSlot, null);
                            abilitySlots.Deselect();
                        },
                        () =>
                        {
                            _canMove = true;
                            abilitySlots.Deselect();
                        });
                    }
                    else
                    {
                        TryMoveOrAttack(hit.point, target);
                    }
                }
            }
        }

        private void TryMoveOrAttack(Vector3 position, GridEntity target)
        {
            AbilityManager.Instance.Use(moveAbility, position, target, () =>
            {
                if (TurnManager.Instance.CurrentTurn == this)
                {
                    AbilityAreaDisplay.Instance.DisplayAreaFor(moveAbility);
                }
                _canMove = true;
            }, () =>
            {
                AbilityManager.Instance.Use(attackAbility, position, target, () =>
                {
                    _canMove = true;
                }, () =>
                {
                    _canMove = true;
                });
            });
        }

        private void OnTurnStart(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            if (args.Entity == this)
            {
                abilitySlots.Deselect();
                abilitySlots.PopulateAbilities(this);
            }
        }

        private void OnTurnEnd(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            abilitySlots.SetAbility(0, null);
            abilitySlots.SetAbility(1, null);
            abilitySlots.SetAbility(2, null);
            abilitySlots.Deselect();
        }

        protected override void OnDestroy()
        {
            TurnManager.Instance.TurnStarted -= OnTurnStart;
            TurnManager.Instance.TurnEnded -= OnTurnEnd;
            abilitySlots.AbilitySelectionChanged -= OnAbilitySelectionChanged;
            base.OnDestroy();
        }
    }
}