using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class ClassicMode : MonoBehaviour
{
    int playerAmmo = 0;
    int enemyAmmo = 0;
    bool playerAlive = true;
    bool enemyAlive = true;

    [SerializeField]
    public TextMeshProUGUI result;
    public TextMeshProUGUI pAmmo;
    public TextMeshProUGUI eAmmo;

    [Header("Visuals")]
    [SerializeField] private Visuals visuals;

    [Header("Sounds")]
    [SerializeField] private ResultSound resultSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pAmmo.text = "Player Ammo: 0";
        eAmmo.text = "Enemy Ammo: 0";
        StartCoroutine(WaitForInput());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Classic Mode");
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            SceneManager.LoadScene("Main Menu");
        }
    }

    IEnumerator WaitForInput() 
    {
        string player = "";

        visuals.PlayerRest();
        result.text = "Choose your action...";

        yield return new WaitUntil(() =>
         Input.GetKeyDown(KeyCode.A) ||
         Input.GetKeyDown(KeyCode.S) ||
         (Input.GetKeyDown(KeyCode.D) && playerAmmo > 0)
         );

         if (Input.GetKeyDown(KeyCode.A)) {
            playerAmmo = playerAmmo + 1;
            player = "reload";
         }
         else if (Input.GetKeyDown(KeyCode.S)) {
            player = "block";
         } else if (Input.GetKeyDown(KeyCode.D) && playerAmmo > 0) {
            playerAmmo = playerAmmo - 1;
            player = "fire";
         }

        //Hand tap then wait 2 seconds
        visuals.PlayClapClap();
        result.text = "";
        yield return new WaitForSeconds(2);
        string enemy = enemyAction();

        visuals.ShowActionPlayer1(player);
        visuals.ShowActionPlayer2(enemy);
        resultSound.PlayResultSound(player, enemy);

        pAmmo.text = "Player Ammo: " + playerAmmo.ToString();
        eAmmo.text = "Enemy Ammo: " + enemyAmmo.ToString();

        //If a player is shot while reloading, they lose.
        //If both players shoot at the same time, there is a tie.
        //Otherwise, the shot is blocked and the game continues.
        if (player == "fire" && enemy == "reload") {
            enemyAlive = false;
            result.text = "Shots fired!";
            yield return new WaitForSeconds(1);
            result.text = "Player 1 wins!";
            visuals.ShowActionPlayer1("win");
            visuals.ShowActionPlayer2("fainted");

        }
         else if (enemy == "fire" && player == "reload") {
            playerAlive = false;
            result.text = "Shots fired!";
            yield return new WaitForSeconds(1);
            result.text = "Computer player wins!";
            visuals.ShowActionPlayer1("fainted");
            visuals.ShowActionPlayer2("win");
        }
         else if (player == "fire" && enemy == "fire") {
            result.text = "Shots fired!";
            yield return new WaitForSeconds(1);
            result.text = "Draw...";
            visuals.ShowActionPlayer1("fainted");
            visuals.ShowActionPlayer2("fainted");
        }
         else if (playerAlive == true && enemyAlive == true && (player == "fire" || enemy == "fire") || (player == "block" && enemy == "block")) {
            result.text = "Blocked!";
            yield return new WaitForSeconds(1);
            StartCoroutine(WaitForInput());
         }
         else if (playerAlive == true && enemyAlive == true && (player == "reload" || enemy == "reload")) {
            result.text = "Reloading...";
            yield return new WaitForSeconds(1);
            StartCoroutine(WaitForInput());
         }
    }

    string enemyAction() {
        //If the enemy has ammo, it can shoot. Otherwise, it can only reload or block.
        if (enemyAmmo > 0) {
            int action = Random.Range(1, 4);
            if (action == 1) {
                enemyAmmo = enemyAmmo + 1;
                return "reload";
            } else if (action == 2) {
                return "block";
            }else {
                enemyAmmo = enemyAmmo - 1;
                return "fire";
            }
        } else {
            int action = Random.Range(1, 3);
            if (action == 1) {
                enemyAmmo = enemyAmmo + 1;
                return "reload";
            } else {
                return "block";
            }
        }
    }
}
