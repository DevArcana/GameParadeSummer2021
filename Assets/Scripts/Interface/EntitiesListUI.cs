using System.Collections.Generic;
using Arena;
using UnityEngine;

namespace Interface
{
    public class EntitiesListUI : MonoBehaviour
    {
        public Transform playerIndicatorPrefab;
        public Transform enemyIndicatorPrefab;

        private readonly Queue<Animator> _queue = new Queue<Animator>();
        private static readonly int TriggerDestroy = Animator.StringToHash("Destroy");

        private void Start()
        {
            TurnManager.Instance.TurnChanged += OnTurnChanged;

            foreach (var entity in TurnManager.Instance.Entities)
            {
                var prefab = enemyIndicatorPrefab;
                if (entity is PlayerEntity)
                {
                    prefab = playerIndicatorPrefab;
                }
            
                _queue.Enqueue(Instantiate(prefab, transform).GetComponent<Animator>());
            }
        }

        private void OnTurnChanged(object sender, TurnManager.OnTurnChangedEventArgs args)
        {
            var anim = _queue.Dequeue();
            //anim.transform.SetParent(null);
            //anim.ResetTrigger(TriggerDestroy);
            anim.SetTrigger(TriggerDestroy);
            Debug.Log("ANimt");

            var prefab = enemyIndicatorPrefab;
            if (args.LastTurn is PlayerEntity)
            {
                prefab = playerIndicatorPrefab;
            }
            
            _queue.Enqueue(Instantiate(prefab, transform).GetComponent<Animator>());
        }
    }
}
