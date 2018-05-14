using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour {

    [Header("Default register scene")]
    public InputField rName;
    public InputField rEmail;
    public InputField rPassword;
    public Toggle     rIsLiteracy;
    public Text       rMessage;
    public Button     rRegisterButton;

    public void Start() {
        rRegisterButton.onClick.AddListener(doRegister);
    }

    public void doRegister() {
        if(rName.text == "" || rEmail.text == "" || rPassword.text == "") {
            rMessage.text = "Todos campos devem ser preenchidos!";
            return;
        }

        DatabaseController dbc = new DatabaseController(DatabaseController.DB_URL);

        //Check if has another account with the same email
        int r = dbc.RunQuery("SELECT PACID FROM PACIENTE WHERE PACEMAIL = '" + rEmail.text + "';").Rows.Count;
        if(r != 0) {
            rMessage.text = "Erro! Email já cadastrado.";
            return;
        }

        //Do the register
        string alf;
        if (rIsLiteracy.isOn) alf = "1";
        else                  alf = "0";

        dbc.RunQueryWithoutReturn("INSERT INTO PACIENTE (PACNOME, PACEMAIL, PACSENHA, PACALFABETIZADA) VALUES ('" + rName.text + "', '" + rEmail.text + "', '" + rPassword.text + "', " + alf + ");");

        SceneManager.LoadScene("Login");
    }
}
