using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    public static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Manager
    InputManager m_input = new InputManager();
    public static InputManager Input { get { return s_instance.m_input; } }
    #endregion

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    static void Init()
    {
        if (s_instance == null)
        { 
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        } 
    }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        m_input.OnUpdate();
    }

    #endregion
}
