using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

 public class PlayerController : BaseController
{
    #region PublicVariables
    #endregion

    #region ProtectedVariables

    protected override float MoveSpeed { get; set; }

    protected override bool IsGrounded { get; set; }
    protected override bool CanMove { get; set; }
    protected override bool IsAttacking { get; set; }
    protected override bool IsDashing { get; set; }

    #endregion

    #region PrivateVariables

    [SerializeField] Define.PlayerActionState m_playerActionState;
    [SerializeField] Define.PlayerMoveState m_playerMoveState;

    [SerializeField] float m_dashPower;
    [SerializeField] float m_jumpPower;

    float m_WeakAttackcoolTime = 0.2f;
    float m_SpecialAttackcoolTime = 0.8f;
    float m_CounterCoolTime = 0.8f;
    [SerializeField] float m_DashcoolTime;
    
    bool m_hasWeapon;
    bool m_hasdash;
    bool m_hasStorngAttack;
    bool m_hasCounter;

    Vector3 m_moveDir;

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected override void Init()
    {
        Util.SearchChild(transform, "sword").gameObject.SetActive(false);

        m_moveDir = Vector3.right;
    }

    protected override void OnUpdate()
    {
        OnKeyboard();

        if (IsDashing == true)
        { 
            Dash(m_moveDir, MoveSpeed);
        }    
    }

    protected override void Move(Vector3 _moveDir, float _moveSpeed)
    {
        transform.position += _moveDir * _moveSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }

    #endregion 

    #region PrivateMethod
   
    private Define.PlayerActionState ActionState
    {
        get { return m_playerActionState; }
        set
        {
            m_playerActionState = value;

            switch (m_playerActionState)
            {
                case Define.PlayerActionState.Idle:
                    {
                        m_animator.CrossFade("Idle", 0.1f);
                    }
                    break;
                case Define.PlayerActionState.DefualtAttack:
                    {
                        m_animator.CrossFade("DefaultAttack", 0.1f, -1, 0);
                        StartCoroutine(Attack("DefaultAttack", m_WeakAttackcoolTime));
                    }
                    break;
                case Define.PlayerActionState.SpecialAttack:
                    {
                        m_animator.CrossFade("SpecialAttack", 0.1f, -1, 0);
                        StartCoroutine(Attack("SpecialAttack", m_SpecialAttackcoolTime));
                    }
                    break;
                case Define.PlayerActionState.Counter:
                    {
                        m_animator.CrossFade("Counter", 0.1f, -1, 0);
                        StartCoroutine(Attack("Counter", m_CounterCoolTime));
                    }
                    break;
                case Define.PlayerActionState.Dash:
                    {
                        m_animator.CrossFade("Dash", 0.1f, -1, 0);
                        StartCoroutine(nameof(DashOut), m_DashcoolTime);
                    }
                    break;
            }
        }
    }

    void OnKeyboard()
    {
        //if (ESCMenu.g_pause == true)
        //{
        //    return;
        //}

        #region Player Move

        if (Input.GetKey(KeyCode.A) && IsDashing == false)
        {
            m_moveDir = Vector3.left;
            Move(m_moveDir, m_moveSpeed);
        }
        if (Input.GetKey(KeyCode.D) && IsDashing == false)
        {
            m_moveDir = Vector3.right;
            Move(m_moveDir, m_moveSpeed);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        #endregion
        #region Player Action

        if (IsAttacking == false /*&& Isdashing == false*/)
        {
            if (m_hasWeapon == true)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    ActionState = Define.PlayerActionState.DefualtAttack;
                }
                if (Input.GetKeyDown(KeyCode.K) && m_hasStorngAttack)
                {
                    ActionState = Define.PlayerActionState.SpecialAttack;
                }
                if (Input.GetKeyDown(KeyCode.L) && m_hasCounter)
                {
                    ActionState = Define.PlayerActionState.Counter;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && m_hasdash && IsDashing == false)
            {
                ActionState = Define.PlayerActionState.Dash;
            }
        }

        #endregion
    }
    
    IEnumerator Attack(string _attack, float _coolTime)
    {
        IsAttacking = true;

        Transform attackTrigger = Util.SearchChild(transform, _attack);
        attackTrigger.gameObject.SetActive(true);

        yield return new WaitForSeconds(_coolTime);
        attackTrigger.gameObject.SetActive(false);

        ActionState = Define.PlayerActionState.Idle;
        IsAttacking = false;
    }

    void Jump()
    {
        if (IsGrounded == false) return;

        m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode2D.Impulse);
        IsGrounded = false;
    }

    void Dash(Vector3 _moveDir, float _moveSpeed)
    {
        StartCoroutine(nameof(DashOut), m_DashcoolTime);
        Move(_moveDir, _moveSpeed * m_dashPower);
    }

    IEnumerator DashOut(float _coolTime)
    {
        IsDashing = true;
        yield return new WaitForSeconds(_coolTime);

        IsDashing = false;
    }

    #region Trigger/Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(nameof(Define.TagName.DropItem)))
        {
            Define.ItemType itemType = other.gameObject.GetComponent<DropItem>().m_itemType;

            switch (itemType)
            {
                case Define.ItemType.Weapon:
                    {
                        m_hasWeapon = true;
                        GameObject go = Util.SearchChild(transform, "sword").gameObject;
                        go.SetActive(true);
                    }
                    break;
                case Define.ItemType.Dash:
                    m_hasdash = true;
                    break;
                case Define.ItemType.StrongAttack:
                    m_hasStorngAttack = true;
                    break;
                case Define.ItemType.Counter:
                    m_hasCounter = true;
                    break;
            }

            Destroy(other.gameObject);
        }
    }

    #endregion

    #endregion
}
