using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class Tooltip : MonoBehaviour
    {
        public static Tooltip Instance { get; private set; }

        private void Start()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            Disable();
        }

        private void Awake()
        {
            Instance = this;
        }

        private TextMeshProUGUI _text;
        private Image _image;

        public void Disable()
        {
            _image.enabled = false;
            _text.enabled = false;
        }

        public void Enable(string tooltip)
        {
            _image.enabled = true;
            _text.enabled = true;
            _text.text = tooltip;
        }
    }
}
