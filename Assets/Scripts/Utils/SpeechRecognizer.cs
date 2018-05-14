using FrostweepGames.SpeechRecognition.Google.Cloud;
using UnityEngine;
using UnityEngine.UI;


public class SpeechRecognizer : MonoBehaviour {
    private ILowLevelSpeechRecognition _speechRecognition;

    private string wordRecognized;

    /** DEBUG BUTTONS **/
    public Button RECORD;
    public Button STOP;

    private void Start() {
        _speechRecognition = SpeechRecognitionModule.Instance;
        _speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

        /*** DEBUG ***/
        RECORD.onClick.AddListener(StartRecordButtonOnClickHandler);
        STOP.onClick.AddListener(StopRecordButtonOnClickHandler);
    }

    private void OnDestroy() {
        _speechRecognition.SpeechRecognizedSuccessEvent -= SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent -= SpeechRecognizedFailedEventHandler;
    }


    public string getWord() {
        return wordRecognized;
    }

    private void StartRecordButtonOnClickHandler() {
        Debug.Log("Started record");
        _speechRecognition.StartRecord();
    }

    private void StopRecordButtonOnClickHandler() {
        Debug.Log("Stoped record!");
        _speechRecognition.StopRecord();
    }

    private void SpeechRecognizedFailedEventHandler(string obj) {
        Debug.Log("Speech Recognition failed. Error: " + obj);
        /*_speechRecognitionState.color = Color.green;
        _speechRecognitionResult.text = "Speech Recognition failed with error: " + obj;*/

        /* _startRecordButton.interactable = true;
            _stopRecordButton.interactable = false;*/
    }

    private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj) {
        if (obj != null && obj.results.Length > 0) {
            //Get the first result from recognizer
            wordRecognized = obj.results[0].alternatives[0].transcript;
            Debug.Log(wordRecognized + " it's said.");
        }
        else {
            Debug.Log("Words are no detected");
        }
        /* _startRecordButton.interactable = true;

            _speechRecognitionState.color = Color.green;

            if (obj != null && obj.results.Length > 0)
            {
                tentativas--;
                _speechRecognitionResult.text = "Speech Recognition succeeded! Detected Most useful: " + obj.results[0].alternatives[0].transcript;
                palavraFalada = obj.results[0].alternatives[0].transcript;
                descobrirFonemas(); //descobrir os fonemas da palavra sendo pronunciada
                testarPronuncia();
            }
            else
            {
                _speechRecognitionResult.text = "Speech Recognition succeeded! Words are no detected.";
                //tentativas++;

            }*/
    }
}