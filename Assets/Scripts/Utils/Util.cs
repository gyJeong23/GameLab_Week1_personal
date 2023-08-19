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

    public static void LimitVelocity2D(Rigidbody2D _rb, Vector3 _limit)
    { 
        if (_rb.velocity.magnitude > _limit.magnitude) 
        {
            _rb.velocity = _limit;
        }
    }

    #endregion

    #region PrivateMethod
    #endregion
}
