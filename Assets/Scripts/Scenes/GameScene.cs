using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    private static GameScene s_instance;
    public static GameScene Instance 
    {
        get { return s_instance; }
    }

    public Dictionary<int, Vector3> m_savePoints = new Dictionary<int, Vector3>();
    
    private int m_revivalCounter = 0;

    void Awake()
    {
        base.init();

        if (s_instance == null) { s_instance = this; }

        SceneType = Define.SceneType.GameScene;

        Vector3 startPos = Vector3.zero;
        m_savePoints.Add(m_revivalCounter,startPos);
    }

    public void Revival(GameObject _go)
    {
        if (m_savePoints == null)
            _go.transform.position = Vector3.zero;

        _go.transform.position = m_savePoints[m_revivalCounter];
    }

    public void SaveRevivalPoint(Vector3 _revivalPos)
    {
        Vector3 newPoint = _revivalPos;
        m_savePoints.Add(++m_revivalCounter, newPoint);
    }

}
