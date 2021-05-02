using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TicTakToe : MonoBehaviour
{
    /// <summary>
    /// �긦 ���� �ѢZ�� �ؾߵǱ� ������ �޾ƿ�
    /// </summary>
    public GameObject gameResultPanel;

    /// <summary>
    /// ���� ����ߴ��� �ؽ�Ʈ�� �����ؾߵǱ� ������ �޾ƿ�
    /// </summary>
    public Text gameResultVictoryText;

    /// <summary>
    /// ���� ���� 'O'���ΰ�?
    /// </summary>
    private bool isTurnO = true;

    /// <summary>
    /// �� - ������ (3x3 �迭)
    /// </summary>
    private TicTakToeCell[,] cells = new TicTakToeCell[3,3];

    private void Start()
    {
        gameResultPanel.SetActive(false); //�Ǽ�����
    }

    public void OnClickCell(TicTakToeCell cell)
    {
        if(cell.currentState != TicTakToeCell.eState.None) // X�� O�� ���,
        {
            return; // �޼ҵ� ����
        }

        if (isTurnO == true) //���� O�ΰ�?
        {
            cell.SetState(TicTakToeCell.eState.O);
        }
        else // false, X���̴�.
        {
            cell.SetState(TicTakToeCell.eState.X);
        }

        cells[cell.x, cell.y] = cell;

        CheckResult(cell);

        isTurnO = !isTurnO; // bool�� ���� (true -> false, false -> true)
    }

    /// <summary>
    /// ���Ӱ���� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    private void CheckResult(TicTakToeCell cell)
    {
        if (cell.currentState == TicTakToeCell.eState.None)
            return;

        //���޾Ƽ� 3���� ���� (none�� �ƴ�) state���� üũ�ؾߵ�.
        //�׷��� ���ؼ� �ʿ��� ������ ? ������ ��ġ ��, ���° ĭ�� �ִ���.
        int sameCount = 0;

        for (int x = -2; x <= 2; x++) // �¿� ���� �˻�
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
        for (int y = -2; y <= 2; y++) // ���� ���� �˻�
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

        sameCount = 0;
        //�������� �밢�� �˻� ����
        for (int x = -2, y = -2; x <= 2 && y <= 2 ; x++, y++) 
        {
            //x -2 y -2
            //x -1 y -1
            // ....
            //x 1 y 1
            //x 2 y 2

            if (cell.x + x < 0 || cell.x + x > 2 ||
                cell.y + y < 0 || cell.y + y > 2 || cells[cell.x + x, cell.y + y] == null)
                continue;

            if (cells[cell.x + x, cell.y + y].currentState == cell.currentState)
            {
                sameCount += 1;
            }
        }

        if(sameCount >= 3)
        {
            GameOver();
            return;
        }


        sameCount = 0;
        //�ö󰡴� �밢�� �˻� ����
        for (int x = -2, y = 2; x <= 2 && y >= -2; x++, y--)
        {
            //x -2 y 2
            //x -1 y 1
            // ....
            //x 1 y -1
            //x 2 y -2
            if (cell.x + x < 0 || cell.x + x > 2 ||
                cell.y + y < 0 || cell.y + y > 2 || cells[cell.x + x, cell.y + y] == null)
                continue;

            if (cells[cell.x + x, cell.y + y].currentState == cell.currentState)
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
        
        //�� ���׿����ڶ� ���� ǥ��
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
