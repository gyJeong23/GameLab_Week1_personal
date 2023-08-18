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

    protected override bool IsGround { get; set; }
    protected override bool IsMove { get; set; }
    protected override bool IsAttackCoolTime { get; set; }
    protected override bool IsDeshCoolTime { get; set; }

    #endregion

    #region PrivateVariables

    [SerializeField] Define.PlayerActionState m_playerActionState;
    [SerializeField] Define.PlayerMoveState m_playerMoveState;

    [SerializeField] float m_deshPower;
    [SerializeField] float m_jumpPower;

    float m_coolTimeWeakAttack = 0.3f;
    float m_coolTimeStrongAttack = 1f;
    float m_coolTimeCounter = 1f;
    [SerializeField] float m_coolTimeDesh = 1f;
    
    bool m_hasWeapon;
    bool m_hasDesh;
    bool m_hasStorngAttack;
    bool m_hasCounter;

    Vector3 m_moveDir;

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected override void Init()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

        Util.SearchChild(m_childs, "sword").gameObject.SetActive(false);

        m_moveDir = Vector3.right;
    }

    protected override void OnUpdate()
    {
        switch (m_playerMoveState)
        {
            case Define.PlayerMoveState.Idle :
                {
                }
                break;
            case Define.PlayerMoveState.Move : 
                {
                    if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                    {
                        IsMove = false;
                        MoveState = Define.PlayerMoveState.Idle;
                    }    
                    
                    Move(m_moveDir);
                }
                break;
            case Define.PlayerMoveState.Desh : 
                {
                    Desh(m_moveDir);
                }
                break;
        }

    }

    protected override void Move(Vector3 _moveDir)
    {
        if (m_playerMoveState != Define.PlayerMoveState.Move)
            return;

        transform.position += _moveDir * MoveSpeed * Time.deltaTime; 
        transform.localScale = new Vector3(_moveDir.x, 1, 1);

    }

    protected override void DefaultAttack()
    {
    }
    
    #endregion 

    #region PrivateMethod
   
    private Define.PlayerMoveState MoveState
    {
        get { return m_playerMoveState; }
        set
        {
            m_playerMoveState = value;

            switch (m_playerMoveState)
            {
                case Define.PlayerMoveState.Idle:
                    { 
                        
                    }
                    break;
                case Define.PlayerMoveState.Move:
                    { 
                        IsMove = true;
                    }
                    break;
                case Define.PlayerMoveState.Desh:
                    {
                        StartCoroutine(nameof(WaitDeshCoolTime), m_coolTimeDesh);
                    }
                    break;
            }
        }
    }

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
                        m_animator.CrossFade("idle", 0.1f);
                    }
                    break;
                case Define.PlayerActionState.WeakAttack:
                    {
                        m_animator.CrossFade("WeakAttack", 0.1f, -1, 0);
                        StartCoroutine(nameof(WaitAttackCoolTime), m_coolTimeWeakAttack);
                    }
                    break;
                case Define.PlayerActionState.StrongAttack:
                    {
                        m_animator.CrossFade("StrongAttack", 0.1f, -1, 0);
                        StartCoroutine(nameof(WaitAttackCoolTime), m_coolTimeStrongAttack);
                    }
                    break;
                case Define.PlayerActionState.Counter:
                    {
                        m_animator.CrossFade("Counter", 0.1f, -1, 0);
                        StartCoroutine(nameof(WaitAttackCoolTime), m_coolTimeCounter);
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

        if (Input.GetKey(KeyCode.A))
        {
            MoveState = Define.PlayerMoveState.Move;

            m_moveDir = Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveState = Define.PlayerMoveState.Move;

            m_moveDir = Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_hasDesh)
        {
            MoveState = Define.PlayerMoveState.Desh;
        }
        //if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        //{
        //   
        //}

        #endregion
        #region Player Action

        if (IsAttackCoolTime == false && m_hasWeapon == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ActionState = Define.PlayerActionState.WeakAttack;
            }
            if (Input.GetKeyDown(KeyCode.K) && IsAttackCoolTime == false && m_hasStorngAttack)
            {
                ActionState = Define.PlayerActionState.StrongAttack;
            }
            if (Input.GetKeyDown(KeyCode.L) && IsAttackCoolTime == false && m_hasCounter)
            {
                ActionState = Define.PlayerActionState.Counter;
            }
        }

        #endregion
    }

    #region Cool Time Define

    IEnumerator WaitAttackCoolTime(float _coolTime)
    {
        IsAttackCoolTime = true;
        yield return new WaitForSeconds(_coolTime);

        IsAttackCoolTime = false;
        ActionState = Define.PlayerActionState.Idle;
    }
    IEnumerator WaitDeshCoolTime(float _coolTime)
    {
        IsDeshCoolTime = true;
        yield return new WaitForSeconds(_coolTime);

        IsDeshCoolTime = false;
        MoveState = Define.PlayerMoveState.Idle;
    }

    #endregion

    #region Player Move Define

    void Jump()
    {
        if (IsGround == false)
            return;

        m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode2D.Impulse);
        IsGround = false;
    }

    void Desh(Vector3 _moveDir)
    {
        if (IsDeshCoolTime) return;

        Vector3 destPos = new Vector3(transform.position.x + _moveDir.x * MoveSpeed, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position ,destPos, 0.7f);
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
