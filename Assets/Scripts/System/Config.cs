using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public const string ROW_NAME = "Row";
    public const string ROWSHOOT_NAME = "Shoot";

    public static int Row_Index = 0;

    public static int GetRowIndex()
    {
        return Row_Index++ % int.MaxValue;
    }
}
