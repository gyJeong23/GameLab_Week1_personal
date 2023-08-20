using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using Vector3 = UnityEngine.Vector3;

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

    [SerializeField] float m_dashPower;
    [SerializeField] float m_jumpPower;

    public GameObject[] m_heartUIs;

    int m_life = 5;

    float m_WeakAttackcoolTime = 0.2f;
    float m_SpecialAttackcoolTime = 0.8f;
    float m_CounterCoolTime = 0.8f;
    float m_DashCoolTime = 0.5f;
    float m_HitCoolTime = 1f;
    float m_knockBackPower = 5f;

    bool m_hasWeapon;
    bool m_hasdash;
    bool m_hasStorngAttack;
    bool m_hasCounter;
    bool m_isCounter;
    bool m_isHit;
    bool m_isDead;

    Vector3 m_moveDir;

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected override void Init()
    {
        Util.SearchChild(transform, "sword").gameObject.SetActive(false);

        m_moveDir = Vector3.right;

        //m_heartCounter = new Stack<bool>();
        //for (int i = 0; i < m_life; i++)
        //    m_heartCounter.Push(true);
    }

    protected override void OnUpdate()
    {
        OnKeyboard();

        if (IsDashing == true)
            Dash(m_moveDir, MoveSpeed);
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
                        m_animator.CrossFade("DefaultAttack", 0.1f);
                        StartCoroutine(Attack("DefaultAttack", m_WeakAttackcoolTime));
                    }
                    break;
                case Define.PlayerActionState.SpecialAttack:
                    {
                        m_animator.CrossFade("SpecialAttack", 0.1f);
                        StartCoroutine(Attack("SpecialAttack", m_SpecialAttackcoolTime));
                    }
                    break;
                case Define.PlayerActionState.Counter:
                    {
                        m_animator.CrossFade("Counter", 0.1f);
                        StartCoroutine(Attack("Counter", m_CounterCoolTime));
                    }
                    break;
                case Define.PlayerActionState.Dash:
                    {
                        m_animator.CrossFade("Dash", 0.1f);
                        StartCoroutine(nameof(DashOut), m_DashCoolTime);
                    }
                    break;
                case Define.PlayerActionState.Hit:
                    {
                        m_animator.CrossFade("Hit", 0.1f);
                        StartCoroutine(nameof(Hit), m_HitCoolTime);
                    }
                    break;
                case Define.PlayerActionState.Die:
                    {
                        m_animator.SetBool("isDead", true);
                        Die();
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
        if (m_isDead)
        {
            if (Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(0);

            else
                return;
        } 


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
            Jump();

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

    IEnumerator Counter(string _counter, float _coolTime)
    {
        m_isCounter = true;

        Transform counterTrigger = Util.SearchChild(transform, _counter);
        counterTrigger.gameObject.SetActive(true);

        yield return new WaitForSeconds(_coolTime);
        counterTrigger.gameObject.SetActive(false);

        ActionState = Define.PlayerActionState.Idle;
        m_isCounter = false;
    }

    void Jump()
    {
        if (IsGrounded == false) return;

        m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode2D.Impulse);
        IsGrounded = false;
    }

    void Dash(Vector3 _moveDir, float _moveSpeed)
    {
        Move(_moveDir, _moveSpeed * m_dashPower);
    }

    IEnumerator DashOut(float _coolTime)
    {
        IsDashing = true;
        yield return new WaitForSeconds(_coolTime);

        IsDashing = false;
    }

    IEnumerator Hit(float _cooltime)
    {
        m_isHit = true;

        //StartCoroutine(nameof(HitChangeBodyColor));

        yield return new WaitForSeconds(_cooltime);
        m_isHit = false;
    }

    //IEnumerator HitChangeBodyColor()
    //{
    //    SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
    //    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    //    {
    //        spriteRenderer.color = new Color(1f, 132f/255f, 132f/255f);
    //    }

    //    yield return new WaitForSeconds(0.1f);
    //    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    //    {
    //        spriteRenderer.color = Color.white;
    //    }
    //}

    void KnockBack(Vector3 _mosterVec)
    {
        Vector3 knockBackDir = new Vector3(_mosterVec.x, 0, 0).normalized;
        knockBackDir += Vector3.up;

        //m_rigidbody.AddForce(knockBackDir * m_knockBackPower, ForceMode2D.Impulse);
        m_rigidbody.velocity = knockBackDir * m_knockBackPower;
        Util.LimitVelocity2D(m_rigidbody, Vector3.one * m_knockBackPower);
    }

    void Die()
    {
        m_isDead = true;
        CircleCollider2D playerCollider;
        TryGetComponent<CircleCollider2D>(out playerCollider);
        playerCollider.isTrigger = true;
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

        if (other.CompareTag(nameof(Define.TagName.MonsterAttack)) && m_isDead == false)
        {
            Vector3 monsterToPlayerVec = transform.position - other.transform.position;

            //if (m_isCounter == true) // 수정 필요
            //{
            //    Rigidbody2D otherRb;
            //    other.gameObject.TryGetComponent<Rigidbody2D>(out otherRb);

            //    Vector3 counterDir = new Vector3(other.transform.position.x - transform.position.x, 0, 0).normalized;
            //    counterDir += Vector3.up;

            //    otherRb.AddForce(counterDir * 1f, ForceMode2D.Impulse);
            //    Util.LimitVelocity2D(otherRb, Vector3.one * 5f);

            //    Debug.Log("counter");
            //}
            //else 
            if (m_isHit == false)
            {
                KnockBack(monsterToPlayerVec);

                m_life--;
                if (m_life >= 0)
                    m_heartUIs[m_life].SetActive(false);

                if (m_life < 1)
                    ActionState = Define.PlayerActionState.Die;
                else
                    ActionState = Define.PlayerActionState.Hit;
            }
            //else
            //{
            //    KnockBack(monsterToPlayerVec);
            //}
        }
    }
    #endregion

}
#endregion
