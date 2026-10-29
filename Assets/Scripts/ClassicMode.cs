using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Duel;

public class ClassicMode : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI result;

    [SerializeField] public TextMeshProUGUI pAmmo;

    [SerializeField] public TextMeshProUGUI eAmmo;

    [Header("Visuals")] [SerializeField] private Visuals visuals;

    private DuelRules _rules = new DuelRules();
    private DuelInput _input = new DuelInput();


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
            SceneManager.LoadScene("Classic Mode");
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
            result.text = "Choose your action...";

            Actions? input = _input.ReadAction();

            while (input == null)
            {
                yield return null;
                input = _input.ReadAction();
            }

            Actions player = input.Value;
            if (player == Actions.Fire && !_rules.CanFire(true))
            {
                player = Actions.Nothing;
            }

            _rules.UpdateAmmo(player, true);

            visuals.PlayClapClap();
            result.text = "";
            yield return new WaitForSeconds(2);
            Actions enemy = _rules.EnemyAction();

            visuals.ShowActions(player, enemy);
            UpdateAmmoUI();

            Round resolveRound = _rules.ResolveRound(player, enemy);

            result.text = (resolveRound) switch
            {
                Round.PlayerWins => "Player 1 wins",
                Round.EnemyWins => "Computer player wins",
                Round.Draw => "Draw ...",
                Round.Blocked => "Blocked",
                Round.Reload => "Reloading ...",
                Round.Continue => "Next round",
                _ => throw new ArgumentOutOfRangeException()
            };
            yield return new WaitForSeconds(1);
            if (_rules.IsGameOver(resolveRound))
            {
                visuals.ShowResult(resolveRound);
                yield return new WaitForSeconds(2);
                result.text = "Game Over";
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    void UpdateAmmoUI()
    {
        pAmmo.text = "Player Ammo: " + _rules.PlayerAmmo;
        eAmmo.text = "Enemy Ammo: " + _rules.EnemyAmmo;
    }
}