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
            slider.maxValue = maxHealth;
            slider.value = health;

            float healthRatio = health / maxHealth;
            slider.fillRect.GetComponentInChildren<Image>().color =
                healthRatio > 0.66 ? high : healthRatio > 0.33 ? medium : low;
            
            slider.GetComponentInChildren<Text>().text = $"{health}/{maxHealth}";
        }

        private void Update()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        }
    }
}