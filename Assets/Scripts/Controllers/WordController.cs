using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour {

    [Header("UI GameObjects")]
    public Text wordText;
    public Image wordSprite;

    [Header("SpeechBox prefab")]
    public GameObject speechBoxPrefab;

    //////////////////////////////
    private string word;
    private string wordSpriteName;
    //////////////////////////////

    void Start() {
        //Connect to the database and get the informations

        //End connection
        //wordText.text     = word;
        //wordSprite.sprite = Resources.Load<Sprite>("Sprites/" + wordSpriteName);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Instantiate the speech box
        GameObject speechBox = Instantiate(speechBoxPrefab, GameObject.Find("Canvas").transform) as GameObject;
        speechBox.GetComponent<SpeechBoxController>().SetWord(wordText.text.ToUpper());

        //Fix the bug of multiples hits in the same barrier
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
