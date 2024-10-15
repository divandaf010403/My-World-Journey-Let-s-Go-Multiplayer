using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;
    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomNameText.text = _info.Name;
    }

    public void onClick()
    {
        Launcher.instance.JoinRoom(info);
    }
}
