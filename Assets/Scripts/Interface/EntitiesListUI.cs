using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Interface
{
    public class EntitiesListUI : MonoBehaviour
    {
        public Transform playerIndicatorPrefab;
        public Transform enemyIndicatorPrefab;

        private readonly List<Animator> _queue = new List<Animator>();
        private static readonly int TriggerDestroy = Animator.StringToHash("Destroy");

        private void Start()
        {
            TurnManager.Instance.TurnEnded += OnTurnEnded;

            foreach (var entity in TurnManager.Instance.Entities)
            {
                var prefab = enemyIndicatorPrefab;
                if (entity is PlayerEntity)
                {
                    prefab = playerIndicatorPrefab;
                }
            
                _queue.Add(Instantiate(prefab, transform).GetComponent<Animator>());
            }
        }

        private void OnTurnEnded(object sender, TurnManager.OnTurnEndedEventArgs args)
        {
            var anim = _queue[0];
            _queue.RemoveAt(0);
            anim.SetTrigger(TriggerDestroy);

            var prefab = enemyIndicatorPrefab;
            if (args.Entity is PlayerEntity)
            {
                prefab = playerIndicatorPrefab;
            }
            
            _queue.Add(Instantiate(prefab, transform).GetComponent<Animator>());
        }
    }
}
