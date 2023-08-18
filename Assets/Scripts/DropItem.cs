using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    #region PublicVariables

    Collider2D m_collider2D;
    Rigidbody2D m_rigidbody2D;

    public Define.ItemType m_itemType = Define.ItemType.Weapon;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void Start()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
        {
            m_collider2D.isTrigger = true;
            m_rigidbody2D.isKinematic = true;
        }
    }

    #endregion
}
