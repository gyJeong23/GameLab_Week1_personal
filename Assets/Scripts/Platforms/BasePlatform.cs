using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlatform : MonoBehaviour
{
    protected enum PlatformType
    {
        Default,
        Drop,
        Shorten,
        Rotate,
    }
    [SerializeField] protected PlatformType m_platformType = default;

    protected virtual void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

    }
}
