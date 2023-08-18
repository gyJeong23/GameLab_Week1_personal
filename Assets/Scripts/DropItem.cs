using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    #region PublicVariables

    public Define.ItemType m_itemType = Define.ItemType.Weapon;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
        { 
            Destroy(gameObject);
        }
    }

    #endregion
}
