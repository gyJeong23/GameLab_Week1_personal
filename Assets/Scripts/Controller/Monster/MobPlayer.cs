using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPlayer : BaseMonster
{
    protected override void Start()
    {
        base.Start();

        #region monster status
        m_life = 10;

        m_thinkTime = 1f;
        m_defaultCoolTime = 1;
        m_specialCoolTime = 5;
        m_hitCoolTime = 1;

        m_detectRange = 10;
        m_defaultAttackRange = 4;
        m_specialAttackRange = 8;
        m_moveSpeed = 6;
        m_knockBackPower = 7;
        #endregion 
    }
}
