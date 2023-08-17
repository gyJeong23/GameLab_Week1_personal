using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    Rigidbody2D m_rigidbody;
    Animator m_animator;
    
    Define.PlayerState m_playerState;

    [SerializeField] float m_playerSpeed;
    [SerializeField] float m_deshRange;
    [SerializeField] float m_jumpPower;
    
    float m_coolTimeWeakAttack = 0.3f;
    float m_coolTimeStrongAttack = 1f;
    float m_coolTimeCounter = 1f;
    float m_coolTimeDesh = 1f;

    bool m_isGround;
    bool m_isAttackCoolTime;
    bool m_isDeshCoolTime;

    #endregion

    #region PublicMethod


    #endregion

    #region PrivateMethod

    private Define.PlayerState State
    {
        get { return m_playerState; }
        set
        {
            m_playerState = value;

            switch (m_playerState)
            {
                case Define.PlayerState.Idle:
                    m_animator.CrossFade("idle", 0.1f);
                    break;
                case Define.PlayerState.WeakAttack:
                    m_animator.CrossFade("WeakAttack", 0.1f, -1, 0);
                    break;
                case Define.PlayerState.StrongAttack:
                    m_animator.CrossFade("StrongAttack", 0.1f, -1, 0);
                    break;
                case Define.PlayerState.Counter:
                    m_animator.CrossFade("Counter", 0.1f, -1, 0);
                    break;
            }
        }
    }

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = transform.GetComponentInChildren<Animator>();
    }

    void OnKeyboard()
    {
        //if (ESCMenu.g_pause == true)
        //{
        //    return;
        //}

        //Player Action
        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_isDeshCoolTime == false)
            {
                State = Define.PlayerState.Desh;
                m_isDeshCoolTime = true;
                Desh(Vector3.left);
                Invoke("WaitmDeshCoolTime", m_coolTimeDesh);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_isDeshCoolTime == false)
            {
                State = Define.PlayerState.Desh;
                m_isDeshCoolTime = true;
                Desh(Vector3.right);
                Invoke("WaitmDeshCoolTime", m_coolTimeDesh);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (m_isAttackCoolTime == false)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                State = Define.PlayerState.WeakAttack;
                m_isAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeWeakAttack);
            }
            if (Input.GetKeyDown(KeyCode.K) && m_isAttackCoolTime == false)
            {
                State = Define.PlayerState.StrongAttack;
                m_isAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeStrongAttack);
            }
            if (Input.GetKeyDown(KeyCode.L) && m_isAttackCoolTime == false)
            {
                State = Define.PlayerState.Counter;
                m_isAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeCounter);
            }
        }
    }

    void WaitAttackCoolTime()
    {
        m_isAttackCoolTime = false;
    }

    void WaitmDeshCoolTime()
    {
        m_isDeshCoolTime = false;
    }

    #region Player Action

    void Move(Vector3 _moveDir)
    {
        transform.position += _moveDir * m_playerSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }

    void Jump()
    {
        if (m_isGround)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode2D.Impulse);
            m_isGround = false;
        }
    }

    void WeakAttack() 
    {
    }

    void StrongAttack()
    {
    }

    void Counter()
    {
    }

    void Desh(Vector3 _moveDir)
    {
        m_rigidbody.velocity = Vector3.zero;
        transform.position += Vector3.right * m_deshRange * _moveDir.x;
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            m_isGround = true;
    }

    #endregion
}
