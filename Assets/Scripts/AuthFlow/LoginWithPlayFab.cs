using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.AddressableAssets;

public class LoginWithPlayFab : MonoBehaviour
{
    public TextMeshProUGUI status;
    public TMP_InputField emailInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public AssetReference sceneRef;

    private void Start() {
        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
            Debug.Log("no connection");
            Application.Quit();
        }
    }

    public void RegisterUser() {
        string email = emailInput.text.Trim();
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();
        if(email.Length < 3) {
            status.text = "Enter a valid email";
            return;
        }
        if (username.Length < 5) {
            status.text = "Username must be at least 5 characters";
            return;
        }
        if (password.Length < 8) {
            status.text = "Password must be at least 8 characters";
            return;
        }
        var request = new RegisterPlayFabUserRequest {
            Email = email,
            Username = username,
            Password = password
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegistered, OnError);
    }

    private void OnError(PlayFabError obj) {
        status.text = obj.ErrorMessage;
        Debug.Log(obj.ToString());
    }

    void OnRegistered(RegisterPlayFabUserResult result) {
        status.text = "Account created!";
        PlayerDataController.instance.SetUsername(usernameInput.text.Trim());
        sceneRef.LoadSceneAsync();
    }

    public void LoginUser() {
        // process username + pw
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (username.Length < 5) {
            status.text = "Invalid username";
            return;
        }
        if (password.Length < 8) {
            status.text = "Invalid password";
            return;
        }
        var request = new LoginWithPlayFabRequest {
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLogin, OnError);
    }

    private void OnLogin(LoginResult obj) {
        status.text = "Logging in...";
        PlayerDataController.instance.SetUsername(usernameInput.text.Trim());
        PlayerDataController.instance.SetData(obj);
        sceneRef.LoadSceneAsync();
    }


    public void ResetPassword() {
        string email = emailInput.text.Trim();
        if (email.Length < 5) { status.text = "Invalid email"; return; }
        var request = new SendAccountRecoveryEmailRequest {
            Email = email,
            TitleId = "3BF88"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult obj) {
        Debug.Log("Password Reset");
    }
}
