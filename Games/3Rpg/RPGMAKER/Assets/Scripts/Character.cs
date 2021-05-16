using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player,Monster,NPC 3가지가 공통적으로 가지고 있는 요소들을 정의
/// </summary>
public abstract class Character : MonoBehaviour
{
    #region 캐릭터 컴포넌트
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    #endregion

    #region 캐릭터 속성치
    public float hp;
    public float moveSpeed;
    public float dmg;
    #endregion

    public virtual void Update()
    {
        Move();
    }

    public abstract void Move();
}
