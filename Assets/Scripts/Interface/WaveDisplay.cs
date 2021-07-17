using System;
using Arena;
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
            WaveManager.Instance.WaveChanged += OnWaveChange;
        }

        private void OnWaveChange(object sender, EventArgs args)
        {
            var waveManager = (WaveManager) sender;
            _text.text = $"Wave: {waveManager.CurrentWave}";
        }
    }
}