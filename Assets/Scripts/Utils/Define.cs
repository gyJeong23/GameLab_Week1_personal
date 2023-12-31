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
        Dead,
    }

    public enum MonsterState
    {
        Idle,
        Move,
        DefualtAttack,
        SpecialAttack,
        Hit,
        Die,
        Dead,
    }

    public enum SceneType
    {
        UnKnown,
        GameScene,
        StartScene,
        ClearScene,
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
        MonsterStrongAttack,
        DeadLine,
        SavePoint
    }

    public enum ItemType
    { 
        Default,
        Weapon,
        Jump,
        Dash,
        StrongAttack,
        Counter,
        Heart,
        Clear,
    }

}
