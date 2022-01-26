using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource m_track1;
    [SerializeField] private AudioSource m_track2;
    private AudioSource mainTrack;

    [SerializeField] private AudioClip m_trappedMusic;
    [SerializeField] private AudioClip m_menuMusic;
    [SerializeField] private AudioClip m_standardMusic;

    private static readonly float m_fadeTime = 0.5f;

    public void UpdateMusicVolume()
    {
        mainTrack.volume = PlayerStats.musicVolume;
    }

    public void PlayMenuMusic()
    {
        StartCoroutine(PlayMusic(m_menuMusic));
    }

    public void PlayStandardMusic()
    {
        StartCoroutine(PlayMusic(m_standardMusic));
    }

    public void PlayTrappedMusic()
    {
        StartCoroutine(PlayMusic(m_trappedMusic));
    }

    private IEnumerator PlayMusic(AudioClip newAudio)
    {
        if(newAudio != null)
        {
            //If the current main track is track 1, start playing new audio on track 2 and set track 2 to 0 volume (and vice cersa)

            AudioSource newMainTrack = null;
            AudioSource oldMainTrack = null;

            if (mainTrack == m_track1)
            {
                newMainTrack = m_track2;
                oldMainTrack = m_track1;
            }
            else
            {
                newMainTrack = m_track1;
                oldMainTrack = m_track2;
            }
            newMainTrack.clip = newAudio;
            newMainTrack.volume = 0.0f;
            newMainTrack.Play();

            float t = 0.0f;


            while(t < m_fadeTime)
            {
                t = Mathf.Clamp(t + Time.deltaTime, 0.0f, m_fadeTime);
                newMainTrack.volume = Mathf.Lerp(0, 1, t / m_fadeTime) * PlayerStats.musicVolume;
                if (oldMainTrack != null)
                {
                    oldMainTrack.volume = Mathf.Lerp(1, 0, t / m_fadeTime) * PlayerStats.musicVolume;
                }
                yield return null;
            }
            mainTrack = newMainTrack;
            mainTrack.volume = PlayerStats.musicVolume;
            if (oldMainTrack != null)
            {
                oldMainTrack.Pause();
            }

        }
    }

}
