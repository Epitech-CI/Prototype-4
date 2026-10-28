using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Duel;

public class TimedMode : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI result;

    [SerializeField] public TextMeshProUGUI pAmmo;

    [SerializeField] public TextMeshProUGUI eAmmo;

    [SerializeField] public TextMeshProUGUI timer;
    
    [Header("Visuals")]
    [SerializeField] private Visuals visuals;


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
            SceneManager.LoadScene("Timed Mode");
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
            Actions? input = null;
            float timer = 0f;
            while (timer < 1f)
            {
                input ??= _input.ReadAction();
                timer += Time.deltaTime;
                yield return null;
                this.timer.text = "Timer: " + timer.ToString("0.0");
            }

            Actions player = input ?? Actions.Nothing;
            _rules.UpdateAmmo(player, true);

            //TODO: for whoever is doing the animations, here is where you would put the pat pat animation (hands tapping)
            visuals.PlayClapClap();
            yield return new WaitForSeconds(2);
            Actions enemy = _rules.EnemyAction();
            UpdateAmmoUI();

            //TODO: for the animations, change the displayed sprites here
            visuals.ShowActions(player, enemy);
            Round resolveRound = _rules.ResolveRound(player, enemy);
            result.text = (resolveRound) switch
            {
                Round.PlayerWins => "Player 1 wins",
                Round.EnemyWins => "Computer player wins",
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

    void ResetResult()
    {
        result.text = "";
    }
}