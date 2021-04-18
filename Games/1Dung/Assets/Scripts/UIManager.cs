using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<UIManager>();
            return _Instance;
        }
    }
    private static UIManager _Instance;

    public Text scoreLabel;
    public GameObject gameOverUI;


    public void ShowGameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
        GameManager.isGameStart = false;
    }

    public void OnClickRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
