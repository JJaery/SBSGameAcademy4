using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TicTakToe : MonoBehaviour
{
    /// <summary>
    /// 얘를 껏다 켜줫다 해야되기 때문에 받아옴
    /// </summary>
    public GameObject gameResultPanel;

    /// <summary>
    /// 누가 우승했는지 텍스트를 수정해야되기 때문에 받아옴
    /// </summary>
    public Text gameResultVictoryText;


    private bool isTurnO = true;
    private TicTakToeCell[,] cells = new TicTakToeCell[3, 3];

    private void Start()
    {
        gameResultPanel.SetActive(false);
    }

    public void OnClickCell(TicTakToeCell cell)
    {
        if(cell.currentState != TicTakToeCell.eState.None)
        {
            return;
        }

        if (isTurnO == true)
        {
            cell.SetState(TicTakToeCell.eState.O);
        }
        else // false
        {
            cell.SetState(TicTakToeCell.eState.X);
        }

        cells[cell.x, cell.y] = cell;

        CheckResult(cell);

        isTurnO = !isTurnO;
    }

    /// <summary>
    /// 게임결과를 확인하는 메소드
    /// </summary>
    private void CheckResult(TicTakToeCell cell)
    {
        //연달아서 3개가 같은 (none이 아닌) state인지 체크해야됨.
        //그러기 위해서 필요한 데이터 ? 셀들의 위치 값, 몇번째 칸에 있는지.

        int sameCount = 0;

        for (int x = -2; x <= 2; x++) // 좌우 라인 검사
        {
            if (cell.x + x < 0 || cell.x + x > 2 || cells[cell.x + x, cell.y] == null)
                continue;

            if (cells[cell.x + x, cell.y].currentState == cell.currentState)
            {
                sameCount += 1;
            }
        }

        if (sameCount >= 3)
        {
            GameOver();
            return;
        }

        sameCount = 0;
        for (int y = -2; y <= 2; y++) // 상하 라인 검사
        {
            if (cell.y + y < 0 || cell.y + y > 2 || cells[cell.x, cell.y + y] == null)
                continue;


            if (cells[cell.x, cell.y + y].currentState == cell.currentState)
            {
                sameCount += 1;
            }
        }

        if (sameCount >= 3)
        {
            GameOver();
            return;
        }
    }

    private void GameOver()
    {
        gameResultVictoryText.text = string.Format(gameResultVictoryText.text, isTurnO == true ? "O" : "X");

        //위 삼항연산자랑 같은 표현
        if(isTurnO == true)
        {
            gameResultVictoryText.text = string.Format(gameResultVictoryText.text, "O");
        }
        else
        {
            gameResultVictoryText.text = string.Format(gameResultVictoryText.text, "X");
        }

        gameResultPanel.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene("Scenes/2SelectGame");
    }
}
