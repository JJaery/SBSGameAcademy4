using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Othello : MonoBehaviour
{
    //오셀로 보드판을 만들기 위한 조건?
    //1.프리팹
    public GameObject cellPrefab;

    //2. 프리팹을 만들 시작 위치
    public Transform cellStart;

    /// <summary>
    /// 지금 현재 백색 턴인가?
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
                // obj.transform.position = cellStart.position; -> localPosition 을 0으로 맞춘다.
                // 36 : 셀의 크기
                // 2 : 셀과 셀 사이 여백의 크기
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
                    //1. 상하좌우대각선 black이 있는지 검사
                    //왼쪽 검사
                    if (x - 1 >= 0 && cells[x - 1, y].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, 0, enemyState);
                    }
                    //오른쪽 검사
                    if (x + 1 < 8 && cells[x + 1, y].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, 0, enemyState);
                    }
                    //위쪽 검사
                    if (y + 1 < 8 && cells[x, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, +1, enemyState);
                    }
                    //아래쪽 검사
                    if (y - 1 >= 0 && cells[x, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, -1, enemyState);
                    }
                    //대각선 왼쪽 아래 (-1,-1)
                    if (x - 1 >= 0 && y - 1 >= 0 && cells[x - 1, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, -1, enemyState);
                    }
                    //대각선 왼쪽 위 (-1,+1)
                    if (x - 1 >= 0 && y + 1 < 8 && cells[x - 1, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, -1, +1, enemyState);
                    }
                    //대각선 오른쪽 아래(+1,-1)
                    if (x + 1 < 8 && y - 1 >= 0 && cells[x + 1, y - 1].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, -1, enemyState);
                    }
                    //대각선 오른쪽 위 (+1,+1)
                    if (x + 1 < 8 && y + 1 < 8 && cells[x + 1, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, +1, +1, enemyState);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 해당 셀을 기준으로 여러개가 있는지
    /// x : 검사할 기준 셀 x
    /// y : 검사할 기준 셀 y
    /// offset : 방향
    /// targetState : 검사할 상태
    /// </summary>
    private void CheckStraight(int x, int y, int offsetX, int offsetY, OthelloCell.eState targetState)
    {
        //x,y를 기준으로 offsetX,offsetY만큼 증가시키면서
        //state가 none인 애를 찾는겁니다.
        //none이 아니고 targetState도 아니면 막혀있기 때문에(내 돌이 있다) 멈춤

        //x,y가 지금 현재 돌 상태이기 때문에 다음 돌 부터 검사하려고 먼저 offset을 더함
        x += offsetX;
        y += offsetY;

        //x,y를 기준으로 offsetX,offsetY만큼 증가시키면서
        for (; (x >= 0 && x < 8) && (y >= 0 && y < 8); x += offsetX, y += offsetY)
        {
            //state가 none인 애를 찾는겁니다.
            if(cells[x,y].curState == OthelloCell.eState.None)
            {
                cells[x, y].SetState(OthelloCell.eState.Selectable);
                break;
            }
            //none이 아니고 targetState도 아니면 막혀있기 때문에(내 돌이 있다) 멈춤
            else if (cells[x,y].curState != targetState)
            {
                break;
            }
            else if (cells[x, y].curState == targetState) // 생략 가능
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
