using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : BaseScene
{
    void Awake()
    {
        base.init();

        SceneType = Define.SceneType.StartScene;
    }

    private void Update()
    {
        if (Input.anyKey) 
            SceneManager.LoadScene(1);
    }
}
