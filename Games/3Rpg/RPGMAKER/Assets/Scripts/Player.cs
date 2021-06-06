using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool isMovable = true;

    private Vector2 lastMoveDir = Vector2.down;

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public override void Attack()
    {
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("IsAttack");
        isMovable = false;
    }

    public override void OnHitEvent()
    {
        isMovable = true;

        ///Vector2.one == new Vector2(1,1)
        Collider2D target = Physics2D.OverlapBox((Vector2)transform.position + lastMoveDir,
                                                    Vector2.one, 0, LayerMask.GetMask("Enemy"));
        if (target != null)
        {
            Debug.Log(target.name);
            Character targetScript = target.GetComponent<Character>();
            targetScript.OnHitted(this);
        }
    }

    public override void OnHitted(Character hitter)
    {
        this.hp -= hitter.dmg;
        StartCoroutine(HittedRoutine());

        if (this.hp <= 0)
        {
            Debug.LogError("게임오버");
        }
    }

    /// <summary>
    /// 피격 시 Sprite의 컬러값을 일시적으로 변경
    /// </summary>
    /// <returns></returns>
    IEnumerator HittedRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }


    public override void Move()
    {

        Vector3 moveDir = Vector3.zero;

        animator.ResetTrigger("DoWalkSide");
        animator.ResetTrigger("DoWalkBack");
        animator.ResetTrigger("DoWalkFront");

        if (isMovable == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir = Vector3.right;
            animator.SetTrigger("DoWalkSide");
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir = Vector3.up;
            animator.SetTrigger("DoWalkBack");
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir = Vector3.down;
            animator.SetTrigger("DoWalkFront");
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir = Vector3.left;
            animator.SetTrigger("DoWalkSide");
            spriteRenderer.flipX = true;
        }

        if (moveDir != Vector3.zero)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            lastMoveDir = moveDir;

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}
