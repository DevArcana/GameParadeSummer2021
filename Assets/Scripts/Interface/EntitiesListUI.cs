using System.Collections.Generic;
using Arena;
using TMPro;
using UnityEngine;

namespace Interface
{
    public class EntitiesListUI : MonoBehaviour
    {
        public Transform playerIndicatorPrefab;
        public Transform enemyIndicatorPrefab;

        private readonly List<(Animator, GridEntity)> _queue = new List<(Animator, GridEntity)>();
        private static readonly int TriggerDestroy = Animator.StringToHash("Destroy");

        private void Start()
        {
            TurnManager.Instance.TurnEnded += OnTurnEnded;
            TurnManager.Instance.EntityDequeued += OnEntityDequeued;
            TurnManager.Instance.EntityEnqueued += OnEntityEnqueued;

            foreach (var entity in TurnManager.Instance.EnqueuedEntities)
            {
                var prefab = enemyIndicatorPrefab;
                if (entity is PlayerEntity)
                {
                    prefab = playerIndicatorPrefab;
                }
            
                var instance = Instantiate(prefab, transform);
                instance.GetComponentInChildren<TextMeshProUGUI>().text = entity.EntityName;
                _queue.Add((instance.GetComponent<Animator>(), entity));
            }
        }
        
        private void OnEntityEnqueued(object sender, TurnManager.EntityEventArgs args)
        {
            var entity = args.Entity;
            var prefab = enemyIndicatorPrefab;
            if (entity is PlayerEntity)
            {
                prefab = playerIndicatorPrefab;
            }

            var instance = Instantiate(prefab, transform);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = entity.EntityName;
            _queue.Add((instance.GetComponent<Animator>(), entity));
        }

        private void OnEntityDequeued(object sender, TurnManager.EntityEventArgs args)
        {
            var item = _queue.Find(x => x.Item2 == args.Entity);
            _queue.Remove(item);

            if (item.Item1 == null) return;
            item.Item1.SetTrigger(TriggerDestroy);
        }

        private void OnTurnEnded(object sender, TurnManager.OnTurnChangeEventArgs args)
        {
            var (anim, gridEntity) = _queue[0];
            _queue.RemoveAt(0);
            anim.SetTrigger(TriggerDestroy);

            var prefab = enemyIndicatorPrefab;
            if (args.Entity is PlayerEntity)
            {
                prefab = playerIndicatorPrefab;
            }
            
            var instance = Instantiate(prefab, transform);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = gridEntity.EntityName;
            _queue.Add((instance.GetComponent<Animator>(), gridEntity));
        }
    }
}
