using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Interface
{
    public class BackgroundMusic : MonoBehaviour
    {
        public AudioClip[] playlist;
        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            StartCoroutine(PlayAudio());
        }

        private IEnumerator PlayAudio()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (!_audio.isPlaying)
                {
                    var track = playlist.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    _audio.clip = track;
                    _audio.Play();
                }
            }
        }
    }
}
