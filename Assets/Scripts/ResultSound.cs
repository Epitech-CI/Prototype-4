using UnityEngine;

public class ResultSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip shieldSound;

    public void PlayResultSound(string player1, string player2)
    {
        if ((player1 == "fire" && player2 != "block") || (player2 == "fire" && player1 != "block")) {
            audioSource.PlayOneShot(shotSound);
        } else if (player1 == "fire" || player2 == "fire" || (player1 == "block" && player2 == "block")) {
            audioSource.PlayOneShot(shieldSound);
        } else {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
