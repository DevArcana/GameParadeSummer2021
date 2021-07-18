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
            var random = new Random();

            const double secondSlotTwoCostProbability = 0.5;
            const double thirdSlotThreeCostProbability = 0.5;
            
            SetAbility(0, AbilityFactory.GetRandomOneCostAbility(entity));
            SetAbility(1, random.NextDouble() <= secondSlotTwoCostProbability ? AbilityFactory.GetRandomTwoCostAbility(entity) : AbilityFactory.GetRandomOneCostAbility(entity));
            SetAbility(2, random.NextDouble() <= thirdSlotThreeCostProbability ? AbilityFactory.GetRandomThreeCostAbility(entity) : AbilityFactory.GetRandomTwoCostAbility(entity));
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