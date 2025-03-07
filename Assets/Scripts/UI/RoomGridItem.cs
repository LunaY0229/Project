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
        winCount.text = "胜利场数\n" + wincount;
        totalCount.text = "总场数\n" + totalcount;
        joinBtn.onClick.AddListener(() => { buttonAction?.Invoke(); });
    }
}
