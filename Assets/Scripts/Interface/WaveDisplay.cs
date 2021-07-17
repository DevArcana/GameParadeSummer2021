using TMPro;
using UnityEngine;

namespace Interface
{
    public class WaveDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
        }

        private void OnWaveChange()
        {
            
        }
    }
}