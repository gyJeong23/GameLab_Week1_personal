using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.UnKnown;

    void Awake()
    {
        init();
    }

    protected virtual void init()
    {
        GameObject go = GameObject.Find("@Scene");
        if (go == null)
        {
            go = new GameObject() { name = "@Scene" };
        }
    }
}
