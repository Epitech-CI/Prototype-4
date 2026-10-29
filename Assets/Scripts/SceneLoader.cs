using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadClassic()
    {
        SceneManager.LoadScene("Classic Mode");
    }

    public void LoadVersus()
    {
        SceneManager.LoadScene("Versus Mode");
    }

    public void LoadTimed()
    {
        SceneManager.LoadScene("Timed Mode");
    }

    public void LoadTimedVersus()
    {
        SceneManager.LoadScene("Timed Versus Mode");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}