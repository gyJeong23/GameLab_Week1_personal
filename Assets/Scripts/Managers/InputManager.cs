using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    #region PublicVariables

    public Action KeyAction = null;
    public Action KeyUpAction = null;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    public void OnUpdate()
    {
        //if (Input.anyKey == false && Input.anyKeyDown == false)
        //    return;

        if (KeyAction != null)
            KeyAction.Invoke();

        if (KeyUpAction != null)
            KeyUpAction.Invoke();

    }

    #endregion

    #region PrivateMethod
    #endregion
}
