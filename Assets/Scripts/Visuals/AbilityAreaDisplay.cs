using System.Collections.Generic;
using Ability;
using Arena;
using UnityEngine;

namespace Visuals
{
    public class AbilityAreaDisplay : MonoBehaviour
    {
        #region Singleton

        public static AbilityAreaDisplay Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public Transform tilePrefab;
        private readonly List<GameObject> _fields = new List<GameObject>();

        public void ClearDisplay()
        {
            foreach (var field in _fields)
            {
                Destroy(field);
            }
        
            _fields.Clear();
        }

        public void DisplayAreaFor(BaseAbility ability)
        {
            ClearDisplay();
        
            var arena = GameArena.Instance;
            var grid = arena.Grid;

            var area = ability.GetArea();

            foreach (var field in area)
            {
                var position = grid.GridToWorld(field.x, field.y) + new Vector3(0.5f, 0.1f, 0.5f);
                var instance = Instantiate(tilePrefab, position, Quaternion.identity, transform);
            
                _fields.Add(instance.gameObject);
            }
        }
    }
}
