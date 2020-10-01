using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRangeGenerator : MonoBehaviour
{
    [SerializeField] GameObject m_tilePrefab;
    List<GameObject> m_tileObj = new List<GameObject>();
    GameStageManagerScript m_sp_gameStageManager;

    MapGeneratorScript m_sp_mapGenerator;

    bool m_isVisibleTile;
     
    // Start is called before the first frame update
    void Start()
    {
        m_sp_gameStageManager = GameObject.Find("GameStageManager").GetComponent<GameStageManagerScript>();
        m_sp_mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGeneratorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //CreateTile();
    }

    public void CreateTile()
    {
        while(m_tileObj.Count < m_sp_gameStageManager.m_canMoveMapID.Count)
        {
            m_tileObj.Add(Instantiate(m_tilePrefab));
        }
    }

    public void GeneratActionRange()
    {
        if (!m_isVisibleTile)
        {
            var mapPosData = m_sp_mapGenerator.m_mapPosData;
            var canActionMapID = m_sp_gameStageManager.m_canMoveMapID;

            for (int i = 0; i < m_sp_gameStageManager.m_canMoveMapID.Count; i++)
            {
                m_tileObj[i].SetActive(true);
                m_tileObj[i].transform.position =
                    new Vector3(
                        mapPosData[(int)canActionMapID[i].x].x,
                        mapPosData[(int)canActionMapID[i].x].y,
                        0.0f);
            }
            m_isVisibleTile = true;
        }
    }

    public void InVisivleTile()
    {
        if (m_isVisibleTile)
        {
            for (int i = 0; i < m_tileObj.Count; i++)
            {
                m_tileObj[i].SetActive(false);
            }
            m_isVisibleTile = false;
        }
    }
}
