using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region PublicVariables

    public Action KeyAction = null;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    public void OnUpdate()
    {
        if (Input.anyKey == false)
            return;

        if (KeyAction != null)
            KeyAction.Invoke();

        
    }

    #endregion

    #region PrivateMethod

    

    #endregion
}
