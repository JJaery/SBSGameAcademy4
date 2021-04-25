using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectGame : MonoBehaviour
{
    //버튼 클릭 이벤트들...
    public void OnClickOthello()
    {
        SceneManager.LoadScene("Scenes/3Game-Othello");
    }

    public void OnClickTicTacToe()
    {
        SceneManager.LoadScene("Scenes/4Game-TicTakToe");
    }

    public void OnClickBaduk()
    {
        SceneManager.LoadScene("Scenes/5Game-Baduk");
    }
}
