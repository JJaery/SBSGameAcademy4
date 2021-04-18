using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 기능을 담당하는 클래스
///  - 움직임 구현
/// </summary>
public class Player : MonoBehaviour
{
    public float moveSpeed;

    Vector2 resultMoveDelta;

    void Update()
    {
        CheckMove();
    }

    void CheckMove()
    {
        resultMoveDelta = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            resultMoveDelta += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            resultMoveDelta += Vector2.right;
        }

        //캐릭터 움직임
        //gameobject, transform은 모든 오브젝트가 가지고 있기 때문에 손쉽게 접근가능
        //유니티는 프레임-프레임 간 걸린 시간을 제공합니다. -> Time.deltaTime.
        //Time.deltaTime을 곱한다 -> '초당' 을 의미
        transform.position += (Vector3)resultMoveDelta * moveSpeed * Time.deltaTime;
    }
}
