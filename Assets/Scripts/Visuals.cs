using UnityEngine;
using System.Collections;

public class Visuals : MonoBehaviour
{
    [Header("Players")]
    // Player 1
    [SerializeField] private SpriteRenderer player1Renderer;
    // Player 2 or computer
    [SerializeField] private SpriteRenderer player2Renderer;


    // Sprites for Player 1
    [Header("Player 1 Sprites")]
    [SerializeField] private Sprite player1Choosing;
    [SerializeField] private Sprite player1Reload;
    [SerializeField] private Sprite player1Block;
    [SerializeField] private Sprite player1Fire;
    [SerializeField] private Sprite player1Win;
    [SerializeField] private Sprite player1Fainted;

    // Sprites for Computer or Player 2
    [Header("Computer / Player 2 Sprites")]
    [SerializeField] private Sprite player2Choosing;
    [SerializeField] private Sprite player2Reload;
    [SerializeField] private Sprite player2Block;
    [SerializeField] private Sprite player2Fire;
    [SerializeField] private Sprite player2Win;
    [SerializeField] private Sprite player2Fainted;


    // Animation for the clap
    [Header("Clap Clap")]
    [SerializeField] private Animator clapAnimator;
    [SerializeField] private SpriteRenderer clapRenderer;
    private Coroutine clapCoroutine;


    // When players are choosing the action
    public void PlayerRest()
    {
        player1Renderer.enabled = true;
        player2Renderer.enabled = true;
        player1Renderer.sprite = player1Choosing;
        player2Renderer.sprite = player2Choosing;
    }

    // Show the action choice
    public void ShowActionPlayer1(string action)
    {
        clapRenderer.enabled = false;
        player1Renderer.enabled = true;
        player1Renderer.sprite = GetSprite(action, true);
    }

    public void ShowActionPlayer2(string action)
    {
        clapRenderer.enabled = false;
        player2Renderer.enabled = true;
        player2Renderer.sprite = GetSprite(action, false);
    }

    private Sprite GetSprite(string action, bool isPlayer1)
    {
        switch (action.ToLowerInvariant())
        {
            case "reload":
                return isPlayer1 ? player1Reload : player2Reload;
            case "block":
                return isPlayer1 ? player1Block : player2Block;
            case "fire":
                return isPlayer1 ? player1Fire : player2Fire;
            case "win":
                return isPlayer1 ? player1Win : player2Win;
            case "fainted":
                return isPlayer1 ? player1Fainted : player2Fainted;
            default:
                return isPlayer1 ? player1Choosing : player2Choosing;
        }
    }

    public void PlayClapClap()
    {
        if (clapCoroutine != null)
        {
            StopCoroutine(clapCoroutine);
        }

        clapCoroutine = StartCoroutine(PlayClapAnimation());
    }

    private IEnumerator PlayClapAnimation()
    {
        player1Renderer.enabled = false;
        player2Renderer.enabled = false;

        clapRenderer.enabled = true;

        clapAnimator.Play("Clapclap", 0, 0f);
        clapAnimator.Update(0f);

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = clapAnimator.GetCurrentAnimatorStateInfo(0);

            return state.IsName("Clapclap") &&
                   state.normalizedTime >= 1f;
        });

        clapRenderer.enabled = false;
        clapCoroutine = null;
    }

    private void Start()
    {
        PlayerRest();

        clapRenderer.enabled = false;
    }
}