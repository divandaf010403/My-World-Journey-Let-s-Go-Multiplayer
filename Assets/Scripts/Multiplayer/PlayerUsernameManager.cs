using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerUsernameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text errorMessageText;

    private void Start() 
    {
        if(PlayerPrefs.HasKey("username"))
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
    }

    public void PlayerUsernameInputValueChanged()
    {
        string username = usernameInput.text;

        if(!string.IsNullOrEmpty(username) && username.Length <= 20)
        {
            PhotonNetwork.NickName = username;
            PlayerPrefs.SetString("username", username);
            errorMessageText.text = "";
            MenuManager.instance.OpenMenu("TitleMenu");
        }
        else
        {
            errorMessageText.text = "Username must not be empty and should be 20 character or less";
        }
    }
}
