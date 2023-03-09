using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    public InputField OwnerName, OwnerPassword, GameName;
    public GameObject LogInFailureScreen, LogInSuccessScreen;

    public static Setup instance;
    private void Awake()
    {
        instance = this;
    }

    public void Continue()
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

    public void SetGameName()
    {
        string name = GameName.textComponent.text;
        if (!(string.IsNullOrEmpty(name)))
        {
            PlayerPrefs.SetString("GameName", this.GameName.textComponent.text.ToString());
            FireBaseAuth.Instance.SetPlayerDataInFirebase(name);
        }
    }


    public void LoginViaFirebase()
    {
        if (FireBaseAuth.Instance != null)
        {
            FireBaseAuth.Instance.SignUpUserButton(PlayerPrefs.GetString("OwnerName"), PlayerPrefs.GetString("RealPassword"));
            Debug.Log("Loaded account retrieved");
        }
    }

    public void LoggedInSuccess()
    {
        //gameObject.SetActive(false);
        LogInSuccessScreen.SetActive(true);
        PlayerPrefs.SetInt("LoggedIn", 1);
    }

    public void LogInFailed()
    {
        gameObject.SetActive(true);
        //PlayerPrefs.SetInt("LoggedIn", 0);
        LogInFailureScreen.SetActive(true);
    }
}
