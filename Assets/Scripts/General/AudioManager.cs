using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("AudioSource")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("Scene SoundTrack")]
    public AudioClip currentSceneSoundTrack;

    [Header("Audio Clip (SFX)")]
    public AudioClip button;
    public AudioClip file;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic(currentSceneSoundTrack);
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
}
