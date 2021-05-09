using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Othello : MonoBehaviour
{
    //--��� ����--//
    //������ �������� ����� ���� ����?
    //1.������
    public GameObject cellPrefab;

    //2. �������� ���� ���� ��ġ
    public Transform cellStart;

    //3. ���� ������ �ȳ��ϴ� UI �ؽ�Ʈ
    public Text logger;

    /// <summary>
    /// ���� ���� ��� ���ΰ�?
    /// </summary>
    private bool isTurnWhite = true;

    private OthelloCell[,] cells = new OthelloCell[8, 8];
    //--��� ���� END --//

    private string playerName
    {
        get
        {
            return isTurnWhite == true ? "���" : "������";
        }
    }

    private void Start()
    {
        //---������ �ʱ�ȭ---//
        for (int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                //x:0, y:0 -> x:0, y:1 .... x:8 , y:8 
                GameObject obj = Instantiate(cellPrefab, cellStart);
                // obj.transform.position = cellStart.position; -> localPosition �� 0���� �����.
                // obj.transform.parent = cellStart;

                // 36 : ���� ũ��
                // 2 : ���� �� ���� ������ ũ��
                obj.transform.localPosition = new Vector3(x * (36 + 2), -y * (36 + 2), 0);

                OthelloCell script = obj.GetComponent<OthelloCell>();
                script.x = x;
                script.y = y;
                script.boardScript = this;
                //x:3 y:3 x:3 y:4
                //x:4 y:3 x:4 y:4

                if ((x == 3 && y == 3) || (x == 4 && y == 4))
                {
                    script.SetState(OthelloCell.eState.White);
                }
                else if ((x == 3 && y == 4) || (x == 4 && y == 3))
                {
                    script.SetState(OthelloCell.eState.Black);
                }
                else
                {
                    script.SetState(OthelloCell.eState.None);
                }

                cells[x, y] = script;
            }
        }
        //---������ �ʱ�ȭ END---//

        logger.text = string.Empty; //�� ��Ʈ�� ����

        Log("������ ���۵Ǿ����ϴ�.");

        StartTurn(isTurnWhite);
    }

    private void StartTurn(bool isWhite)
    {
        Log($"\"{playerName}\" ���� ���۵Ǿ����ϴ�!");

        foreach (OthelloCell item in cells)
        {
            //Selectable �ʱ�ȭ
            if (item.curState == OthelloCell.eState.Selectable)
            {
                item.SetState(OthelloCell.eState.None);
            }
        }

        OthelloCell.eState playerState = isWhite == true ? OthelloCell.eState.White : OthelloCell.eState.Black;

        OthelloCell.eState enemyState = playerState == OthelloCell.eState.White ? OthelloCell.eState.Black : OthelloCell.eState.White;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cells[x, y].curState == playerState)
                {
                    //1. �����¿�밢�� black�� �ִ��� �˻�
                    //���� �˻�
                    if (x - 1 >= 0 && cells[x - 1, y].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, 0, enemyState);
                    }
                    //������ �˻�
                    if (x + 1 < 8 && cells[x + 1, y].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, 0, enemyState);
                    }
                    //�Ʒ��� �˻�
                    if (y + 1 < 8 && cells[x, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, +1, enemyState);
                    }
                    //���� �˻�
                    if (y - 1 >= 0 && cells[x, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, -1, enemyState);
                    }
                    //�밢�� ���� �Ʒ� (-1,-1)
                    if (x - 1 >= 0 && y - 1 >= 0 && cells[x - 1, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, -1, enemyState);
                    }
                    //�밢�� ���� �� (-1,+1)
                    if (x - 1 >= 0 && y + 1 < 8 && cells[x - 1, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, +1, enemyState);
                    }
                    //�밢�� ������ �Ʒ�(+1,-1)
                    if (x + 1 < 8 && y - 1 >= 0 && cells[x + 1, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, -1, enemyState);
                    }
                    //�밢�� ������ �� (+1,+1)
                    if (x + 1 < 8 && y + 1 < 8 && cells[x + 1, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, +1, enemyState);
                    }
                }
            }
        }

        int selectableCount = 0;
        int whiteCount = 0;
        int blackCount = 0;
        int noneCount = 0;

        foreach (OthelloCell item in cells)
        {
            if (item.curState == OthelloCell.eState.Selectable)
            {
                selectableCount++;
            }
            if(item.curState == OthelloCell.eState.White)
            {
                whiteCount++;
            }
            if(item.curState == OthelloCell.eState.Black)
            {
                blackCount++;
            }
            if(item.curState == OthelloCell.eState.None)
            {
                noneCount++;
            }
        }

        //if(noneCount == 0 && selectableCount == 0) 
        if(noneCount + selectableCount == 0) // GameOver
        {
            bool isDraw = whiteCount == blackCount;
            if (isDraw == true) // ���º�
            {
                Log("���� ������ �����ϴ�. �����ϴ�!");
            }
            else
            {
                string winnerName = whiteCount > blackCount ? "���" : "������";
                Log($"{winnerName}�� �̰���ϴ�!");
                Log($"Score ��: {whiteCount} ��: {blackCount}");
            }
            return;
        }
        else if (selectableCount == 0)
        {
            //�Ͽ���
            Log($"{playerName}�� �� ���� ���� ������ ���� ����˴ϴ�.");
            isTurnWhite = !isTurnWhite;
            StartTurn(isTurnWhite);
            return;
        }
    }

    /// <summary>
    /// �ش� ���� �������� �������� �ִ���
    /// x : �˻��� ���� �� x
    /// y : �˻��� ���� �� y
    /// offset : ����
    /// targetState : �˻��� ����
    /// </summary>
    private void CheckStraight(int x, int y, int offsetX, int offsetY, OthelloCell.eState targetState)
    {
        //x,y�� �������� offsetX,offsetY��ŭ ������Ű�鼭
        //state�� none�� �ָ� ã�°̴ϴ�.
        //none�� �ƴϰ� targetState�� �ƴϸ� �����ֱ� ������(�� ���� �ִ�) ����

        //x,y�� ���� ���� �� �����̱� ������ ���� �� ���� �˻��Ϸ��� ���� offset�� ����
        x += offsetX;
        y += offsetY;

        //x,y�� �������� offsetX,offsetY��ŭ ������Ű�鼭
        for (; (x >= 0 && x < 8) && (y >= 0 && y < 8); x += offsetX, y += offsetY)
        {
            //state�� none�� �ָ� ã�°̴ϴ�.
            if(cells[x,y].curState == OthelloCell.eState.None)
            {
                cells[x, y].SetState(OthelloCell.eState.Selectable);
                break;
            }
            //none�� �ƴϰ� targetState�� �ƴϸ� �����ֱ� ������(�� ���� �ִ�) ����
            else if (cells[x,y].curState != targetState)
            {
                break;
            }
            else if (cells[x, y].curState == targetState) // ���� ����
            {
                continue;
            }
        }
    }

    public void OnClickCell(OthelloCell targetScript)
    {
        OthelloCell.eState playerState = isTurnWhite == true ? OthelloCell.eState.White : OthelloCell.eState.Black;
        OthelloCell.eState enemyState = playerState == OthelloCell.eState.White ? OthelloCell.eState.Black : OthelloCell.eState.White;

        if(targetScript.curState == OthelloCell.eState.None)
        {
            Log("�� �� ���� ���Դϴ�.");
        }

        //1. Selectable ���̸�, �ڱ� �ڽ��� ���� �����ϱ�
        if (targetScript.curState == OthelloCell.eState.Selectable)
        {
            targetScript.SetState(playerState);

            //2.��ġ�� ���� �ٸ� �� �� ���̿� �� ���� �ִ����� üũ
            //8�� �������� �޿� �˻縦 �ϸ� �˴ϴ�.

            int curX = targetScript.x;
            int curY = targetScript.y;
            int getCount = 0;

            //����
            if (curX - 1 >= 0 && cells[curX - 1, curY].curState == enemyState)
            {
                // ������ ����ġ�� ���� �޿� �˻�,
                CheckStraightV2(curX, curY, -1, 0, playerState, enemyState, ref getCount);
            }

            //������
            if (curX + 1 < 8 && cells[curX + 1, curY].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, 0, playerState, enemyState, ref getCount);
            }

            //��
            if (curY - 1 >= 0 && cells[curX, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, 0, -1, playerState, enemyState, ref getCount);
            }

            //�Ʒ�
            if (curY + 1 < 8 && cells[curX, curY + 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, 0, +1, playerState, enemyState, ref getCount);
            }

            //�밢�� ���� �Ʒ� (-1,-1)
            if (curX - 1 >= 0 && curY - 1 >= 0 && cells[curX - 1, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, -1, -1, playerState, enemyState, ref getCount);
            }

            //�밢�� ���� �� (-1,+1)
            if (curX - 1 >= 0 && curY + 1 < 8 && cells[curX - 1, curY+ 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, -1, +1, playerState, enemyState, ref getCount);
            }

            //�밢�� ������ �Ʒ� (+1,-1)
            if (curX + 1 < 8 && curY - 1 >= 0 && cells[curX + 1, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, -1, playerState, enemyState, ref getCount);
            }

            //�밢�� ������ �� (+1,+1)
            if (curX + 1 < 8 && curY + 1 < 8 && cells[curX + 1, curY + 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, +1, playerState, enemyState, ref getCount);
            }


            Log($"{playerName}�� {getCount}�� ȹ���߽��ϴ�.");

            //3.�� ����
            isTurnWhite = !isTurnWhite;
            StartTurn(isTurnWhite);
        }
    }

    private void CheckStraightV2(int x, int y, int offsetX, int offsetY
        , OthelloCell.eState playerState, OthelloCell.eState enemyState, ref int count)
    {
        List<OthelloCell> enemiesCell = new List<OthelloCell>();
        x += offsetX;
        y += offsetY;

        for (; (x >= 0 && x < 8) && (y >= 0 && y < 8); x += offsetX, y += offsetY)
        {
            if (cells[x, y].curState == enemyState)
            {
                enemiesCell.Add(cells[x, y]);
            }
            else if (cells[x, y].curState == playerState)
            {
                foreach (OthelloCell item in enemiesCell)
                {
                    item.SetState(playerState);
                }
                count += enemiesCell.Count;
                break;
            }
            else // Selectable �̰ų� None�� ��찡 ���⿡ ���ϴ�.
            {
                break;
            }
        }
    }

    private void Log(string something)
    {
        //logger.text += "\r\n";
        logger.text += System.Environment.NewLine; // OS�� ���� ���๮�ڰ� �ٸ�����, System.Environment.NewLine�� OS�� ���缭 �޶���
        logger.text += something;
        int lineCount = logger.text.Count(t => t == '\n');

        if(lineCount > 20)
        {
            int firstLineIndex = logger.text.IndexOf('\n');
            logger.text = logger.text.Substring(firstLineIndex + 1);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Scenes/2SelectGame");
    }
}
