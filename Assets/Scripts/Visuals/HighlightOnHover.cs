using UnityEngine;

namespace Visuals
{
    public class HighlightOnHover : MonoBehaviour
    {
        public float lerpFactor = 1.0f;
        
        public Color onHoverColor;
        public Color onClickColor;

        private Color _defaultColor;
        
        private Renderer _renderer;

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
        }

        private void OnMouseEnter()
        {
            _status = HoverStatus.Hover;
        }

        private void OnMouseExit()
        {
            _status = HoverStatus.None;
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
