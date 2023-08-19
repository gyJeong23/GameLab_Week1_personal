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

    public static Transform SearchChild(Transform _parent, string _name)
    {
        Transform[] _childs = _parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in _childs)
        {
            if (child.gameObject.name == _name)
                return child;
        }
        return null;
    }

    public bool IsAnimationEnd()
    {
        return true;
    }

    #endregion

    #region PrivateMethod
    #endregion
}
