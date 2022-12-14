using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoParty
{
    public class Utility : MonoBehaviour
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            // Source: https://answers.unity.com/questions/301868/yield-waitforseconds-outside-of-timescale.html
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }

        //@Source: https://forum.unity.com/threads/fade-out-audio-source.335031/
        public static IEnumerator CSoundFadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        public static IEnumerator CSoundFadeIn(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            audioSource.volume = 0;

            audioSource.Play();
            while (audioSource.volume < startVolume)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }
        }
    }
}

