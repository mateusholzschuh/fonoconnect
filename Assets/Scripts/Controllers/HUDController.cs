using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour {

    public Image healthImage;
    public Text scoreText;

    public static float health;
    public static int score;

	void Start () {
        health = 100.0f;
        score = 0;
	}
	
	void Update () {
        healthImage.fillAmount = health / 100;
        scoreText.text = score.ToString();

        //Check if the player is alive
        if(health <= 0) { //isDead
            //End of the game
            //Gambiarra por enquanto
            SceneManager.LoadScene("PatientMenu");
        }
    }

}
