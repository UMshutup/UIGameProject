using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Audio Clip")]
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip battleMusic;
    [SerializeField] public AudioClip UI;
    [SerializeField] public AudioClip hurt;
    [SerializeField] public AudioClip claivettaCry;
    [SerializeField] public AudioClip mugsquitoCry;
    [SerializeField] public AudioClip signuleiCry;

    public void PlayMusic(AudioClip clip, float volume)
    {
        musicSource.loop = true;
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayUISound()
    {
        SFXSource.PlayOneShot(UI);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
