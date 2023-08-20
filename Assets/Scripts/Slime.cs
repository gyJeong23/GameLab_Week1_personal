using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    [SerializeField] Define.MonsterActionState m_currentState;

    [SerializeField] Define.MonsterActionState m_mosterState;
    Define.MonsterActionState MonsterState
    { 
        get { return m_mosterState; }
        set 
        {
            m_mosterState = value;

            #region State Start
            switch (m_mosterState) 
            {
                case Define.MonsterActionState.Idle:
                    {
                        int randomDir = UnityEngine.Random.Range(0, 2);
                        randomDir = (randomDir == 1 ? 1: -1);

                        m_moveDir = new Vector3(randomDir, 0, 0);
                    }
                    break;
                case Define.MonsterActionState.Move:
                    {

                    }
                    break;
                case Define.MonsterActionState.DefualtAttack:
                    {
                        Debug.Log("DefualtAttack");
                        StartCoroutine(Attack(MonsterState, m_defaultCoolTime));
                    }
                    break;
                case Define.MonsterActionState.SpecialAttack:
                    {
                        Debug.Log("SpecialAttack");
                        StartCoroutine(Attack(MonsterState, m_specialCoolTime));
                    }
                    break;
                case Define.MonsterActionState.Hit:
                    {

                    }
                    break;
            }
            #endregion
        }
    }

    Transform m_targetTransform;

    Vector3 m_moveDir = Vector3.right;

    [SerializeField] float m_thinkTime;
    [SerializeField] float m_defaultCoolTime;
    [SerializeField] float m_specialCoolTime;

    float m_thinkTimeCounter;
    float m_targetDistance;
    [SerializeField] float m_detectRange;
    [SerializeField] float m_defaultAttackRange;
    [SerializeField] float m_specialAttackRange;
    [SerializeField] float m_moveSpeed;

    bool m_isGround;
    bool m_isAttacking;
    bool m_canSpecialAttack;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void Start()
    {
        m_currentState = Define.MonsterActionState.Idle;
        m_targetTransform = GameObject.FindWithTag("Player").transform;

    }

    private void Update()
    {
        m_targetDistance = (m_targetTransform.position - transform.position).magnitude;

        if (m_thinkTimeCounter > m_thinkTime && m_isGround)
        {
            m_thinkTimeCounter = 0;
            MonsterState = m_currentState;
        }    
        else
            m_thinkTimeCounter += Time.deltaTime;

        if (m_thinkTimeCounter > m_specialCoolTime)
            m_canSpecialAttack = true;

        #region State Update
        switch (MonsterState)
        {
            case Define.MonsterActionState.Idle:
                {
                    IdleState();
                }
                break;
            case Define.MonsterActionState.Move:
                {
                    MoveState();
                }
                break;
        }
        #endregion
    }

    private void SetState(Define.MonsterActionState _state)
    {
        if (_state == m_currentState)
            return;

        MonsterState = _state;
    }

    private void IdleState()
    { 

        if (m_targetDistance < m_detectRange)
            MonsterState = Define.MonsterActionState.Move;

        Move(m_moveDir, m_moveSpeed);
    }

    private void MoveState()
    {

        if (m_targetDistance < m_specialAttackRange && m_canSpecialAttack)
            MonsterState = Define.MonsterActionState.SpecialAttack;
        if (m_targetDistance < m_defaultAttackRange)
            MonsterState = Define.MonsterActionState.DefualtAttack;

        if (m_targetDistance > m_detectRange)
            m_currentState = Define.MonsterActionState.Idle;

        m_moveDir = new Vector3((m_targetTransform.position - transform.position).x, 0).normalized;

        Move(m_moveDir, m_moveSpeed * 1.5f);
    }

    IEnumerator Attack(Define.MonsterActionState _state, float _coolTime)
    {
        m_isAttacking = true;

        yield return new WaitForSeconds(_coolTime);

        if (_state == Define.MonsterActionState.SpecialAttack)
            m_canSpecialAttack = false;

        m_currentState = Define.MonsterActionState.Idle;
        m_isAttacking = false;
    }

    void Move(Vector3 _moveDir, float _moveSpeed)
    {
        if (m_isAttacking) return;

        transform.position += _moveDir * _moveSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }

    void FixedUpdate()
    {
        if (m_isGround == true)
        {
            Vector3 frontVec = transform.position + Vector3.right * m_moveDir.x;
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider == null)
                m_moveDir *= -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            m_isGround = true; 

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            m_isGround = false;
    }

    #endregion
}
