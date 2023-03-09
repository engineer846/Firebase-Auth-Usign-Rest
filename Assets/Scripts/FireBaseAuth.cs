using Proyecto26;
using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



[Serializable]
public class SignUpResponse
{
    public string localId;
    public string idToken;
    public string UserName;
    public string PSWD;
}

[Serializable]
public class PlayerData
{
    public string GameName;
}

public class FireBaseAuth : MonoBehaviour
{
    public string Account;
    //Authentication
    private string AuthKey = "AIzaSyChjHW2u-51qeffbVxioChtif6U98bEv5g";
    public string localId;
    public string idToken;
    public static FireBaseAuth Instance;

    public PlayerData Data;
    private SignUpResponse signUpResponse;
    private static readonly string firebaseLink = "https://authentication-34a12-default-rtdb.firebaseio.com/";

    public delegate void PostItemCallback();
    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerDataInFirebase(string GameName)
    {
        Data.GameName = GameName;

        PostToDatabase(Data, false);
    }

    public void PostToDatabase(PlayerData playerData, bool signingUp = false)
    {
        if (signingUp)
            RestClient.Put<SignUpResponse>($"{firebaseLink}" + "/" + localId /*+ "/" + Account */+ "/" + "Credentials" + ".json?auth=" + idToken, signUpResponse).Then(response =>
            {
                if (Setup.instance != null)
                    Setup.instance.LoggedInSuccess();
            });
        else
        {
            RestClient.Put<PlayerData>($"{firebaseLink}" + "/" + localId + ".json?auth=" + idToken, playerData).Then(response =>
            {
                callback();
            });
        }
    }

    private void callback()
    {
    }

    #region Firebase Auth
    public void SignUpUserButton(string ownerName, string password)
    {
        Debug.Log("Sign up Account: " + Account);
        SignUpUser(Account + "@auth.com", ownerName, password);
    }

    public void SignInUserButton(string email, string ownerName, string password)
    {
        Debug.Log("Account: " + email + " Password: " + password);
        SignInUser(Account + "@auth.com", password);
    }
    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignUpResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                response.PSWD = PlayerPrefs.GetString("RealPassword");
                response.UserName = username;
                //PlayerData pd = new PlayerData();
                //signUpResponse = response;
                //Debug.Log("Signed up Account: " + localId);
                //PostToDatabase(pd, true);

            }).Catch(error =>
            {
                SignInUserButton(email, username, password);
                Debug.Log("Sign in Account called: " + localId);
                Debug.Log(error);
            });
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignUpResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                if (Setup.instance != null)
                    Setup.instance.LoggedInSuccess();
                Debug.Log("Signed in Account called: " + localId);
                //GetUsername();
            }).Catch(error =>
            {
                //Failed to login
                Setup.instance.LogInFailed();
                Debug.Log(error);
            });
    }

    public void GetUsername()
    {
        RestClient.Get<SignUpResponse>($"{firebaseLink}" + "/" + localId /*+ "/" + Account */+ "/" + "Credentials" + ".json?auth=" + idToken).Then(response =>
        {
            //Setup.instance.mainMenu.UserNameText.text = response.UserName;
            //OwnerProperties.instance.mainMenu.PasswordText.text = response.PSWD;
            //OwnerProperties.instance.mainMenu.PasswordRetrieveScreen.SetActive(true);
        });
    }

    #endregion
}
