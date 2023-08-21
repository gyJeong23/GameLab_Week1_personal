using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMonster : BaseMonster
{
    protected override void Start()
    {
        base.Start();

        #region monster status
        m_life = 5;

        m_thinkTime = 1f;
        m_defaultCoolTime = 1;
        m_specialCoolTime = 3;
        m_hitCoolTime = 1.6f;

        m_detectRange = 15;
        m_defaultAttackRange = 2;
        m_specialAttackRange = 10;
        m_moveSpeed = 3;
        m_knockBackPower = 5;
        #endregion 
    }

    protected override IEnumerator AttackState(string _attack, float _coolTime)
    {
        if (m_isAttacking)
        {
            m_currentState = Define.MonsterState.Move;
            yield break;
        }

        if (_attack.Equals("SpecialAttack") && m_canSpecialAttack == false)
        {
            m_currentState = Define.MonsterState.Move;
            yield break;
        }

        if (_attack.Equals("SpecialAttack"))
            m_animator.SetTrigger("onSpecialAttack");
        else
            m_animator.SetTrigger("onDefaultAttack");

        m_isAttacking = true;

        // Dash Special
        if (_attack.Equals("SpecialAttack"))
        {
            m_rigidbody.velocity = Vector3.zero;

            yield return new WaitForSeconds(0.1f);
            m_rigidbody.velocity = m_moveDir * m_moveSpeed * 5;
        }

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
}
