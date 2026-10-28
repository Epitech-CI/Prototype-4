using UnityEngine;

public class ClapSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clapSound;

    public void PlayClapSound()
    {
        audioSource.PlayOneShot(clapSound);
    }
}