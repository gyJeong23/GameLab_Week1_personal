using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables

    Transform m_player;

    [SerializeField] Vector3 m_offest;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() ;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(m_player.position.x, m_player.position.y, m_offest.z);
    }

    #endregion
}
