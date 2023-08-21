using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Awake()
    {
        base.init();

        SceneType = Define.SceneType.GameScene;
    }

}
