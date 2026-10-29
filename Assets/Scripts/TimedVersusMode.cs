using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Duel;

public class TimedVersusMode : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI result;

    [SerializeField] public TextMeshProUGUI pAmmo;

    [SerializeField] public TextMeshProUGUI eAmmo;

    [SerializeField] public TextMeshProUGUI timer;

    [Header("Visuals")] [SerializeField] private Visuals visuals;

    [Header("Sounds")] [SerializeField] private ResultSound resultSound;

    private DuelRules _rules = new DuelRules();
    private DuelInput _p1Input = new DuelInput(KeyCode.A, KeyCode.S, KeyCode.D);
    private DuelInput _p2Input = new DuelInput(KeyCode.J, KeyCode.K, KeyCode.L);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateAmmoUI();
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Timed Versus Mode");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }


    IEnumerator GameLoop()
    {
        while (true)
        {
            visuals.PlayerRest();
            ResetResult();
            Actions? p1 = null;
            Actions? p2 = null;
            float timer = 0f;
            while (timer < 1f)
            {
                p1 ??= _p1Input.ReadAction();
                p2 ??= _p2Input.ReadAction();
                timer += Time.deltaTime;
                this.timer.text = "Timer: " + timer.ToString("0.0");
                yield return null;
            }

            Actions player1 = p1 ?? Actions.Nothing;
            Actions player2 = p2 ?? Actions.Nothing;
            if (player1 == Actions.Fire && !_rules.CanFire(true))
            {
                player1 = Actions.Nothing;
            }

            if (player2 == Actions.Fire && !_rules.CanFire(false))
            {
                player2 = Actions.Nothing;
            }

            _rules.UpdateAmmo(player1, true);
            _rules.UpdateAmmo(player2, false);
            visuals.PlayClapClap();

            yield return new WaitForSeconds(2);

            UpdateAmmoUI();
            visuals.ShowActions(player1, player2);
            resultSound.PlayResultSound(player1, player2);

            Round resolveRound = _rules.ResolveRound(player1, player2);
            result.text = resolveRound switch
            {
                Round.PlayerWins => "Player 1 wins",
                Round.EnemyWins => "Player 2 wins",
                Round.Draw => "Draw ...",
                Round.Blocked => "Blocked",
                Round.Reload => "Reloading ...",
                Round.Continue => "Next Round",
                _ => throw new ArgumentOutOfRangeException(nameof(resolveRound), resolveRound, null)
            };
            yield return new WaitForSeconds(1);

            if (_rules.IsGameOver(resolveRound))
            {
                visuals.ShowResult(resolveRound);
                yield return new WaitForSeconds(2);
                result.text = "Game Over";
                yield break;
            }
        }
    }

    void UpdateAmmoUI()
    {
        pAmmo.text = "Player 1 Ammo: " + _rules.PlayerAmmo;
        eAmmo.text = "Player 2 Ammo: " + _rules.EnemyAmmo;
    }

    void ResetResult()
    {
        result.text = "";
    }
}