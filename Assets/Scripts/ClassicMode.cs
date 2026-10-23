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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        yield return new WaitUntil(() =>
         Input.GetKeyDown(KeyCode.A) ||
         Input.GetKeyDown(KeyCode.S) ||
         (Input.GetKeyDown(KeyCode.D) && playerAmmo > 0)
         );

         if (Input.GetKeyDown(KeyCode.A)) {
            playerAmmo = playerAmmo + 1;
            pAmmo.text = "Player Ammo: " + playerAmmo.ToString();
            player = "reload";
         }
         else if (Input.GetKeyDown(KeyCode.S)) {
            player = "block";
         } else if (Input.GetKeyDown(KeyCode.D) && playerAmmo > 0) {
            playerAmmo = playerAmmo - 1;
            pAmmo.text = "Player Ammo: " + playerAmmo.ToString();
            player = "fire";
         }

         //TODO: for whoever is doing the animations, here is where you would put the pat pat animation (hands tapping)
         yield return new WaitForSeconds(2);
         string enemy = enemyAction();

         //If a player is shot while reloading, they lose.
         //If both players shoot at the same time, there is a tie.
         //Otherwise, the shot is blocked and the game continues.
         //TODO: for the animations, change the displayed sprites here
         if (player == "fire" && enemy == "reload") {
             enemyAlive = false;
             result.text = "Player 1 wins!";
         }
         else if (enemy == "fire" && player == "reload") {
             playerAlive = false;
             result.text = "Computer player wins!";
         }
         else if (player == "fire" && enemy == "fire") {
             result.text = "Draw...";
         }

         else if (playerAlive == true && enemyAlive == true && (player == "fire" || enemy == "fire") || (player == "block" && enemy == "block")) {
            result.text = "Blocked!";
            StartCoroutine(WaitForInput());
         }
         else if (playerAlive == true && enemyAlive == true && (player == "reload" || enemy == "reload")) {
            result.text = "Reloading...";
            StartCoroutine(WaitForInput());
         }
    }

    string enemyAction() {
        //If the enemy has ammo, it can shoot. Otherwise, it can only reload or block.
        if (enemyAmmo > 0) {
            int action = Random.Range(1, 4);
            if (action == 1) {
                enemyAmmo = enemyAmmo + 1;
                eAmmo.text = "Enemy Ammo: " + enemyAmmo.ToString();
                return "reload";
            } else if (action == 2) {
                return "block";
            }else {
                enemyAmmo = enemyAmmo - 1;
                eAmmo.text = "Enemy Ammo: " + enemyAmmo.ToString();
                return "fire";
            }
        } else {
            int action = Random.Range(1, 3);
            if (action == 1) {
                enemyAmmo = enemyAmmo + 1;
                eAmmo.text = "Enemy Ammo: " + enemyAmmo.ToString();
                return "reload";
            } else {
                return "block";
            }
        }
    }
}
