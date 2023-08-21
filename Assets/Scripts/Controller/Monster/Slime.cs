using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : BaseMonster
{
    protected override void Start()
    {
        base.Start();

        #region monster status
        m_life = 2;
        
        m_thinkTime = 1;
        m_defaultCoolTime = 1;
        m_specialCoolTime = 3;
        m_hitCoolTime = 1;
        
        m_detectRange = 5;
        m_defaultAttackRange = 2;
        m_specialAttackRange = 3;
        m_moveSpeed = 3;
        m_knockBackPower = 5;
        #endregion 
    }
}
