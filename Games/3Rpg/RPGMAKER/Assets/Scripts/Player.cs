using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Move()
    {
        Vector3 moveDir = Vector3.zero;

        animator.ResetTrigger("DoWalkSide");
        animator.ResetTrigger("DoWalkBack");
        animator.ResetTrigger("DoWalkFront");

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
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}
