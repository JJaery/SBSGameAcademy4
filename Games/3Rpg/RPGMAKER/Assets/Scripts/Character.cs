using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player,Monster,NPC 3������ ���������� ������ �ִ� ��ҵ��� ����
/// </summary>
public abstract class Character : MonoBehaviour
{
    #region ĳ���� ������Ʈ
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    #endregion

    #region ĳ���� �Ӽ�ġ
    public float hp;
    public float moveSpeed;
    public float dmg;
    #endregion

    public virtual void Update()
    {
        Move();
    }

    public abstract void Move();

    public virtual void OnHitted(Character hitter)
    {

    }

    public virtual void OnHitEvent()
    {

    }

    public virtual void Attack()
    {

    }
}
