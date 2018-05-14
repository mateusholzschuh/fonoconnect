using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsButtons : MonoBehaviour {
 
    public void LoadLoginScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadCreateAccScene()
    {
        SceneManager.LoadScene(1);
    }
   
    public void LoadPatientMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void die()
    {
        HUDController.health = 0;
    }

}
