using UnityEngine;
using Duel;

public class ResultSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip shieldSound;

    public void PlayResultSound(Actions player1, Actions player2)
    {
        if ((player1 == Actions.Fire && player2 != Actions.Block) || (player2 == Actions.Fire && player1 != Actions.Block)) {
            audioSource.PlayOneShot(shotSound);
        } else if (player1 == Actions.Fire || player2 == Actions.Fire || (player1 == Actions.Block && player2 == Actions.Block)) {
            audioSource.PlayOneShot(shieldSound);
        } else {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
