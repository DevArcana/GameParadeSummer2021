using System;
using Arena;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class ActionPointsUI : MonoBehaviour
    {
        public Toggle actionPoint1;
        public Toggle actionPoint2;

        private void Start()
        {
            actionPoint1.isOn = false;
            actionPoint2.isOn = false;
            
            TurnManager.Instance.ActionPointsChanged += OnActionPointsChanged;
            OnActionPointsChanged(TurnManager.Instance, null);
        }

        public void OnActionPointsChanged(object sender, TurnManager.OnActionPointsChangedEventArgs args)
        {
            var manager = (TurnManager) sender;

            if (manager.ActionPoints == 0)
            {
                actionPoint1.isOn = false;
                actionPoint2.isOn = false;
            }
            else if (manager.ActionPoints == 1)
            {
                actionPoint1.isOn = true;
                actionPoint2.isOn = false;
            }
            else
            {
                actionPoint1.isOn = true;
                actionPoint2.isOn = true;
            }
        }
    }
}