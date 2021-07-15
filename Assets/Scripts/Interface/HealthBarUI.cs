using System;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class HealthBarUI : MonoBehaviour
    {
        public Slider slider;
        public Color low = Color.red;
        public Color medium = Color.yellow;
        public Color high = Color.green;
        public Vector3 offset;

        public void SetHealth(float health, float maxHealth)
        {
            slider.gameObject.SetActive(health < maxHealth);
            slider.value = health;
            slider.maxValue = maxHealth;

            float healthRatio = health / maxHealth;
            slider.fillRect.GetComponentInChildren<Image>().color =
                healthRatio > 0.66 ? high : healthRatio > 0.33 ? medium : low;
        }

        private void Update()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        }
    }
}