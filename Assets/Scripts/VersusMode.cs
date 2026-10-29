using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Duel;

public class VersusMode : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI result;
    public TextMeshProUGUI pAmmo;
    public TextMeshProUGUI eAmmo;

    [Header("Visuals")] [SerializeField] private Visuals visuals;

    [Header("Sounds")] [SerializeField] private ResultSound resultSound;

    private DuelRules _rules = new DuelRules();
    private DuelInput _player1 = new DuelInput(KeyCode.A, KeyCode.S, KeyCode.D);
    private DuelInput _player2 = new DuelInput(KeyCode.J, KeyCode.K, KeyCode.L);

    void Start()
    {
        UpdateAmmoUI();
        StartCoroutine(WaitForInput());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Versus Mode");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    IEnumerator WaitForInput()
    {
        while (true)
        {
            visuals.PlayerRest();

            Actions? p1 = null;
            Actions? p2 = null;
            ShowReadyStatus(p1, p2);

            while (p1 == null || p2 == null)
            {
                p1 ??= ReadAction(_player1, true);
                p2 ??= ReadAction(_player2, false);
                ShowReadyStatus(p1, p2);
                yield return null;
            }

            Actions player1 = p1.Value;
            Actions player2 = p2.Value;

            _rules.UpdateAmmo(player1, true);
            _rules.UpdateAmmo(player2, false);

            visuals.PlayClapClap();
            result.text = "";
            yield return new WaitForSeconds(2);

            UpdateAmmoUI();
            visuals.ShowActions(player1, player2);
            resultSound.PlayResultSound(player1, player2);

            Round resolveRound = _rules.ResolveRound(player1, player2);

            if (_rules.IsGameOver(resolveRound))
            {
                result.text = "Shots fired!";
                yield return new WaitForSeconds(1);
                result.text = resolveRound switch
                {
                    Round.PlayerWins => "Player 1 wins!",
                    Round.EnemyWins => "Player 2 wins!",
                    _ => "Draw..."
                };
                visuals.ShowResult(resolveRound);
                yield break;
            }

            bool blocked = resolveRound == Round.Blocked ||
                           (player1 == Actions.Block && player2 == Actions.Block);
            result.text = blocked ? "Blocked!" : "Reloading...";
            yield return new WaitForSeconds(1);
        }
    }

    // Firing without ammo is ignored so the player keeps choosing instead of losing the turn
    Actions? ReadAction(DuelInput input, bool isPlayer1)
    {
        Actions? action = input.ReadAction();
        if (action == Actions.Fire && !_rules.CanFire(isPlayer1))
        {
            return null;
        }

        return action;
    }

    void ShowReadyStatus(Actions? player1, Actions? player2)
    {
        string status1 = player1 == null ? "choosing..." : "ready!";
        string status2 = player2 == null ? "choosing..." : "ready!";
        result.text = "P1: " + status1 + "      P2: " + status2;
    }

    void UpdateAmmoUI()
    {
        pAmmo.text = "Player 1 Ammo: " + _rules.PlayerAmmo;
        eAmmo.text = "Player 2 Ammo: " + _rules.EnemyAmmo;
    }
}
