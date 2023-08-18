using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
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

    GameObject m_target;
    Vector3 m_targetDir;

    [SerializeField] float m_detectRange;

    #endregion

    #region PublicMethod
    #endregion

    #region ProtectedMethod

    protected override void Init()
    {
        m_target = GameObject.FindWithTag(nameof(Define.TagName.Player));
    }

    protected override void OnUpdate()
    {
       
    }

    protected override void Move(Vector3 _moveDir)
    {
        transform.position += _moveDir * MoveSpeed * Time.deltaTime;

        if (_moveDir.x < 0)
            transform.localScale= new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale= new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    protected override void DefaultAttack()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region PrivateMethod

    private void LateUpdate()
    {
        m_targetDir = m_target.transform.position - transform.position;

        if (m_targetDir.magnitude < m_detectRange && IsGround)
        {
            Move(m_targetDir.normalized);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
        {
            IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
        {
            IsGround = false;
        }
    }

    #endregion

}
