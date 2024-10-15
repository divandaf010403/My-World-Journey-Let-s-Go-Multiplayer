using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class UsernameDisplay : MonoBehaviour
{
    public TMP_Text usernameText;

    public PhotonView view;

    private void Start() {
        if(view.IsMine)
        {
            // Don't want to display username
            gameObject.SetActive(false);
        }

        usernameText.text = view.Owner.NickName;
    }
}
