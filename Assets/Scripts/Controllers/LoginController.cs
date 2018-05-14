using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour {

    public InputField lEmail;
    public InputField lPassword;
    public Button     lLoginButton;
    public Text       lMessage;

    private void Start() {
        lLoginButton.onClick.AddListener(doLogin);
    }

    private void doLogin() {
        //Check if the inputs are empty
        if(lEmail.text == "" || lPassword.text == "") {
            lMessage.transform.gameObject.SetActive(true);
            lMessage.text = "Os campos devem ser preenchidos!";
        }
        else {
            //Connect with database to check the login
            DatabaseController dbc = new DatabaseController(DatabaseController.DB_URL);
            int result = (dbc.RunQuery("SELECT * FROM PACIENTE WHERE PACEMAIL = '" + lEmail.text + "' AND PACSENHA = '" + lPassword.text + "';")).Rows.Count;

            Debug.Log(result + " = resultado query");
            Debug.Log("User: " + lEmail.text);
            Debug.Log("Pass: " + lPassword.text);

            //Patient founded
            if(result == 1) {
                SceneManager.LoadScene("PatientMenu");
            }
            else {
                lMessage.text = "Erro! Usuário ou Senha incorretos";
            }
        }
    }
}
