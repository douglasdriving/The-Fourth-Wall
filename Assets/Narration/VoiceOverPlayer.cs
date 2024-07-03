using UnityEngine;

public class VoiceOverPlayer : MonoBehaviour
{
    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayClip(AudioClip clip) //could have a subtitle? or that might be a different player.
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
