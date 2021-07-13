using System;
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
        }
    }
}