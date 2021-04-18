using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일을 관리하는 스크립트.
/// 1. 미사일이 특정 지점에 도달하면 삭제 v
/// 2. 플레이어 충돌 감지
/// </summary>
public class Missile : MonoBehaviour
{
    void Update()
    {
        CheckDeadLine();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            OnHit();
        }
    }

    void CheckDeadLine()
    {
        if(this.transform.position.y < -5)
        {
            //삭제
            // Destroy(this); -> 스크립트 삭제
            GameManager.score += 100;
            UIManager.Instance.scoreLabel.text = GameManager.score.ToString();
            Destroy(this.gameObject);
        }
    }

    void OnHit()
    {
        //사망 조건이 있으면 여기에 추가하면 되는겁니다.
        UIManager.Instance.ShowGameOverUI();
        Destroy(this.gameObject);
    }


}
