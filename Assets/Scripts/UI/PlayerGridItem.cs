using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGridItem : MonoBehaviour
{
    public Text userId;
    public Text userTotalCount;
    public Text userWinCount;
    public Text readyText;

    private bool isReady;

    public void SetUser(string userId, string userTotalCount,string userWinCount,bool isReady)
    {
        this.userId.text = userId;
        this.userTotalCount.text = "总场次\n" + userTotalCount;
        this.userWinCount.text = "胜李场次\n" + userWinCount;

        SetReady(isReady);
    }

    public void SetReady(bool isReady)
    {
        this.isReady = isReady;

        if (isReady)
        {
            readyText.text = "准备就绪";
            readyText.color = Color.green;
        }
        else
        {
            readyText.text = "未准备";
            readyText.color = Color.red;
        }
    }
}
