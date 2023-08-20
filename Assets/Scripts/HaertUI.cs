using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaertUI : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    [SerializeField] PlayerController m_player;

    Vector3 m_UIpos;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void Start()
    {
        m_player = GetComponent<PlayerController>();
        m_UIpos = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(m_UIpos);
    }

    #endregion
}
