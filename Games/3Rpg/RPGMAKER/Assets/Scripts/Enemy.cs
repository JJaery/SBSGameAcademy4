using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    /// <summary>
    /// Enemy�� ������ �ִ� ���� enum��
    /// </summary>
    public enum eFsmState
    {
        /// <summary>
        /// ���� �ȵ� ����
        /// </summary>
        None,

        /// <summary>
        /// ������ �ִ� ����
        /// </summary>
        Idle,
        /// <summary>
        /// �������� ����
        /// </summary>
        Patrol,
        /// <summary>
        /// �������� ����
        /// </summary>
        Trace,
        /// <summary>
        /// ���� ����
        /// </summary>
        Attack,
    }


    public eFsmState curState;
    public Vector3 moveDir;

    [Header("���� �����Ÿ�")]
    public float attackRange;

    [Header("�νİŸ�")]
    public float recogRange;

    [Header("���� ã�� ���� ���")]
    public Character attackTarget;

    private Coroutine delayCoroutine;


    /// <summary>
    /// Start���� �ѹ� ȣ��Ǵ� ��ӵ� �̺�Ʈ �޼ҵ�
    /// </summary>
    private void Awake()
    {
        ChangeState(eFsmState.Idle);
    }

    public override void Update()
    {
        base.Update();

        switch(curState)
        {
            case eFsmState.Idle:
                OnUpdateIdleState();
                break;
            case eFsmState.Patrol:
                OnUpdatePatrolState();
                break;
            case eFsmState.Trace:
                OnUpdateTraceState();
                break;
            case eFsmState.Attack:
                OnUpdateAttackState();
                break;
        }
    }

    public void ChangeState(eFsmState targetState)
    {
        switch(curState)
        {
            case eFsmState.Idle:
                OnExitIdleState();
                break;
            case eFsmState.Patrol:
                OnExitPatrolState();
                break;
            case eFsmState.Trace:
                OnExitTraceState();
                break;
            case eFsmState.Attack:
                OnExitAttackState();
                break;
        }

        switch(targetState)
        {
            case eFsmState.Idle:
                OnEnterIdleState();
                break;
            case eFsmState.Patrol:
                OnEnterPatrolState();
                break;
            case eFsmState.Trace:
                OnEnterTraceState();
                break;
            case eFsmState.Attack:
                OnEnterAttackState();
                break;
        }

        curState = targetState;
    }

    #region Exit
    private void OnExitIdleState()
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }
    }
    private void OnExitPatrolState()
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }
    }
    private void OnExitTraceState()
    {

    }
    private void OnExitAttackState()
    {

    }
    #endregion

    #region Enter
    private void OnEnterIdleState()
    {
        animator.SetBool("IsWalking", false);
        delayCoroutine = StartCoroutine(DelayChangePatrol());
    }

    private void OnEnterPatrolState()
    {
        // x,y ������ : -1, 0, 1
        int value = Random.Range(-1, 2); //���� ���� RPG���� ��Ƽ�� �¶��� NPC �̵� ���
        if(value != 0)
        {
            moveDir = new Vector3(value, 0, 0);
        }
        else
        {
            moveDir = new Vector3(0, Random.Range(-1, 2), 0);
        }

        delayCoroutine = StartCoroutine(DelayChangePatrolToIdle());
    }
    private void OnEnterTraceState()
    {

    }
    private void OnEnterAttackState()
    {
        animator.SetBool("IsWalking", false);
    }
    #endregion

    #region Update
    private void OnUpdateIdleState()
    {
        CheckRecoRange();
    }
    private void OnUpdatePatrolState()
    {
        Move(moveDir);
        CheckRecoRange();
    }
    private void OnUpdateTraceState()
    {
        //�߰�(������)
        Vector3 dir = attackTarget.transform.position - transform.position;

        //�� ������Ʈ�� �Ÿ��� ���ϴ� ��� 2����
        //1. Vector3.Distance�̿��ϱ�
        //float dist = Vector3.Distance(transform.position, attackTarget.transform.position);
        //2. Vector ���� �̿��ϱ� + ��Ÿ��� ���� �̿� -> magnitude
        float dist = dir.magnitude; // �������� �� ���� (���� ����)
        //float dist = dir.sqrMagnitude; //�������� ������ ���� ������ �Ÿ� �񱳿����� ���ſ� �����ٿ����� ������ �� ������ ����Ѵ�.

        if (dist >= recogRange * 1.2f)
        {
            attackTarget = null;
            ChangeState(eFsmState.Idle);
        }
        else if(dist <= attackRange)
        {
            ChangeState(eFsmState.Attack);
        }
        else
        {
            //dir�� ���̰� �ֱ� ������ 1�� ������ �ʿ䰡 �ִ�. (normalized, ���⸸ �˰� ���� �� ���)
            Move(dir.normalized);
        }
    }

    private float lastAttackTime;

    private void OnUpdateAttackState()
    {
        if(attackTarget == null)
        {
            ChangeState(eFsmState.Idle);
            return;
        }

        if (Vector2.Distance(this.transform.position, attackTarget.transform.position) > attackRange)
        {
            ChangeState(eFsmState.Trace);
            return;
        }

        if(Time.time > lastAttackTime + 1)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    public override void Attack()
    {
        attackTarget.OnHitted(this);
    }


    private void CheckRecoRange()
    {
        int layerMask = LayerMask.GetMask("Player");  // 1 << 6;
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, recogRange, layerMask);

        foreach(Collider2D item in target)
        {
            if (Vector3.Distance(item.transform.position, transform.position) <= recogRange)
            {
                attackTarget = item.GetComponent<Character>();
                ChangeState(eFsmState.Trace);
                break;
            }
        }
    }
    #endregion


    IEnumerator DelayChangePatrol()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        ChangeState(eFsmState.Patrol);
    }

    IEnumerator DelayChangePatrolToIdle()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(eFsmState.Idle);
    }

    public void Move(Vector3 dir)
    {
        if (animator.GetBool("IsWalking") == false)
        {
            //x�� �������� �̵�?
            if (dir.x != 0)
            {
                animator.SetTrigger("SetSide");
            }
            //y�� �������� �̵�?
            else if (dir.y > 0)
            {
                animator.SetTrigger("SetBack");
            }
            else if (dir.y < 0)
            {
                animator.SetTrigger("SetForward");
            }
        }

        spriteRenderer.flipX = dir.x < 0;
        animator.SetBool("IsWalking", true);

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public override void Move()
    {

    }

    public override void OnHitted(Character hitter)
    {
        this.hp -= hitter.dmg;
        StartCoroutine(HittedRoutine());

        if (this.hp <= 0)
            Destroy(this.gameObject);
    }

    /// <summary>
    /// �ǰ� �� Sprite�� �÷����� �Ͻ������� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator HittedRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}