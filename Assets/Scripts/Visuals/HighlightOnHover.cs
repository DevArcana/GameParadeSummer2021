using Arena;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    public class HighlightOnHover : MonoBehaviour
    {
        public float lerpFactor = 1.0f;
        
        public Color onHoverColor;
        public Color onClickColor;

        private Color _defaultColor;
        
        private Renderer _renderer;

        private Text _text;
        private Slider _slider;
        private bool _isCharacter;

        private enum HoverStatus
        {
            None,
            Hover,
            Click
        }

        private HoverStatus _status = HoverStatus.None;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();

            _defaultColor = _renderer.material.color;
            
            if (transform.parent.CompareTag("Character"))
            {
                var parent = transform.parent;
                _text = parent.GetComponentInChildren<Text>(true);
                _slider = parent.GetComponentInChildren<Slider>(true);
                _isCharacter = true;
            }
        }

        private void OnMouseEnter()
        {
            _status = HoverStatus.Hover;
            if (_isCharacter)
            {
                _slider.gameObject.SetActive(true);
                _text.enabled = true;
            }
        }

        private void OnMouseExit()
        {
            _status = HoverStatus.None;
            if (_isCharacter)
            {
                _slider.gameObject.SetActive(_slider.value < _slider.maxValue);
                _text.enabled = false;
            }
        }

        private void OnMouseDown()
        {
            _status = HoverStatus.Click;
        }

        private void OnMouseUp()
        {
            _status = HoverStatus.Hover;
        }

        private void Update()
        {
            var color = _renderer.material.color;
            var targetColor = _defaultColor;
            
            if (_status == HoverStatus.Hover)
            {
                targetColor = onHoverColor;
            }
            else if (_status == HoverStatus.Click)
            {
                targetColor = onClickColor;
            }
            
            _renderer.material.color = Color.Lerp(color, targetColor, lerpFactor * Time.deltaTime);
        }
    }
}
