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
        this.userTotalCount.text = "�ܳ���\n" + userTotalCount;
        this.userWinCount.text = "ʤ���\n" + userWinCount;

        SetReady(isReady);
    }

    public void SetReady(bool isReady)
    {
        this.isReady = isReady;

        if (isReady)
        {
            readyText.text = "׼������";
            readyText.color = Color.green;
        }
        else
        {
            readyText.text = "δ׼��";
            readyText.color = Color.red;
        }
    }
}
