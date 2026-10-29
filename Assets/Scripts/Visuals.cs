using UnityEngine;
using System.Collections;
using Duel;

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

    // Show the action choice of both players
    public void ShowActions(Actions player1, Actions player2)
    {
        ShowSprites(GetActionSprite(player1, true), GetActionSprite(player2, false));
    }

    // Show the winner and the fainted player(s) at the end of the game
    public void ShowResult(Round round)
    {
        switch (round)
        {
            case Round.PlayerWins:
                ShowSprites(player1Win, player2Fainted);
                break;
            case Round.EnemyWins:
                ShowSprites(player1Fainted, player2Win);
                break;
            case Round.Draw:
                ShowSprites(player1Fainted, player2Fainted);
                break;
        }
    }

    private void ShowSprites(Sprite player1, Sprite player2)
    {
        clapRenderer.enabled = false;
        player1Renderer.enabled = true;
        player2Renderer.enabled = true;
        player1Renderer.sprite = player1;
        player2Renderer.sprite = player2;
    }

    private Sprite GetActionSprite(Actions action, bool isPlayer1)
    {
        return action switch
        {
            Actions.Reloading => isPlayer1 ? player1Reload : player2Reload,
            Actions.Block => isPlayer1 ? player1Block : player2Block,
            Actions.Fire => isPlayer1 ? player1Fire : player2Fire,
            _ => isPlayer1 ? player1Choosing : player2Choosing
        };
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