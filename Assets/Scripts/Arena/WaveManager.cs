using System;
using UnityEngine;

namespace Arena
{
    public class WaveManager : MonoBehaviour
    {
        #region Singleton

        public static WaveManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public event EventHandler WaveChanged;

        protected virtual void OnWaveChanged()
        {
            WaveChanged?.Invoke(this, EventArgs.Empty);
        }

        private int _currentWave = 1;

        public int CurrentWave
        {
            get => _currentWave;
            set
            {
                _currentWave = value;
                OnWaveChanged();
            }
        }

        public void NextWave()
        {
            CurrentWave++;
        }
    }
}