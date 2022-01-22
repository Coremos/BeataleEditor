using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public float CurrentTime
    {
        get
        {
            return 1;
        }
    }
    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void OnClickPlayButton()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.time = 0.0f;
        }
        else
        {
            AudioSource.Play();
        }
    }

    public void OnClickPauseButton()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }
        else
        {
            AudioSource.Play();
        }

    }

    public void OnClickStopButton()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }
    }

}
