using System;
using Ability;
using Arena;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    public Button abilityButton1;
    public Button abilityButton2;
    public Button abilityButton3;
    
    public Image abilityButtonImage1;
    public Image abilityButtonImage2;
    public Image abilityButtonImage3;
    
    public TextMeshProUGUI abilityButtonText1;
    public TextMeshProUGUI abilityButtonText2;
    public TextMeshProUGUI abilityButtonText3;

    private AbilitySlots _slots;
    
    private void Start()
    {
        abilityButton1.onClick.AddListener(() =>
        {
            SelectSlot(0);
        });
        
        abilityButton2.onClick.AddListener(() =>
        {
            SelectSlot(1);
        });
        
        abilityButton3.onClick.AddListener(() =>
        {
            SelectSlot(2);
        });

        var turnManager = TurnManager.Instance;
        turnManager.ActionPointsChanged += ActionPointsChanged;
        _slots = turnManager.CurrentTurn.abilitySlots;
        if (_slots != null)
        {
            _slots.AbilitySelectionChanged += AbilitySelectionChanged;
        }
        RefreshSlots();
        turnManager.TurnStarted += OnTurnChange;
    }

    private void ActionPointsChanged(object sender, EventArgs e)
    {
        RefreshSlots();
    }

    private void OnTurnChange(object sender, TurnManager.OnTurnChangeEventArgs args)
    {
        if (args.Entity is PlayerEntity player)
        {
            if (_slots != null)
            {
                _slots.AbilitySelectionChanged -= AbilitySelectionChanged;
            }
            _slots = player.abilitySlots;
            _slots.AbilitySelectionChanged += AbilitySelectionChanged;
            
            RefreshSlots();
        }
    }

    private void RefreshSlots()
    {
        if (_slots == null)
        {
            return;
        }

        var slot = _slots.SelectedSlot;

        var ap = TurnManager.Instance.ActionPoints;
        
        var ability = _slots.GetAbility(0);
        abilityButtonText1.text = ability != null ? ability.Name : "";
        abilityButtonImage1.color = slot == 0 ? new Color(0.7f, 0.7f, 0.7f) : ability == null || ability.Cost > ap ? Color.gray : Color.white;
        
        ability = _slots.GetAbility(1);
        abilityButtonText2.text = ability != null ? ability.Name : "";
        abilityButtonImage2.color = slot == 1 ? new Color(0.7f, 0.7f, 0.7f) : ability == null || ability.Cost > ap ? Color.gray : Color.white;
        
        ability = _slots.GetAbility(2);
        abilityButtonText3.text = ability != null ? ability.Name : "";
        abilityButtonImage3.color = slot == 2 ? new Color(0.7f, 0.7f, 0.7f) : ability == null || ability.Cost > ap ? Color.gray : Color.white;
    }

    private void SelectSlot(int slot)
    {
        if (_slots == null)
        {
            return;
        }

        if (_slots.SelectedSlot == slot)
        {
            _slots.Deselect();
            return;
        }
        
        _slots.SelectAbility(slot);
    }

    private void AbilitySelectionChanged(object sender, AbilitySlots.AbilitySelectionChangedEventArgs args)
    {
        RefreshSlots();
    }
}
