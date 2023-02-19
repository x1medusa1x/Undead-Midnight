using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static void PlaySound(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }

    public static void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
