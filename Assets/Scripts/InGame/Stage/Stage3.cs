using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : BaseStage
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
        
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Stages/Stage3");
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(Platforms.transform);
    }

    #endregion


}
