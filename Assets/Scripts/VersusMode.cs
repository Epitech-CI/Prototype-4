using UnityEngine;
using System;
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
    private DuelRules _rules = new DuelRules();
    private DuelInput _player1 = new DuelInput();
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
            result.text = "Choose your action...";

            Actions? p1 = null;
            Actions? p2 = null;

            while (p1 == null || p2 == null)
            {
                p1 ??= this._player1.ReadAction();
                p2 ??= this._player2.ReadAction();
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
        pAmmo.text = "Player Ammo: " + _rules.PlayerAmmo;
        eAmmo.text = "Enemy Ammo: " + _rules.EnemyAmmo;
    }
}