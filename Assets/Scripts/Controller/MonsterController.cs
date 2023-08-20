//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MonsterController : MonoBehaviour
//{
//    #region PublicVariables
//    #endregion

//    #region PrivateVariables

//    GameObject m_target;
//    Vector3 m_targetDir;

//    [SerializeField] float m_detectRange;
//    [SerializeField] float m_moveSpeed;
//    [SerializeField] float m_thinkTime;
//    [SerializeField] float m_thinkTimeCounter;

//    bool IsGround;

//    #endregion

//    #region PublicMethod
//    #endregion

//    #region PrivateMethod

//    private void Update()
//    {
//        m_targetDir = m_target.transform.position - transform.position;

//        if (m_targetDir.magnitude < m_detectRange && IsGround)
//        {
//            Move(m_targetDir.normalized, m_moveSpeed);
//        }
//    }

//    void Move(Vector3 _moveDir, float _moveSpeed)
//    {
//        transform.position += _moveDir * _moveSpeed * Time.deltaTime;

//        if (_moveDir.x < 0)
//            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
//        else
//            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
//    }


//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
//        {
//            IsGround = true;
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag(nameof(Define.TagName.Ground)))
//        {
//            IsGround = false;
//        }
//    }

//    #endregion

//}
