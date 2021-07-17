using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arena;
using UnityEngine;

namespace Ability
{
    public enum AbilityTag
    {
        SelfTargeted,
        AllyTargeted,
        EnemyTargeted,
        AreaTargeted,
        NoTarget,
        Mobility,
        Damage,
        Healing,
        Buff,
        Ranged,
        Melee,
        AreaOfEffect,
        Shield,
        Sacrifice,
        Execute,
        Clone,
        SingleTarget
    }
    
    public abstract class BaseAbility
    {
        public GridEntity AbilityUser { get; }
        
        public abstract int Cost { get; }
        public abstract string Name { get; }
        public abstract string Tooltip { get; }
        public abstract HashSet<AbilityTag> Tags { get; }

        public virtual List<Vector2Int> GetArea()
        {
            return GameArena.Instance.Grid.GetWholeGrid().ToList();
        }

        public abstract IEnumerator Execute(Vector3 position, GridEntity targetEntity, Action onFinish);

        public virtual bool CanExecute(Vector3 position, GridEntity targetEntity)
        {
            return true;
        }
        
        protected BaseAbility(GridEntity user)
        {
            AbilityUser = user;
        }
    }
}