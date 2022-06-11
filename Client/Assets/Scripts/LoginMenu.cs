using Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Login menu.
/// </summary>
public class LoginMenu : MonoBehaviour
{
    private TMP_InputField nicknameField;
    private TMP_Text errorText;

    void Start()
    {
        nicknameField = GetComponentInChildren<TMP_InputField>();
        errorText = transform.Find("ErrorText").GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Send registration request and set active game scene.
    /// </summary>
    public async void OnRegisterButtonClick()
    {
        var nickname = nicknameField.text;
        var response = await HttpProvider.Instance.RegisterPlayer(nickname);
        if (response.Type == RegistrationResponse.ResponseType.Error) {
            errorText.text = response.ErrorMessage;
        } else if (response.Type == RegistrationResponse.ResponseType.Success) {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Send login request and set active game scene.
    /// </summary>
    public async void OnLoginButtonClick()
    {
        var nickname = nicknameField.text;
        var response = await HttpProvider.Instance.LoginPlayer(nickname);
        if (response.Type == LoginResponse.ResponseType.Error) {
            errorText.text = response.ErrorMessage;
        } else if (response.Type == LoginResponse.ResponseType.Succes) {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
