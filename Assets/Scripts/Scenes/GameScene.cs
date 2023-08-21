using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : BaseScene
{
    private static GameScene s_instance;
    public static GameScene Instance
    {
        get { return s_instance; }
    }

    public Dictionary<int, Vector3> m_savePoints = new Dictionary<int, Vector3>();

    public GameObject m_gameover;

    private int m_revivalCounter = 0;

    private bool m_isGameOver;

    void Awake()
    {
        base.init();

        if (s_instance == null) { s_instance = this; }

        SceneType = Define.SceneType.GameScene;

        Vector3 startPos = Vector3.zero;
        m_savePoints.Add(m_revivalCounter, startPos);


        m_gameover = GameObject.Find("GameOver Panel");
        m_gameover.SetActive(false);
    }

    public void Revival(GameObject _go)
    {
        if (m_savePoints == null)
            _go.transform.position = Vector3.zero;

        if (m_gameover && Input.GetKey(KeyCode.R)) 
            SceneManager.LoadScene(1);

        _go.transform.position = m_savePoints[m_revivalCounter];
    }

    public void SaveRevivalPoint(Vector3 _revivalPos)
    {
        Vector3 newPoint = _revivalPos;
        m_savePoints.Add(++m_revivalCounter, newPoint);
    }

    public void LoadClearScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ActiveGameOverPanel(Vector3 _playerPos)
    {
        m_isGameOver = true;
        m_gameover.SetActive(true);

        m_gameover.transform.position = _playerPos;
        Debug.Log(m_gameover.transform.position);
    }
}
