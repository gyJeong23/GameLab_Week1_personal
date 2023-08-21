using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StrongMonster : BaseMonster
{
    protected override void Start()
    {
        base.Start();

        #region monster status
        m_life = 8;
        
        m_thinkTime = 1f;
        m_defaultCoolTime = 1;
        m_specialCoolTime = 5;
        m_hitCoolTime = 1.5f;
        
        m_detectRange = 10;
        m_defaultAttackRange = 4;
        m_specialAttackRange = 6;
        m_moveSpeed = 5;
        m_knockBackPower = 3;
        #endregion 
    }
}
