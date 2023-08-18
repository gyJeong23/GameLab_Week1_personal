using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region ProtectedVariables

    protected Rigidbody2D m_rigidbody;
    protected Animator m_animator;
    protected Transform[] m_childs;

    [SerializeField] protected float m_moveSpeed;

    protected abstract float MoveSpeed { get; set; }

    protected abstract bool IsGround { get; set; }
    protected abstract bool IsMove { get; set; }
    protected abstract bool IsAttackCoolTime { get; set; }
    protected abstract bool IsDeshCoolTime { get; set; }

    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = transform.GetComponentInChildren<Animator>();
        m_childs = gameObject.GetComponentsInChildren<Transform>();

        #region Property Init

        MoveSpeed = m_moveSpeed;

        #endregion

        Init();
    }

    protected void Update()
    {
        #region Property Update

        MoveSpeed = m_moveSpeed;

        #endregion

        OnUpdate();
    }

    protected abstract void Init();
    protected abstract void OnUpdate();
    protected abstract void Move(Vector3 _moveDir);
    protected abstract void DefaultAttack();

    #endregion

    #region PrivateMethod
    #endregion
}
