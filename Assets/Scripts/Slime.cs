using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    [SerializeField] Define.MonsterState m_currentState;
    [SerializeField] Define.MonsterState m_mosterState;

    Define.MonsterState MonsterState
    {
        get { return m_mosterState; }
        set
        {
            m_mosterState = value;

            #region State Start
            switch (m_mosterState)
            {
                case Define.MonsterState.Idle:
                    {
                        m_animator.CrossFade("Idle", 0.1f);
                        int randomDir = UnityEngine.Random.Range(0, 2);
                        randomDir = (randomDir == 1 ? 1 : -1);

                        m_moveDir = new Vector3(randomDir, 0, 0);
                    }
                    break;
                case Define.MonsterState.Move:
                    {
                        m_animator.CrossFade("Move", 0.1f);
                    }
                    break;
                case Define.MonsterState.DefualtAttack:
                    {
                        m_animator.SetTrigger("onDefaultAttack");
                        StartCoroutine(AttackState("DefaultAttack", m_defaultCoolTime));
                        m_currentState = Define.MonsterState.Move;

                    }
                    break;
                case Define.MonsterState.SpecialAttack:
                    {
                        m_animator.SetTrigger("onSpecialAttack");
                        StartCoroutine(AttackState("SpecialAttack", m_specialCoolTime));
                        m_canSpecialAttack = false;

                        m_currentState = Define.MonsterState.Move;
                    }
                    break;
                case Define.MonsterState.Hit:
                    {
                        m_animator.SetTrigger("onHit");
                        StartCoroutine(nameof(Hit), m_hitCoolTime);
                        m_currentState = Define.MonsterState.Move;
                    }
                    break;
                case Define.MonsterState.Die:
                    {
                        m_isDead = true;
                        DropItem();

                        m_animator.SetTrigger("onHit");
                        StartCoroutine(nameof(Die));
                        MonsterState = Define.MonsterState.Dead;
                    }
                    break;
                case Define.MonsterState.Dead:
                    {
                       
                    }
                    break;
            }
            #endregion
        }
    }

    Transform m_targetTransform;
    Animator m_animator;
    Rigidbody2D m_rigidbody;
    [SerializeField] GameObject m_dropItem;

    Vector3 m_moveDir;

    [SerializeField] int m_life;

    [SerializeField] float m_thinkTime;
    [SerializeField] float m_defaultCoolTime;
    [SerializeField] float m_specialCoolTime;
    [SerializeField] float m_hitCoolTime;

    float m_thinkTimeCounter;
    float m_targetDistance;
    [SerializeField] float m_detectRange;
    [SerializeField] float m_defaultAttackRange;
    [SerializeField] float m_specialAttackRange;
    [SerializeField] float m_moveSpeed;
    [SerializeField] float m_knockBackPower;

    [SerializeField] bool m_isGround;
    bool m_isAttacking;
    bool m_canSpecialAttack = true;
    bool m_isHit;
    bool m_isDead;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void Start()
    {
        Util.SearchChild(transform, "Body").TryGetComponent<Animator>(out m_animator);
        TryGetComponent<Rigidbody2D>(out m_rigidbody);

        m_currentState = Define.MonsterState.Idle;
        m_moveDir = Vector3.right;
        m_targetTransform = GameObject.FindWithTag("Player").transform;

    }

    private void Update()
    {
        if (m_isDead) return;

        m_targetDistance = (m_targetTransform.position - transform.position).magnitude;

        if (m_thinkTimeCounter > m_thinkTime && m_isGround)
        {
            m_thinkTimeCounter = 0;
            MonsterState = m_currentState;
        }
        else
            m_thinkTimeCounter += Time.deltaTime;

        #region State Update
        switch (MonsterState)
        {
            case Define.MonsterState.Idle:
                {
                    IdleState();
                }
                break;
            case Define.MonsterState.Move:
                {
                    MoveState();
                }
                break;
            case Define.MonsterState.Dead:
                {
                    Dead();
                    MonsterState = Define.MonsterState.Dead;       
                }
                break;
        }
        #endregion
    }
    void FixedUpdate()
    {
        if (m_isGround == true)
            Turn();
    }

    private void Turn()
    {
        Vector3 frontVec = transform.position + Vector3.right * m_moveDir.x;
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        
        if (rayHit.collider == null)
            m_moveDir *= -1f;
    }

    private void IdleState()
    {

        if (m_targetDistance < m_detectRange)
            m_currentState = Define.MonsterState.Move;

        Move(m_moveDir, m_moveSpeed);
    }

    private void MoveState()
    {
        if (m_isAttacking == false) 
        {
            if (m_targetDistance < m_specialAttackRange && m_canSpecialAttack)
                m_currentState = Define.MonsterState.SpecialAttack;
            else if (m_targetDistance < m_defaultAttackRange)
                m_currentState = Define.MonsterState.DefualtAttack;
        }

        if (m_targetDistance > m_detectRange)
            m_currentState = Define.MonsterState.Idle;

        if (m_targetDistance > 1f)
            m_moveDir = new Vector3((m_targetTransform.position - transform.position).x, 0).normalized;

        Move(m_moveDir, m_moveSpeed * 1.5f);
    }

    IEnumerator AttackState(string _attack, float _coolTime)
    {
        m_isAttacking = true;

        Transform attackTrigger = Util.SearchChild(transform, _attack);
        attackTrigger.gameObject.SetActive(true);
        float curAnimationTime = m_animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(curAnimationTime);
        attackTrigger.gameObject.SetActive(false);
        m_currentState = Define.MonsterState.Idle;

        m_isAttacking = false;

        if (_attack.Equals("SpecialAttack"))
        {
            m_canSpecialAttack = false;

            yield return new WaitForSeconds(_coolTime);
            m_canSpecialAttack = true;
        }

    }

    void Move(Vector3 _moveDir, float _moveSpeed)
    {
        transform.position += _moveDir * _moveSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }


    IEnumerator Hit(float _cooltime)
    {
        m_isHit = true;
        
        yield return new WaitForSeconds(_cooltime);
        
        m_isHit = false;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(m_hitCoolTime / 2);

        m_animator.SetTrigger("onDie");

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (m_dropItem == null) return;
        if (m_isDead == false) return;

        GameObject dropItem = GameObject.Instantiate<GameObject>(m_dropItem);

        dropItem.transform.position = transform.position;
        dropItem.GetComponent<Rigidbody2D>().velocity = Vector3.up * 5f;

        GameObject go = GameObject.Find("DropItems");
        dropItem.transform.SetParent(go.transform);
    }

    void Dead()
    {
        Collider2D monsterCollider;
        TryGetComponent<Collider2D>(out monsterCollider);
        monsterCollider.isTrigger = true;

        if (m_isGround) 
            m_rigidbody.bodyType = RigidbodyType2D.Static;
    }

    void KnockBack(Vector3 _mosterVec)
    {
        Vector3 knockBackDir = new Vector3(_mosterVec.x, 0, 0).normalized;
        knockBackDir += Vector3.up * 2f;

        m_rigidbody.velocity = knockBackDir * m_knockBackPower;
        Util.LimitVelocity2D(m_rigidbody, Vector3.one * m_knockBackPower);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && m_isGround == false)
            m_isGround = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && m_isGround == true)
            m_isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WeakAttack") || collision.CompareTag("StrongAttack"))
        {
            Vector3 playerToMonsterVec = transform.position - m_targetTransform.position;

            if (m_isHit == false && m_isDead == false)
            {
                KnockBack(playerToMonsterVec);

                m_life--;

                if (m_life < 1)
                {
                    KnockBack(playerToMonsterVec);
                    MonsterState = Define.MonsterState.Die;
                }   
                else
                    MonsterState = Define.MonsterState.Hit;
            }
        }
    }

    #endregion
    
}
