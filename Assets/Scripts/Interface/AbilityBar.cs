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
        _slots = turnManager.CurrentTurn.abilitySlots;
        if (_slots != null)
        {
            _slots.AbilitySelectionChanged += AbilitySelectionChanged;
        }
        RefreshSlots();
        turnManager.TurnStarted += OnTurnChange;
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
        
        abilityButtonImage1.color = slot == 0 ? Color.gray : Color.white;
        abilityButtonImage2.color = slot == 1 ? Color.gray : Color.white;
        abilityButtonImage3.color = slot == 2 ? Color.gray : Color.white;

        var ability1 = _slots.GetAbility(0);
        abilityButtonText1.text = ability1 != null ? ability1.Name : "";
        
        var ability2 = _slots.GetAbility(1);
        abilityButtonText2.text = ability2 != null ? ability2.Name : "";
        
        var ability3 = _slots.GetAbility(2);
        abilityButtonText3.text = ability3 != null ? ability3.Name : "";
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
        }
        
        _slots.SelectAbility(slot);
    }

    private void AbilitySelectionChanged(object sender, AbilitySlots.AbilitySelectionChangedEventArgs args)
    {
        RefreshSlots();
    }
}
