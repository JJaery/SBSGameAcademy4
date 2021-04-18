using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool isInit = false;
    public static bool isGameStart = false;
    public static int score = 0;


    private void Start()
    {
        score = 0;
        isGameStart = false;

        if (isInit == false)
        {
            DontDestroyOnLoad(this.gameObject);
            isInit = true;
        }
        else // isInit == true
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Scenes/Game");
        isGameStart = true;
    }
}
