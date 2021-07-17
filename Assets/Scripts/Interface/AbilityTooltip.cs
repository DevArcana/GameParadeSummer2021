using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class AbilityTooltip : MonoBehaviour
    {
        public int slot;
        private AbilityBar _bar;

        private void Start()
        {
            _bar = transform.parent.GetComponent<AbilityBar>();
        }

        public void DisplayTooltip()
        {
            var ability = _bar.slots?.GetAbility(slot);

            if (ability != null)
            {
                Tooltip.Instance.Enable($"Cost: {ability.Cost} AP\n{ability.Tooltip}");
            }
        }

        public void HideTooltip()
        {
            Tooltip.Instance.Disable();
        }
    }
}