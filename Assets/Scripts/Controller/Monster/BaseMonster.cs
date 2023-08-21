using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.CompilerServices;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    #region Protected Variables

    [SerializeField] protected Define.MonsterState m_currentState;
    [SerializeField] protected Define.MonsterState m_mosterState;

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

                    }
                    break;
                case Define.MonsterState.SpecialAttack:
                    {
                        if (m_canSpecialAttack == false)
                            MonsterState = Define.MonsterState.Move;

                        m_animator.SetTrigger("onSpecialAttack");
                        StartCoroutine(AttackState("SpecialAttack", m_specialCoolTime));
                        m_canSpecialAttack = false;
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
                       

                        m_animator.SetTrigger("onDie");
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

    protected Transform m_targetTransform;
    protected Animator m_animator;
    protected Rigidbody2D m_rigidbody;
    protected Transform m_invincibleState;

    protected Vector3 m_moveDir;

    protected float m_thinkTimeCounter;
    protected float m_targetDistance;

    protected bool m_isGround;
   [SerializeField] protected bool m_isAttacking;
    protected bool m_canSpecialAttack = true;
    protected bool m_isHit;
    protected bool m_isDead;


    [SerializeField] protected GameObject m_dropItem;

    protected int m_life;

    protected float m_thinkTime;
    protected float m_defaultCoolTime;
    protected float m_specialCoolTime;
    protected float m_hitCoolTime;

    protected float m_detectRange;
    protected float m_defaultAttackRange;
    protected float m_specialAttackRange;
    protected float m_moveSpeed;
    protected float m_knockBackPower;

    #endregion

    #region Protected Methods

    protected virtual void Start()
    {
        Util.SearchChild(transform, "Body").TryGetComponent<Animator>(out m_animator);
        TryGetComponent<Rigidbody2D>(out m_rigidbody);

        m_currentState = Define.MonsterState.Idle;
        m_moveDir = Vector3.right;
        m_targetTransform = GameObject.FindWithTag("Player").transform;
        m_invincibleState = Util.SearchChild(transform, "Invincible State");

        m_invincibleState.gameObject.SetActive(false);
        Util.SearchChild(transform, "DefaultAttack").gameObject.SetActive(false);
        Util.SearchChild(transform, "SpecialAttack").gameObject.SetActive(false);

    }

    protected void Update()
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
                    MonsterState = Define.MonsterState.Dead;
                }
                break;
        }
        #endregion

        InvincibleState();
    }
    protected void FixedUpdate()
    {
        

        if (m_isGround == true)
        {
            bool isWall = IsWall();
            bool isGround = IsGround();

            if (isWall == true || isGround == false)
                m_moveDir *= -1;

        }    
    }

    protected bool IsGround()
    {
        Vector3 frontVec = transform.position + Vector3.right * m_moveDir.x;
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (rayHit.collider != null)
            return true;
        else return false;
    }

    protected bool IsWall()
    {
        Vector3 frontVec = transform.position + Vector3.right * m_moveDir.x + Vector3.up;
        Debug.DrawRay(frontVec, m_moveDir, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, m_moveDir, 1, LayerMask.GetMask("Ground"));

        if (rayHit.collider != null)
            return true;
        else return false;

    }

    void InvincibleState()
    {
        if (m_isHit)
            m_invincibleState.gameObject.SetActive(true);
        else
            m_invincibleState.gameObject.SetActive(false);
    }

    protected void IdleState()
    {

        if (m_targetDistance < m_detectRange)
            m_currentState = Define.MonsterState.Move;

        Move(m_moveDir, m_moveSpeed);
    }

    protected void MoveState()
    {
        if (m_isAttacking == false)
        {
            if (m_targetDistance < m_specialAttackRange && m_canSpecialAttack == true)
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


    protected virtual IEnumerator AttackState(string _attack, float _coolTime)
    {
        if (m_isAttacking)
        {
            m_currentState = Define.MonsterState.Move;
            yield break;
        }

        m_isAttacking = true;

        Transform attackTrigger = Util.SearchChild(transform, _attack);
        attackTrigger.gameObject.SetActive(true);
        float curAnimationTime = m_animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(curAnimationTime);
        attackTrigger.gameObject.SetActive(false);
        m_isAttacking = false;

        if (_attack.Equals("SpecialAttack"))
        {
            m_canSpecialAttack = false;

            yield return new WaitForSeconds(_coolTime);
            m_canSpecialAttack = true;
        }

        m_currentState = Define.MonsterState.Move;
    }

    protected void Move(Vector3 _moveDir, float _moveSpeed)
    {
        if (m_isAttacking) return;

        transform.position += _moveDir * _moveSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }

    protected IEnumerator Hit(float _cooltime)
    {
        m_isHit = true;

        yield return new WaitForSeconds(_cooltime);

        m_isHit = false;
    }

    protected IEnumerator Die()
    {
        if (m_isDead) yield break;

        m_isDead = true;
        DropItem();

        Collider2D monsterCollider;
        TryGetComponent<Collider2D>(out monsterCollider);
        monsterCollider.isTrigger = true;

        if (m_isGround)
            m_rigidbody.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    protected void DropItem()
    {
        if (m_dropItem == null) return;
        if (m_isDead == false) return;

        GameObject dropItem = GameObject.Instantiate<GameObject>(m_dropItem);

        dropItem.transform.position = transform.position;
        dropItem.GetComponent<Rigidbody2D>().velocity = Vector3.up * 7f;

        GameObject go = GameObject.Find("DropItems");
        dropItem.transform.SetParent(go.transform);
    }

    protected void KnockBack(Vector3 _mosterVec, float _knockBackPower)
    {
        Vector3 knockBackDir = new Vector3(_mosterVec.x, 0, 0).normalized;
        knockBackDir += Vector3.up * 3f;

        m_rigidbody.velocity = knockBackDir * _knockBackPower;
        Util.LimitVelocity2D(m_rigidbody, Vector3.one * _knockBackPower);
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && m_isGround == false)
            m_isGround = true;

    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && m_isGround == true)
            m_isGround = false;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WeakAttack") || collision.CompareTag("StrongAttack"))
        {
            if (m_isDead) return;
            if (m_isHit) return;

            Vector3 playerToMonsterVec = transform.position - m_targetTransform.position;


            if (collision.CompareTag("StrongAttack"))
                KnockBack(playerToMonsterVec, m_knockBackPower * 1.5f);
            else
                KnockBack(playerToMonsterVec, m_knockBackPower);


            m_life--;

            if (m_life < 1)
            {
                MonsterState = Define.MonsterState.Die;
            }
            else
                MonsterState = Define.MonsterState.Hit;

        }
    }


    #endregion
}
