using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadClassic() 
    {
        SceneManager.LoadScene("Classic Mode");
    }
}
