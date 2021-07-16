using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Visuals;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        #region Singleton

        public static TurnManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            ActionPoints = 2;
        }

        #endregion

        public int ActionPoints { get; private set; }

        public List<GridEntity> EnqueuedEntities { get; } = new List<GridEntity>();
        public GridEntity CurrentTurn => EnqueuedEntities.Any() ? EnqueuedEntities[0] : null;

        #region OnTurnChange

        public class OnTurnChangeEventArgs : EventArgs
        {
            public GridEntity Entity { get; set; }
        }

        public event EventHandler<OnTurnChangeEventArgs> TurnStarted;
        public event EventHandler<OnTurnChangeEventArgs> TurnEnded;

        private void OnTurnStarted(GridEntity entity)
        {
            TurnStarted?.Invoke(this, new OnTurnChangeEventArgs {Entity = entity});
        }

        private void OnTurnEnded(GridEntity entity)
        {
            TurnEnded?.Invoke(this, new OnTurnChangeEventArgs {Entity = entity});
        }

        #endregion

        #region OnActionPointsChanged

        public event EventHandler ActionPointsChanged;

        private void OnActionPointsChanged()
        {
            ActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region OnEntityDequeued

        public class OnEntityDequeuedEventArgs : EventArgs
        {
            public GridEntity Entity { get; set; }
        }

        public event EventHandler<OnEntityDequeuedEventArgs> EntityDequeued;

        private void OnEntityDequeued(GridEntity entity)
        {
            EntityDequeued?.Invoke(this, new OnEntityDequeuedEventArgs {Entity = entity});
        }

        #endregion

        public bool TrySpendActionPoint() => TrySpendActionPoints(1);
        
        public bool TrySpendActionPoints(int amount)
        {
            if (ActionPoints < amount)
            {
                return false;
            }
            
            ActionPoints -= amount;
            OnActionPointsChanged();

            return true;
        }

        public void NextTurn()
        {
            AbilityAreaDisplay.Instance.ClearDisplay();
            ActionPoints = 2;
            OnActionPointsChanged();
            
            var last = CurrentTurn;
            EnqueuedEntities.RemoveAt(0);
            EnqueuedEntities.Add(last);
            
            OnTurnEnded(last);
            StartCoroutine(Delay(0.25f, () =>
            {
                OnTurnStarted(CurrentTurn);
            }));
        }

        public IEnumerator Delay(float seconds, [CanBeNull] Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        public void Enqueue(GridEntity entity)
        {
            EnqueuedEntities.Add(entity);

            if (EnqueuedEntities.Count == 1)
            {
                OnTurnStarted(entity);
            }
        }

        public void Dequeue(GridEntity entity)
        {
            EnqueuedEntities.Remove(entity);
            OnEntityDequeued(entity);
        }
    }
}