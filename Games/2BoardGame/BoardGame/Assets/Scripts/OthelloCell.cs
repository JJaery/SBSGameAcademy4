using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OthelloCell : MonoBehaviour
{
    public int x;
    public int y;
    public Othello boardScript;

    public Image btnImage;

    //current state -> ���� ����
    public eState curState = eState.None;

    public enum eState
    {
        None,
        White,
        Black,
        /// <summary>
        /// ��ġ������ ����
        /// </summary>
        Selectable
    }

    public void SetState(eState targetState)
    {
        switch(targetState)
        {
            case eState.None:
                btnImage.color = new Color(0, 0, 0, 0);
                break;
            case eState.White:
                btnImage.color = new Color(1, 1, 1, 1);
                break;
            case eState.Black:
                btnImage.color = new Color(0, 0, 0, 1);
                break;
            case eState.Selectable:
                btnImage.color = new Color(0.2f, 0.2f, 1f, 1);
                break;
        }

        curState = targetState;
    }

    /// <summary>
    /// Cell ��ư�� ������ ��
    /// </summary>
    public void OnClickButton()
    {
        boardScript.OnClickCell(this);
    }
}
