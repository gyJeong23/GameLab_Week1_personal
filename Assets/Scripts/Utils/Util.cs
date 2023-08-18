using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    public static Transform SearchChild(Transform[] _childs, string _name)
    {
        foreach (Transform child in _childs)
        {
            if (child.gameObject.name == _name)
                return child;
        }
        return null;
    }

    #endregion

    #region PrivateMethod
    #endregion
}
