using FrostweepGames.SpeechRecognition.Google.Cloud;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBoxController : MonoBehaviour {
    //Google Speech Recognizer instance
    private ILowLevelSpeechRecognition _speechRecognition;

    public Text   wordText;
    public Text   remainingAttempts;
    public Text   remainingText;
    public Text   messageText;
    public Button record;
    public Button stop;

    private int remainig;
    private string wordRecognized;    

    private void Start() {
        //Setting up the library
        _speechRecognition = SpeechRecognitionModule.Instance;
        _speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

        //Pause the game
        TimeControl.PauseGame();

        //Set the listener to buttons
        record.onClick.AddListener(StartRecordButtonOnClickHandler);
        stop.onClick.AddListener(StopRecordButtonOnClickHandler);

        //Set the initial chances
        remainig = 3;
        remainingAttempts.text = remainig.ToString();

    }

    public void SetWord(string word) {
        wordText.text = word;
    }

    private void OnDestroy() {
        _speechRecognition.SpeechRecognizedSuccessEvent -= SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent -= SpeechRecognizedFailedEventHandler;
        TimeControl.ResumeGame();
    }

    private void StartRecordButtonOnClickHandler() {
        Debug.Log("Started record");
        record.transform.gameObject.SetActive(false);
        stop.transform.gameObject.SetActive(true);
        remainingText.text = "Tentando reconhecer...";
        _speechRecognition.StartRecord();
    }

    private void StopRecordButtonOnClickHandler() {
        Debug.Log("Stoped record!");
        record.transform.gameObject.SetActive(true);
        stop.transform.gameObject.SetActive(false);
        record.interactable = false;
        _speechRecognition.StopRecord();
    }

    private void SpeechRecognizedFailedEventHandler(string obj) {
        Debug.Log("Speech Recognition failed. Error: " + obj);
        record.interactable = true;
    }

    private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj) {
        Debug.Log("Success + " + obj);
        if (obj != null && obj.results.Length > 0) {
            //Get the first result from recognizer
            wordRecognized = obj.results[0].alternatives[0].transcript;
            Debug.Log("'" + wordRecognized + "' it's said.");

            //Check if the recognized is equal the word
            checkWord();
        }
        else {
            Debug.Log("Words are no detected");
        }

        record.interactable = true;
        //wordText.text = wordRecognized;
    }

    private void checkWord() {
        if(wordRecognized != null) {
            if ( wordText.text.Equals(wordRecognized, System.StringComparison.InvariantCultureIgnoreCase) ) {
                //Set the message
                messageText.transform.gameObject.SetActive(true);
                remainingAttempts.transform.gameObject.SetActive(false);
                remainingText.transform.gameObject.SetActive(false);

                //Add score
                HUDController.score += 1400 * Mathf.RoundToInt(Time.time);

                Destroy(this.gameObject);
            }
            else {
                remainig--;
                remainingAttempts.text = remainig.ToString();
            }
        }
    }


}
