using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.SceneManagement;

public class ClearScene : BaseScene
{
    void Awake()
    {
        base.init();

        SceneType = Define.SceneType.ClearScene;
    }

    private void Update()
    {
        if (Input.anyKey)
            SceneManager.LoadScene(0);
    }
}
