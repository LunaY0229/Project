using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    public int sceneIndex = 0;
    public void Restart()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
