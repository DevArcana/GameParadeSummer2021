﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Arena
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }
        
        public int ActionPoints { get; private set; }

        public Queue<GridEntity> Entities { get; set; } = new Queue<GridEntity>();

        public GridEntity CurrentTurn => Entities.Any() ? Entities.Peek() : null;

        #region OnTurnStarted

        public class OnTurnStartedEventArgs : EventArgs
        {
            public GridEntity Entity;
        }
        
        public event EventHandler<OnTurnStartedEventArgs> TurnStarted;
        
        public void OnTurnStarted(GridEntity entity)
        {
            TurnStarted?.Invoke(this, new OnTurnStartedEventArgs {Entity = entity});
        }

        #endregion

        #region OnTurnEnded

        public class OnTurnEndedEventArgs : EventArgs
        {
            public GridEntity Entity;
        }
        
        public event EventHandler<OnTurnEndedEventArgs> TurnEnded;
        
        public void OnTurnEnded(GridEntity entity)
        {
            TurnEnded?.Invoke(this, new OnTurnEndedEventArgs {Entity = entity});
        }

        #endregion
        
        #region OnActionPointsChanged

        public class OnActionPointsChangedEventArgs : EventArgs
        {
            public int ActionPointsSpent;
        }

        public event EventHandler<OnActionPointsChangedEventArgs> ActionPointsChanged;
        
        public void OnActionPointsChanged(int points)
        {
            ActionPointsChanged?.Invoke(this, new OnActionPointsChangedEventArgs {ActionPointsSpent = points});
        }

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ActionPoints = 2;
        }

        public bool IsPlayerTurn()
        {
            return Entities.Peek() is PlayerEntity;
        }
        
        public bool CanSpendActionPoints(int amount = 1)
        {
            return ActionPoints >= amount;
        }
        
        public bool SpendActionPoints(int amount = 1)
        {
            if (!CanSpendActionPoints(amount))
            {
                return false;
            }
            
            ActionPoints -= amount;
            OnActionPointsChanged(amount);

            return true;
        }

        public void FinishTurn()
        {
            ActionPoints = 2;
            OnActionPointsChanged(-2);
            var last = Entities.Dequeue();
            Entities.Enqueue(last);
            OnTurnEnded(last);
            StartCoroutine(Delay(1, () =>
            {
                OnTurnStarted(Entities.Peek());
            }));
        }

        public IEnumerator Delay(int seconds, [CanBeNull] Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        public void Enqueue(GridEntity entity)
        {
            Entities.Enqueue(entity);

            // if (Entities.Count == 1)
            // {
            //     OnTurnChanged(null, entity);
            // }
        }
    }
}