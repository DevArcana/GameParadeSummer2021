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
            Tooltip.Instance.Enable(_entity.EntityName);
        }

        private void OnMouseExit()
        {
            Tooltip.Instance.Disable();
        }
    }
}