using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Othello : MonoBehaviour
{
    //������ �������� ����� ���� ����?
    //1.������
    public GameObject cellPrefab;

    //2. �������� ���� ���� ��ġ
    public Transform cellStart;

    /// <summary>
    /// ���� ���� ��� ���ΰ�?
    /// </summary>
    private bool isTurnWhite = true;

    private OthelloCell[,] cells = new OthelloCell[8, 8];

    private void Start()
    {
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                //x:0, y:0 -> x:0, y:1 .... x:8 , y:8 
                GameObject obj = Instantiate(cellPrefab, cellStart);
                // obj.transform.position = cellStart.position; -> localPosition �� 0���� �����.
                // 36 : ���� ũ��
                // 2 : ���� �� ���� ������ ũ��
                obj.transform.localPosition = new Vector3(x * (36 + 2), -y * (36 + 2), 0);

                OthelloCell script = obj.GetComponent<OthelloCell>();
                script.x = x;
                script.y = y;
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

        StartTurn(isTurnWhite);
    }


    private void StartTurn(bool isWhite)
    {
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
                    //���� �˻�
                    if (y + 1 < 8 && cells[x, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, +1, enemyState);
                    }
                    //�Ʒ��� �˻�
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




    public void Back()
    {
        SceneManager.LoadScene("Scenes/2SelectGame");
    }
}
