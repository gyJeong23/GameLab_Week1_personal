using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("s");


        if (collision.gameObject.CompareTag(nameof(Define.TagName.Player)))
        {
            GameScene.Instance.SaveRevivalPoint(collision.transform.position);
            Debug.Log("save");
            
            gameObject.SetActive(false);
        }
    }

    
    
}
