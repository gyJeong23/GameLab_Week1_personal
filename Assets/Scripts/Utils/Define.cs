using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerActionState
    {
        Idle,
        WeakAttack,
        StrongAttack,
        Counter,
    }
    public enum PlayerMoveState
    {
        Idle,
        Move,
        Desh,
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
        Desh,
        StrongAttack,
        Counter,
    }

}
