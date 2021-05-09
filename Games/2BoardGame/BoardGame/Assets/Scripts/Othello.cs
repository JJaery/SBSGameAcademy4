using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Othello : MonoBehaviour
{
    //--멤버 변수--//
    //오셀로 보드판을 만들기 위한 조건?
    //1.프리팹
    public GameObject cellPrefab;

    //2. 프리팹을 만들 시작 위치
    public Transform cellStart;

    //3. 게임 진행을 안내하는 UI 텍스트
    public Text logger;

    /// <summary>
    /// 지금 현재 백색 턴인가?
    /// </summary>
    private bool isTurnWhite = true;

    private OthelloCell[,] cells = new OthelloCell[8, 8];
    //--멤버 변수 END --//

    private string playerName
    {
        get
        {
            return isTurnWhite == true ? "흰색" : "검정색";
        }
    }

    private void Start()
    {
        //---보드판 초기화---//
        for (int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                //x:0, y:0 -> x:0, y:1 .... x:8 , y:8 
                GameObject obj = Instantiate(cellPrefab, cellStart);
                // obj.transform.position = cellStart.position; -> localPosition 을 0으로 맞춘다.
                // obj.transform.parent = cellStart;

                // 36 : 셀의 크기
                // 2 : 셀과 셀 사이 여백의 크기
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
        //---보드판 초기화 END---//

        logger.text = string.Empty; //빈 스트링 셋팅

        Log("게임이 시작되었습니다.");

        StartTurn(isTurnWhite);
    }

    private void StartTurn(bool isWhite)
    {
        Log($"\"{playerName}\" 턴이 시작되었습니다!");

        foreach (OthelloCell item in cells)
        {
            //Selectable 초기화
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
                    //아래쪽 검사
                    if (y + 1 < 8 && cells[x, y + 1].curState == enemyState)
                    {
                        CheckStraight(x, y, 0, +1, enemyState);
                    }
                    //위쪽 검사
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
            if (isDraw == true) // 무승부
            {
                Log("서로 점수가 같습니다. 비겼습니다!");
            }
            else
            {
                string winnerName = whiteCount > blackCount ? "흰색" : "검정색";
                Log($"{winnerName}이 이겼습니다!");
                Log($"Score 백: {whiteCount} 흑: {blackCount}");
            }
            return;
        }
        else if (selectableCount == 0)
        {
            //턴오버
            Log($"{playerName}이 둘 곳이 없기 때문에 턴이 종료됩니다.");
            isTurnWhite = !isTurnWhite;
            StartTurn(isTurnWhite);
            return;
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

    public void OnClickCell(OthelloCell targetScript)
    {
        OthelloCell.eState playerState = isTurnWhite == true ? OthelloCell.eState.White : OthelloCell.eState.Black;
        OthelloCell.eState enemyState = playerState == OthelloCell.eState.White ? OthelloCell.eState.Black : OthelloCell.eState.White;

        if(targetScript.curState == OthelloCell.eState.None)
        {
            Log("둘 수 없는 곳입니다.");
        }

        //1. Selectable 셀이면, 자기 자신의 돌로 변경하기
        if (targetScript.curState == OthelloCell.eState.Selectable)
        {
            targetScript.SetState(playerState);

            //2.설치한 돌과 다른 내 돌 사이에 적 돌이 있는지를 체크
            //8개 방향으로 쭈욱 검사를 하면 됩니다.

            int curX = targetScript.x;
            int curY = targetScript.y;
            int getCount = 0;

            //왼쪽
            if (curX - 1 >= 0 && cells[curX - 1, curY].curState == enemyState)
            {
                // 내돌을 마주치기 까지 쭈욱 검사,
                CheckStraightV2(curX, curY, -1, 0, playerState, enemyState, ref getCount);
            }

            //오른쪽
            if (curX + 1 < 8 && cells[curX + 1, curY].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, 0, playerState, enemyState, ref getCount);
            }

            //위
            if (curY - 1 >= 0 && cells[curX, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, 0, -1, playerState, enemyState, ref getCount);
            }

            //아래
            if (curY + 1 < 8 && cells[curX, curY + 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, 0, +1, playerState, enemyState, ref getCount);
            }

            //대각선 왼쪽 아래 (-1,-1)
            if (curX - 1 >= 0 && curY - 1 >= 0 && cells[curX - 1, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, -1, -1, playerState, enemyState, ref getCount);
            }

            //대각선 왼쪽 위 (-1,+1)
            if (curX - 1 >= 0 && curY + 1 < 8 && cells[curX - 1, curY+ 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, -1, +1, playerState, enemyState, ref getCount);
            }

            //대각선 오른쪽 아래 (+1,-1)
            if (curX + 1 < 8 && curY - 1 >= 0 && cells[curX + 1, curY - 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, -1, playerState, enemyState, ref getCount);
            }

            //대각선 오른쪽 위 (+1,+1)
            if (curX + 1 < 8 && curY + 1 < 8 && cells[curX + 1, curY + 1].curState == enemyState)
            {
                CheckStraightV2(curX, curY, +1, +1, playerState, enemyState, ref getCount);
            }


            Log($"{playerName}이 {getCount}개 획득했습니다.");

            //3.턴 오버
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
            else // Selectable 이거나 None인 경우가 여기에 들어갑니다.
            {
                break;
            }
        }
    }

    private void Log(string something)
    {
        //logger.text += "\r\n";
        logger.text += System.Environment.NewLine; // OS에 따라 개행문자가 다름으로, System.Environment.NewLine은 OS에 맞춰서 달라짐
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
