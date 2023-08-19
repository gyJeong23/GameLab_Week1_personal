using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerActionState
    {
        Idle,
        DefualtAttack,
        SpecialAttack,
        Counter,
        Dash,
        Hit,
    }

    public enum MonsterActionState
    {
        Idle,
        Move,
        DefualtAttack,
        SpecialAttack,
        Hit,
    }

    public enum TagName
    {
        Default,
        Player,
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
