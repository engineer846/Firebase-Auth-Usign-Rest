using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    public InputField OwnerName, OwnerPassword, GameName;
    public InputField SignUpUserName, SignUpMobileNumber, SignUpPassword, SignUpConfirmPassword;
    public GameObject LogInFailureScreen, LogInSuccessScreen, SignUpButton, RegisterButton, SignUpScreen;
    public GameObject Alert;

    public static Setup instance;
    private void Awake()
    {
        instance = this;
        SignUpButton.SetActive(false);
    }

    public void SignIn()
    {
        string name = OwnerName.textComponent.text;
        FireBaseAuth.Instance.Account = name;
        string Password = OwnerPassword.textComponent.text;
        if (!(string.IsNullOrEmpty(name)) && !(string.IsNullOrEmpty(Password)))
        {
            PlayerPrefs.SetString("OwnerName", this.OwnerName.textComponent.text.ToString());
            PlayerPrefs.SetString("RealPassword", this.OwnerPassword.text);
            PlayerPrefs.SetString("OwnerPassword", this.OwnerPassword.textComponent.text.ToString());
            Debug.Log("Password: " + OwnerPassword.text);
            LoginViaFirebase();
        }
    }

    public void CheckPassword()
    {
        string name = SignUpUserName.textComponent.text;
        FireBaseAuth.Instance.Account = name;
        string Password = SignUpPassword.textComponent.text;
        string ConfirmPassord = SignUpConfirmPassword.textComponent.text;
        if (!(string.IsNullOrEmpty(Password)) && !(string.IsNullOrEmpty(ConfirmPassord)) && Password == ConfirmPassord)
        {
            RegisterButton.SetActive(true);
        }
    }

    public void Regiser()
    {
        string name = SignUpUserName.textComponent.text;
        FireBaseAuth.Instance.Account = name;
        string Password = SignUpPassword.textComponent.text;
        string ConfirmPassord = SignUpConfirmPassword.textComponent.text;
        if (!(string.IsNullOrEmpty(Password)) && !(string.IsNullOrEmpty(ConfirmPassord)) && Password == ConfirmPassord)
        {
            PlayerPrefs.SetString("OwnerName", this.SignUpUserName.textComponent.text.ToString());
            PlayerPrefs.SetString("RealPassword", this.SignUpPassword.text);
            PlayerPrefs.SetString("OwnerPassword", this.SignUpPassword.textComponent.text.ToString());
            Debug.Log("Password: " + OwnerPassword.text);
            SignUpToFirebase();
        }
    }

    public void SetGameName()
    {
        string name = GameName.textComponent.text;
        if (!(string.IsNullOrEmpty(name)))
        {
            PlayerPrefs.SetString("GameName", this.GameName.textComponent.text.ToString());
            FireBaseAuth.Instance.SetPlayerDataInFirebase(name);
        }
    }

    public void SignUpToFirebase()
    {
        if (FireBaseAuth.Instance != null)
        {
            FireBaseAuth.Instance.SignUpUserButton(PlayerPrefs.GetString("OwnerName"), PlayerPrefs.GetString("RealPassword"));
            Debug.Log("Loaded account retrieved");
        }
    }

    public void LoginViaFirebase()
    {
        if (FireBaseAuth.Instance != null)
        {
            FireBaseAuth.Instance.SignInUserButton(PlayerPrefs.GetString("OwnerName"), PlayerPrefs.GetString("RealPassword"));
            Debug.Log("Loaded account retrieved");
        }
    }

    public void LoggedInSuccess()
    {
        //gameObject.SetActive(false);
        SignUpScreen.SetActive(false);
        SignUpButton.SetActive(false);
        RegisterButton.SetActive(false);
        LogInSuccessScreen.SetActive(true);
        PlayerPrefs.SetInt("LoggedIn", 1);
    }

    public void LogInFailed()
    {
        //PlayerPrefs.SetInt("LoggedIn", 0);
        SignUpButton.SetActive(true);
        LogInFailureScreen.SetActive(true);
    }
}
