/*using FrostweepGames.SpeechRecognition.Utilites;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

namespace FrostweepGames.SpeechRecognition.Google.Cloud.Examples
{
	public class example2 : MonoBehaviour
	{
		private ILowLevelSpeechRecognition _speechRecognition;

		private Button _startRecordButton,
		_stopRecordButton;

		private Image _speechRecognitionState;

		private Text _speechRecognitionResult;

		private string palavraFalada;

		public Text frase;

		private bool alreadyRight, pontuou;

		private int tentativas, cont;
		private string wordRight, fonema;
		public IDbConnection cn;
		private string sqlQuery;
		private string sqlQuery2;



		private void Start()
		{
			string conn = "URI=file:" + Application.dataPath + "/fonemas.db";
			cn = new SqliteConnection(conn);
			cn.Open(); //Open connection to the database.
			if (cn.State.ToString () == "Open")
				Debug.Log ("open");
			
			tentativas = 3; //tem q ter
			frase.text = "A palavra a pronunciar é: " + spawnController.palavra + "."; //tem q ter

			alreadyRight = false;

			_speechRecognition = SpeechRecognitionModule.Instance;
			_speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
			_speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

			_startRecordButton = transform.Find("Canvas/Button_StartRecord").GetComponent<Button>();
			_stopRecordButton = transform.Find("Canvas/Button_StopRecord").GetComponent<Button>();
			//_startRuntimeDetection = transform.Find("Canvas/Button_StartRuntimeDetection").GetComponent<Button>();
			//_stopRuntimeDetection = transform.Find("Canvas/Button_StopRuntimeDetection").GetComponent<Button>();

			_speechRecognitionState = transform.Find("Canvas/Image_RecordState").GetComponent<Image>();

			_speechRecognitionResult = transform.Find("Canvas/Text_Result").GetComponent<Text>();

			//_isRuntimeDetectionToggle = transform.Find("Canvas/Toggle_IsRuntime").GetComponent<Toggle>();

			//_languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();

			//_contextPhrases = transform.Find("Canvas/InputField_SpeechContext").GetComponent<InputField>();

			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
			_stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);
			//_startRuntimeDetection.onClick.AddListener(StartRuntimeDetectionButtonOnClickHandler);
			//_stopRuntimeDetection.onClick.AddListener(StopRuntimeDetectionButtonOnClickHandler);
			//_isRuntimeDetectionToggle.onValueChanged.AddListener(IsRuntimeDetectionOnValueChangedHandler);

			_speechRecognitionState.color = Color.white;
			_startRecordButton.interactable = true;
			_stopRecordButton.interactable = false;
		

		}

		private void OnDestroy()
		{
			_speechRecognition.SpeechRecognizedSuccessEvent -= SpeechRecognizedSuccessEventHandler;
			_speechRecognition.SpeechRecognizedFailedEvent -= SpeechRecognizedFailedEventHandler;
		}



		private void StartRecordButtonOnClickHandler()
		{
			//tentativas--;
			_startRecordButton.interactable = false;
			_stopRecordButton.interactable = true;
			_speechRecognitionState.color = Color.red;
			_speechRecognitionResult.text = "";
			_speechRecognition.StartRecord();
		}

		private void StopRecordButtonOnClickHandler()
		{
			//ApplySpeechContextPhrases();

			_stopRecordButton.interactable = false;
			_speechRecognitionState.color = Color.yellow;
			_speechRecognition.StopRecord();
		}


		private void SpeechRecognizedFailedEventHandler(string obj)
		{
			_speechRecognitionState.color = Color.green;
			_speechRecognitionResult.text = "Speech Recognition failed with error: " + obj;

			_startRecordButton.interactable = true;
			_stopRecordButton.interactable = false;
		}

		private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj)
		{
			_startRecordButton.interactable = true;

			_speechRecognitionState.color = Color.green;

			if (obj != null && obj.results.Length > 0)
			{
				tentativas--;
				_speechRecognitionResult.text = "Speech Recognition succeeded! Detected Most useful: " + obj.results[0].alternatives[0].transcript;
				palavraFalada = obj.results [0].alternatives [0].transcript;
				descobrirFonemas (); //descobrir os fonemas da palavra sendo pronunciada
				testarPronuncia ();
			}
			else
			{
				_speechRecognitionResult.text = "Speech Recognition succeeded! Words are no detected.";
				//tentativas++;

			}
		}

		private void testarPronuncia()
		{
			
			if (palavraFalada.Equals(spawnController.palavra, StringComparison.InvariantCultureIgnoreCase) && alreadyRight == false && tentativas != 0) 
			{
				_startRecordButton.interactable = false;
				frase.text = "Sua pronúncia está CERTA.";
				alreadyRight = true; //para ele n pontuar 2x
				cont++;
				pontuou = true;
				descobrirFonemas ();
				//pontuarFonemas();
				playerController.pontuacaoAloha =  spawnController.corPalavra;
				spawnController.corPalavra = 0; //zerando novamente
			} 
			else if (palavraFalada != spawnController.palavra && tentativas == 2 ) 
			{
				frase.text = "Sua pronúncia está ERRADA. A palavra é " + spawnController.palavra + ".Você tem mais 2 tentativas.";
				//aumentar o núm de tentativas dos fonemas da palavra em questão
				cont++;
				descobrirFonemas ();
			}
			else if (palavraFalada != spawnController.palavra &&  tentativas == 1) 
			{
				frase.text = "Sua pronúncia está ERRADA. A palavra é " + spawnController.palavra + ".Você tem mais 1 tentativa.";

			}
			else if (palavraFalada != spawnController.palavra && tentativas == 0) 
			{

				frase.text = "Sua pronúncia está ERRADA. A palavra é " + spawnController.palavra + ".Você não tem mais tentativas.";
				_startRecordButton.interactable = false;

			}



		}

		public void voltar ()
		{
			if (tentativas == 0) {
				playerController.died = true;
				PlayerPrefs.SetInt ("pontuacao", playerController.pontuacao);
				if (playerController.pontuacao > PlayerPrefs.GetInt ("recorde"))
					PlayerPrefs.SetInt ("recorde", playerController.pontuacao);

				SceneManager.LoadScene ("gameover");
			} else {
				SceneManager.LoadScene ("jogo");
			}
		}

		public void descobrirFonemas () 
		{
			if (cont == 1) { // p entrar só uma vez nas 3 tentativas
				wordRight = spawnController.palavra;
				IDbCommand dbcmd = cn.CreateCommand ();
				sqlQuery = "select fonema from palavras_fonemas pf where pf.palavra = '" + wordRight + "'";
				Debug.Log ("sql porcEvolucao: " + sqlQuery);
				dbcmd.CommandText = sqlQuery;
				DataTable dt = new DataTable ();
				dt.Load (dbcmd.ExecuteReader ());
				foreach (DataRow row in dt.Rows) {
					Debug.Log ("entrou no foreach");
					fonema = row [0].ToString ();
					Debug.Log ("fonema=" + fonema);

					IDbCommand dbcmd2 = cn.CreateCommand ();
					sqlQuery2 = "UPDATE paciente_fonema SET tentativas = tentativas + 1  where id_fonemas = '" + fonema + "' AND id_paciente = '" + login.charEmail + "'";
					Debug.Log (sqlQuery2);
					dbcmd2.CommandText = sqlQuery2;
					dbcmd2.ExecuteNonQuery ();

					if (pontuou) {

						IDbCommand dbcmd3 = cn.CreateCommand ();
						sqlQuery2 = "UPDATE paciente_fonema SET acertos = acertos + 1  where id_fonemas = '"+ fonema + "' AND id_paciente = '" + login.charEmail +"'";
						Debug.Log ("sql pontuar fonemas: " + sqlQuery2);
						dbcmd3.CommandText = sqlQuery2;
						dbcmd3.ExecuteNonQuery ();
						Debug.Log ("Modificado");
					}
				}
			}
			
		}

		public void pontuarFonemas()
		{
			
			IDbCommand dbcmd = cn.CreateCommand ();
			sqlQuery2 = "UPDATE paciente_fonema SET acertos = acertos + 1  where id_fonemas = '"+ fonema + "' AND id_paciente = '" + login.charEmail +"'";
			Debug.Log ("sql pontuar fonemas: " + sqlQuery2);
			dbcmd.CommandText = sqlQuery2;
			dbcmd.ExecuteNonQuery ();
			Debug.Log ("Modificado");

		}

	




	}
}
*/