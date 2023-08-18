using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerController : BaseController
{
    #region PublicVariables
    #endregion

    #region ProtectedVariables

    [SerializeField] protected override float MoveSpeed { get; set; }

    [SerializeField] protected override bool IsGround { get; set; }
    [SerializeField] protected override bool IsAttackCoolTime { get; set; }
    [SerializeField] protected override bool IsDeshCoolTime { get; set; }

    #endregion

    #region PrivateVariables

    Define.PlayerState m_playerState;

    [SerializeField] float m_deshRange;
    [SerializeField] float m_jumpPower;

    float m_coolTimeWeakAttack = 0.3f;
    float m_coolTimeStrongAttack = 1f;
    float m_coolTimeCounter = 1f;
    float m_coolTimeDesh = 1f;
    
    bool m_hasWeapon;
    bool m_hasDesh;
    bool m_hasStorngAttack;
    bool m_hasCounter;

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected override void Init()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

        Util.SearchChild(m_childs, "sword").gameObject.SetActive(false);
    }

    protected override void OnUpdate()
    {
    }

    protected override void Move(Vector3 _moveDir)
    {
        transform.position += _moveDir * MoveSpeed * Time.deltaTime;
        transform.localScale = new Vector3(_moveDir.x, 1, 1);
    }

    protected override void DefaultAttack()
    {
    }
    
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


    void OnKeyboard()
    {
        //if (ESCMenu.g_pause == true)
        //{
        //    return;
        //}

        #region Player Move

        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);

            if (Input.GetKeyDown(KeyCode.LeftShift) && IsDeshCoolTime == false && m_hasDesh)
            {
                State = Define.PlayerState.Desh;
                IsDeshCoolTime = true;
                Desh(Vector3.left);
                Invoke("WaitmDeshCoolTime", m_coolTimeDesh);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);

            if (Input.GetKeyDown(KeyCode.LeftShift) && IsDeshCoolTime == false && m_hasDesh)
            {
                State = Define.PlayerState.Desh;
                IsDeshCoolTime = true;
                Desh(Vector3.right);
                Invoke("WaitmDeshCoolTime", m_coolTimeDesh);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        #endregion

        #region Player Action

        if (IsAttackCoolTime == false && m_hasWeapon == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                State = Define.PlayerState.WeakAttack;
                IsAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeWeakAttack);
            }
            if (Input.GetKeyDown(KeyCode.K) && IsAttackCoolTime == false && m_hasStorngAttack)
            {
                State = Define.PlayerState.StrongAttack;
                IsAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeStrongAttack);
            }
            if (Input.GetKeyDown(KeyCode.L) && IsAttackCoolTime == false && m_hasCounter)
            {
                State = Define.PlayerState.Counter;
                IsAttackCoolTime = true;
                Invoke("WaitAttackCoolTime", m_coolTimeCounter);
            }
        }

        #endregion
    }

    #region Cool Time Define

    void WaitAttackCoolTime()
    {
        IsAttackCoolTime = false;
    }

    void WaitmDeshCoolTime()
    {
        IsDeshCoolTime = false;
    }

    #endregion

    #region Player Move Define

    void Jump()
    {
        if (IsGround)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode2D.Impulse);
            IsGround = false;
        }
    }

    void Desh(Vector3 _moveDir)
    {
        m_rigidbody.velocity = Vector3.zero;
        transform.position += Vector3.right * m_deshRange * _moveDir.x;
    }

    #endregion

    #region Player Action Define

    void StrongAttack()
    {
    }

    void Counter()
    {
    }

    #endregion

    #region Trigger/Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGround = true;
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
                        GameObject go = Util.SearchChild(m_childs, "sword").gameObject;
                        go.SetActive(true);
                    }
                    break;
                case Define.ItemType.Desh:
                    m_hasDesh = true;
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
