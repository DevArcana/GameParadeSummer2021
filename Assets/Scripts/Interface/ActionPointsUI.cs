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
        public Toggle actionPoint3;

        private void Start()
        {
            actionPoint1.isOn = false;
            actionPoint2.isOn = false;
            actionPoint3.isOn = false;
            
            TurnManager.Instance.ActionPointsChanged += OnActionPointsChanged;
            OnActionPointsChanged(TurnManager.Instance, EventArgs.Empty);
        }

        public void OnActionPointsChanged(object sender, EventArgs args)
        {
            var manager = (TurnManager) sender;

            if (manager.ActionPoints == 0)
            {
                actionPoint1.isOn = false;
                actionPoint2.isOn = false;
                actionPoint3.isOn = false;
            }
            else if (manager.ActionPoints == 1)
            {
                actionPoint1.isOn = true;
                actionPoint2.isOn = false;
                actionPoint3.isOn = false;
            }
            else if (manager.ActionPoints == 2)
            {
                actionPoint1.isOn = true;
                actionPoint2.isOn = true;
                actionPoint3.isOn = false;
            }
            else
            {
                actionPoint1.isOn = true;
                actionPoint2.isOn = true;
                actionPoint3.isOn = true;
            }
        }
    }
}