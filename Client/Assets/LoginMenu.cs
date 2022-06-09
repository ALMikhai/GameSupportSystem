using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoginMenu : MonoBehaviour
{
    private TMP_InputField nicknameField;
    private TMP_Text errorText;

    void Start()
    {
        nicknameField = GetComponentInChildren<TMP_InputField>();
        errorText = transform.Find("ErrorText").GetComponent<TMP_Text>();
    }

    public async void OnRegisterButtonClick()
    {
        var nickname = nicknameField.text;
        var response = await HttpProvider.Instance.RegisterPlayer(nickname);
        if (response.Type == RegistrationResponse.ResponseType.Error) {
            errorText.text = response.ErrorMessage;
        } else if (response.Type == RegistrationResponse.ResponseType.Succes) {
            SceneManager.LoadScene("ChatScene", LoadSceneMode.Additive);
        }
    }

    public async void OnLoginButtonClick()
    {
        var nickname = nicknameField.text;
        var response = await HttpProvider.Instance.LoginPlayer(nickname);
        if (response.Type == LoginResponse.ResponseType.Error) {
            errorText.text = response.ErrorMessage;
        } else if (response.Type == LoginResponse.ResponseType.Succes) {
            SceneManager.LoadScene("ChatScene", LoadSceneMode.Additive);
        }
    }
}
