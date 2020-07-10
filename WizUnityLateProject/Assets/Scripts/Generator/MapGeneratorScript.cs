using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class MapGeneratorScript : MonoBehaviour
{
    //
    [SerializeField] List<string> m_textureName;
    //生成させるオブジェクトの保存
    [SerializeField] List<GameObject> m_mapObj;
    //
    [SerializeField] List<uint> m_mapStateNum;

    //マップの半径
    [SerializeField] uint m_cellSizeHalf;

    //生成を始めるポジション
    [SerializeField] Vector2 m_firstGeneratPos;
    public List<Vector2> m_mapPosData;

    void Awake()
    {
        MapGenerat();

    }

    // Start is called before the first frame update
    void Start()
    {
        LoadXML();
    }

    void LoadXML()
    {

    }

    void MapGenerat()
    {
        int x = (int)Mathf.Sqrt(25.0f);
        int y = (int)Mathf.Sqrt(25.0f);

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                //ポジジョンの設定
                Vector3 setPos = new Vector3(m_firstGeneratPos.x + (m_cellSizeHalf * j), m_firstGeneratPos.y - (m_cellSizeHalf * i), 0.0f);
                //回転情報の設定
                Quaternion setQuat = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                //オブジェクト生成
                Instantiate(m_mapObj[(int)m_mapStateNum[i + j]], setPos, setQuat);
                m_mapPosData.Add(new Vector2(setPos.x, setPos.y));
            }
        }
    }
}
