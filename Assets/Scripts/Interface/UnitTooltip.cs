using Arena;
using UnityEngine;

namespace Interface
{
    public class UnitTooltip : MonoBehaviour
    {
        private GridEntity _entity;
        
        private void Start()
        {
            _entity = transform.parent.GetComponent<GridEntity>();
        }

        private void OnMouseEnter()
        {
            var tooltip =
                $"{_entity.EntityName}\nHealth: {_entity.health}/{_entity.maxHealth}\nStrength: {_entity.strength}\nFocus: {_entity.focus}\nAgility: {_entity.agility}";
            Tooltip.Instance.Enable(tooltip);
        }

        private void OnMouseExit()
        {
            Tooltip.Instance.Disable();
        }
    }
}