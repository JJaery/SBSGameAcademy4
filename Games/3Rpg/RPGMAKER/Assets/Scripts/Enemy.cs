using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    /// <summary>
    /// Enemy가 가지고 있는 상태 enum값
    /// </summary>
    public enum eFsmState
    {
        /// <summary>
        /// 설정 안된 상태
        /// </summary>
        None,

        /// <summary>
        /// 가만히 있는 상태
        /// </summary>
        Idle,
        /// <summary>
        /// 순찰중인 상태
        /// </summary>
        Patrol,
        /// <summary>
        /// 추적중인 상태
        /// </summary>
        Trace,
        /// <summary>
        /// 공격 상태
        /// </summary>
        Attack,
    }


    public eFsmState curState;
    public Vector3 moveDir;

    [Header("공격 사정거리")]
    public float attackRange;

    [Header("인식거리")]
    public float recogRange;

    [Header("내가 찾은 공격 대상")]
    public Character attackTarget;

    private Coroutine delayCoroutine;


    /// <summary>
    /// Start전에 한번 호출되는 약속된 이벤트 메소드
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
        // x,y 랜덤값 : -1, 0, 1
        int value = Random.Range(-1, 2); //세계 최초 RPG게임 울티마 온라인 NPC 이동 방식
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
        //추격(일직선)
        Vector3 dir = attackTarget.transform.position - transform.position;

        //두 오브젝트의 거리를 구하는 방법 2가지
        //1. Vector3.Distance이용하기
        //float dist = Vector3.Distance(transform.position, attackTarget.transform.position);
        //2. Vector 뺄셈 이용하기 + 피타고라스 정리 이용 -> magnitude
        float dist = dir.magnitude; // 제곱근이 들어간 버전 (실제 길이)
        //float dist = dir.sqrMagnitude; //제곱근이 생략된 버전 간단한 거리 비교에서는 무거운 제곱근연산을 제외한 이 버전을 사용한다.

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
            //dir은 길이가 있기 때문에 1로 맞춰줄 필요가 있다. (normalized, 방향만 알고 싶을 때 사용)
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
            //x값 방향으로 이동?
            if (dir.x != 0)
            {
                animator.SetTrigger("SetSide");
            }
            //y값 방향으로 이동?
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
    /// 피격 시 Sprite의 컬러값을 일시적으로 변경
    /// </summary>
    /// <returns></returns>
    IEnumerator HittedRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}