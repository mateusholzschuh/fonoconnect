using UnityEngine;
using UnityEngine.UI;

public class DEBUG_SPEECH : MonoBehaviour {

   /* public Text wordText;
    public Image wordImage;
    public Text remainingAttempts;
    public Button actionButton;
    */
    //Debug Buttons
    public Button rightAnswer;
    public Button wrongAnswer;
    /*
    public Sprite mic;
    public Sprite stop;

    /////////////////////////
    private int remaining;

    /////////////////////////

    int test = 1;
    */
	// Use this for initialization
	void Start () {
        //remaining = 3;
        rightAnswer.onClick.AddListener(DRightButton);
        wrongAnswer.onClick.AddListener(DWrongButton);
        
	}

    void DRightButton()
    {
        //remaining++;
        //remainingAttempts.text = remaining.ToString();
        HUDController.score += 1500 * (int) Time.deltaTime + 700;
        TimeControl.ResumeGame();
        Destroy(this.gameObject);
    }

    void DWrongButton()
    {
       // remaining--;
        //remainingAttempts.text = remaining.ToString();
        HUDController.health -= 25;
        TimeControl.ResumeGame();
        Destroy(this.gameObject);
    }
    /*
    public void recordbutton()
    {
        if (test == 1)
        {
            actionButton.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = stop;
            test = 0;
        }
        else
        {
            actionButton.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = mic;
            test = 1;

            TimeControl.ResumeGame();
        }
        
    }*/
}
