using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : BasePlatform
{

    [SerializeField] private float m_quakeRange;
    [SerializeField] private float m_quakeTime;

    [SerializeField] float m_lifeTime;

    bool m_isQuake;
    [SerializeField] bool m_isDropped;
    
    protected override void Start()
    {
        base.Start();
        m_platformType = PlatformType.Drop;
    }

    private void Update()
    {
        if (m_isDropped) 
            StartCoroutine(nameof(Drop), m_lifeTime);    
    }

    private IEnumerator Drop(float _lifeTime = 0)
    {
        if (_lifeTime < 0) yield break;

        yield return new WaitForSeconds(_lifeTime);
        m_isQuake = true;

        yield return new WaitForSeconds(m_quakeTime);
        m_isQuake = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void QuakePlatform()
    {
        if (m_isQuake == false)
            return;

        transform.position += new Vector3(Random.Range(m_quakeRange * -1, m_quakeRange), Random.Range(m_quakeRange * -1, m_quakeRange), 0) * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.Player)))
            m_isDropped = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(nameof(Define.TagName.StrongAttack)))
            m_isDropped = true;
    }
}
