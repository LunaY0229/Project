using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomGridItem : MonoBehaviour
{
    public Text userName;
    public Text winCount;
    public Text totalCount;
    public Button joinBtn;
    private void Awake()
    {

    }

    public void SetRoomGrid(string username,string wincount,string totalcount, Action buttonAction)
    {
        userName.text = username;
        winCount.text = "ʤ������\n" + wincount;
        totalCount.text = "�ܳ���\n" + totalcount;
        joinBtn.onClick.AddListener(() => { buttonAction?.Invoke(); });
    }
}
