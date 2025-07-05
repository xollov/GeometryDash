using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [HideInInspector]
    public AudioSource musicSource;
    [HideInInspector]
    public AudioSource sfxSource;

    public AudioClip musicClip;
    public AudioClip sfxCollision;

    [Range(0,1)]
    public float musicVolume;
    [Range(0,1)]
    public float sfxVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
       // sfxSource = new AudioSource();
    }
    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
        //sfxSource.clip = sfxCollision;
    }
    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
        musicVolume = value;
    }
    public void ChangeSfxVolume(float value)
    {
        sfxSource.volume = value;
        sfxVolume = value;
    }
}
