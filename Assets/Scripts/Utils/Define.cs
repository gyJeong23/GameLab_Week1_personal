using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerState
    {
        Idle,
        DefualtAttack,
        SpecialAttack,
        Counter,
        Dash,
        Hit,
        Die,
    }

    public enum MonsterState
    {
        Idle,
        Move,
        DefualtAttack,
        SpecialAttack,
        Hit,
        Die,
    }

    public enum TagName
    {
        Default,
        Player,
        WeakAttack,
        StrongAttack,
        Ground,
        DropItem,
        Monster,
        MonsterAttack,
    }

    public enum ItemType
    { 
        Default,
        Weapon,
        Dash,
        StrongAttack,
        Counter,
    }

}
