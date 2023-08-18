using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : BaseStage
{
    
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    public override void StartStage()
    {
        base.InitStage();
        Platforms = new GameObject { name = "Platforms" };

        MakePlatform();
    }

    void MakePlatform()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Stages/Stage2");
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(Platforms.transform);
    }

   

    #endregion


}
