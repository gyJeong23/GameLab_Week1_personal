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

    }
    public enum PlayerMoveState
    {
        Idle,
        Move,
        Jump,
    }

    public enum TagName
    {
        Default,
        Player,
        Ground,
        DropItem,
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
