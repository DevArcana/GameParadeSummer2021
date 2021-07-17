using System;
using Ability.Abilities;
using Arena;

namespace Ability
{
    public class AbilitySlots
    {
        private BaseAbility[] _slots = new BaseAbility[3];

        public BaseAbility SelectedAbility => SelectedSlot != -1 ? _slots[SelectedSlot] : null;
        public int SelectedSlot { get; private set; } = -1;

        public class AbilitySelectionChangedEventArgs : EventArgs
        {
            public int Slot { get; set; }
        }

        public event EventHandler<AbilitySelectionChangedEventArgs> AbilitySelectionChanged;
        
        private void OnAbilitySelectionChanged()
        {
            AbilitySelectionChanged?.Invoke(this, new AbilitySelectionChangedEventArgs {Slot = SelectedSlot});
        }

        public bool HasAbility(int slot)
        {
            return _slots[slot] != null;
        }

        public void PopulateAbilities(GridEntity entity)
        {
            SetAbility(0, AbilityFactory.GetRandomAbility(entity));
            SetAbility(1, AbilityFactory.GetRandomAbility(entity));
            SetAbility(2, AbilityFactory.GetRandomAbility(entity));
        }

        public void SetAbility(int slot, BaseAbility ability)
        {
            _slots[slot] = ability;
            if (SelectedSlot == slot)
            {
                Deselect();
            }
        }

        public BaseAbility GetAbility(int slot)
        {
            return _slots[slot];
        }

        public bool SelectAbility(int slot)
        {
            if (!HasAbility(slot))
            {
                return false;
            }

            SelectedSlot = slot;
            OnAbilitySelectionChanged();

            return true;
        }

        public void Deselect()
        {
            SelectedSlot = -1;
            OnAbilitySelectionChanged();
        }
    }
}