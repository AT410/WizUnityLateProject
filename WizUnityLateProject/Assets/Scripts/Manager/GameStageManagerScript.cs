using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;

namespace DataBase {
    public enum EN_PlayerID
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    public enum EN_MenuState
    {
        ChoosePlayr,
        ChooseCommand,
        ChooseMap,
        ChooseEnemy,
        MovePlayer,
    }

    public enum EN_MapState
    {
        NotAvailable,//進行不能
        Nothing,    //何もない場所
        Damage,     //ダメージを受ける
        Event,      //イベントがある
    }

    public enum EN_MapCost
    {
        Default = -1,
        Forest = -2,
        NoEntry = -10,
    }

    [System.Serializable]
    public struct CharacterData
    {
        public uint JobID;
        public uint CharacterID;
        public uint Level;
        public uint EXP;
        public uint HP;
        public uint Attack;
        public uint Defense;
        public uint MoveDist;
        public uint AttackDist;
    };

    [System.Serializable]
    public struct CharacterCommandData
    {
        public bool bAttacked;
        public bool bMoved;
        public bool bWaiting;

        public CharacterCommandData(bool attacked, bool moved, bool waiting)
        {
            bAttacked = attacked;
            bMoved = moved;
            bWaiting = waiting;
        }
    }
}

public class GameStageManagerScript: MonoBehaviour
{
    [SerializeField]
    GameObject m_mapDataManagerObj;
    [SerializeField]
    GameObject m_playerMapDataManagerObj;
    [SerializeField]
    GameObject m_character;

    List<List<GameObject>> m_characterObj = new List<List<GameObject>>();

    [SerializeField]
    List<int> m_mapDate;

    List<Vector2> m_mapPosData;

    [Header("キャラクターデータ")] [SerializeField]
    List<DataBase.CharacterData> m_debugCharacterData;
    [SerializeField]
    List<uint> m_debugCharacterMapID;

    [SerializeField]
    List<int> m_mapCost;

    [Header("アクションの有効範囲")] [SerializeField]
    List<int> m_actionCostMap;

    [SerializeField]
    List<Vector2> m_canMoveMapID;

    //[SerializeField]
    public List<List<DataBase.CharacterData>> m_characterData = new List<List<DataBase.CharacterData>>();
    [SerializeField]
    List<List<DataBase.CharacterCommandData>> m_characterCommandData = new List<List<DataBase.CharacterCommandData>>();

    //キャラクターのいるマップデータ
    //m_characterMapID[A][B]
    // A = プレイヤーID、　B = 場にいるキャラクター
    public List<List<uint>> m_characterMapID = new List<List<uint>>();

    public CharacterCommandData m_nowCommandData = new CharacterCommandData();

    [Header("マネージャースクリプト")]
    public MapGeneratorScript m_sp_MapGenerator;
    [SerializeField]
    public UIManager m_sp_UIManager;

    //コマンドメニューが表示されている判定
    public bool m_bVisibleCommandMenuUI;

    [SerializeField]
    int m_maxPlayerNum;
    [SerializeField]
    int m_maxCharacterNum;

    //誰のターンか識別する
    public uint m_playerTurnNum;

    //ターン中のメニューの状態
    [SerializeField]
    uint m_menuStateNum;

    [SerializeField]
    uint m_choiceMapID;

    [SerializeField]
    uint m_choiceCharacterID;

    ///ゲッター・セッター
    //選ばれたマップID
    public uint ChoiceMapID
    {
        get { return m_choiceMapID; }
        set { m_choiceMapID = value; }
    }

    //メニューステータス
    public uint MenuStateNum
    {
        get { return m_menuStateNum; }
        set { m_menuStateNum = value; }
    }
   
    public CharacterCommandData GetCharacterCommandData()
    {
        return m_nowCommandData = m_characterCommandData[(int)m_playerTurnNum][(int)m_choiceCharacterID];
    }

    /// <summary>
    /// 処理
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        m_sp_MapGenerator = m_mapDataManagerObj.GetComponent<MapGeneratorScript>();

        m_mapPosData = m_sp_MapGenerator.m_mapPosData;

        CreateCharacterData();
        CreateCharacterCommandData();
        CreateCharacterMapID();
        CreateMapCost();
        DebugCreateMapData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //キャラクターデータをマネージャーに作成
    void CreateCharacterData()
    {
        for (int i = 0; i < m_maxPlayerNum; i++) {
            m_characterData.Add(new List<DataBase.CharacterData>());

            for (int j = 0; j < m_maxCharacterNum; j++)
            {
                //DataBase.CharacterData charaData = new DataBase.CharacterData();

                //デバッグ用のデータを追加
                m_characterData[i].Add(m_debugCharacterData[j]);
            }
        }
    }

    //キャラクターのコマンドデータをマネージャーに作成
    void CreateCharacterCommandData()
    {
        for (int i = 0; i < m_maxPlayerNum; i++)
        {
            m_characterCommandData.Add(new List<DataBase.CharacterCommandData>());

            for (int j = 0; j < m_maxCharacterNum; j++)
            {
                m_characterCommandData[i].Add(new DataBase.CharacterCommandData());
            }
        }
    }

    //キャラクターのマップデータをマネージャーに作成
    void CreateCharacterMapID()
    {
        for (int i = 0; i < m_maxPlayerNum; i++)
        {
            m_characterMapID.Add(new List<uint>());
            m_characterObj.Add(new List<GameObject>());

            for (int j = 0; j < m_maxCharacterNum; j++)
            {
                //デバッグ用のキャラクターの見た目を追加
                m_characterMapID[i].Add(m_debugCharacterMapID[j + i * m_maxPlayerNum]);
                GameObject obj = Instantiate(m_character, new Vector3(
                    m_sp_MapGenerator.m_mapPosData[(int)m_characterMapID[i][j]].x,
                    m_sp_MapGenerator.m_mapPosData[(int)m_characterMapID[i][j]].y, 0.0f), 
                    new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

                m_characterObj[i].Add(obj);
            }
        }
    }

    //マップのコストをマネージャーに作成
    void CreateMapCost()
    {
        for (int i = 0; i < m_sp_MapGenerator.m_mapPosData.Count; i++)
        {
            m_actionCostMap.Add((int)DataBase.EN_MapCost.Default);
            m_mapCost.Add((int)DataBase.EN_MapCost.Default);
        }
    }
    
    //
    void CreateCanMoveMapID()
    {
        var moveCost = m_characterData[(int)m_playerTurnNum][(int)m_choiceCharacterID].MoveDist;
        m_actionCostMap[(int)m_characterMapID[(int)m_playerTurnNum][(int)m_choiceCharacterID]] = (int)moveCost;

        ConfirmationMapID((int)m_choiceMapID, (int)moveCost);

        for (int i = 0; i < m_canMoveMapID.Count; i++)
        {
            Debug.Log(m_canMoveMapID[i]);
        }

        ResetMapCost();
    }

    /// <summary>
    /// 再起呼び出し用
    /// 
    /// int choiceMapID : 現在選ばれているマップID
    /// </summary>
    void ConfirmationMapID(int choiceMapID, int moveCost)
    {
        var mapColumn = Mathf.Sqrt(m_mapCost.Count);

        for (int i = moveCost; i >= 0; i--)
        {
            for (int j = 0; j < m_actionCostMap.Count; j++)
            {
                if (m_actionCostMap[j] == i)
                {
                    int upID = (int)choiceMapID - (int)mapColumn * (moveCost - i + 1);
                    int downID = (int)choiceMapID + (int)mapColumn * (moveCost - i + 1);
                    int rightID = (int)choiceMapID + 1 * (moveCost - i + 1);
                    int leftID = (int)choiceMapID - 1 * (moveCost - i + 1);

                    //選択されたマップの上
                    if (upID >= 0)
                    {
                        Debug.Log("up " + i);
                        j = SettingMapID(j, upID, i);
                    }

                    //選択されたマップの下
                    if (downID < m_mapCost.Count)
                    {
                        Debug.Log("down " + i);
                        j = SettingMapID(j, downID, i);
                    }

                    //選択されたマップの右
                    if (choiceMapID < m_mapCost.Count && j % mapColumn != 0)
                    {
                        Debug.Log("right "+ i);
                        j = SettingMapID(j, rightID, i);
                    }

                    //選択されたマップの左
                    if (choiceMapID >= 0 && j % mapColumn != 0)
                    {
                        j = SettingMapID(j, leftID, i);
                    }
                }
                //Debug.Log("J " + j);
            }
        }
    }

    int SettingMapID(int nowMapID, int nextMapID, int moveCost)
    {
        bool same = false;

        for (int i = 0; i < m_canMoveMapID.Count; i++)
        {
            if(m_canMoveMapID[i].x == nowMapID)
            {
                same = true;
                break;
            }
        }

        Debug.Log("ne" + nowMapID);

        if (m_actionCostMap[nowMapID] > 0 && !same)
        {
            Debug.Log(nextMapID);
            if (m_mapCost[nowMapID] != (int)DataBase.EN_MapCost.NoEntry)
            {
                //マップコストデータを更新
                m_canMoveMapID.Add(new Vector2((int)nowMapID, 0.0f));
                m_actionCostMap[nextMapID] = moveCost + m_mapCost[nowMapID];
                //moveCost += m_mapCost[nextMapID];
                //ConfirmationMapID(nextMapID, m_actionCostMap[nextMapID]);
                return 0;
            }
        }
        return nowMapID;
    }

    void ResetMapCost()
    {
        for(int i = 0; i < m_actionCostMap.Count; i++)
        {
            m_actionCostMap[i] = (int)DataBase.EN_MapCost.Default;
        }

        for(int i = 0; i < m_canMoveMapID.Count; i++)
        {
            m_canMoveMapID.Clear();
        }
    }

    void DebugCreateMapData()
    {
        for (int i = 0; i < m_sp_MapGenerator.m_mapPosData.Count; i++)
        {
            m_mapDate.Add((int)DataBase.EN_MapState.Nothing);
        }
    }

    public void AttackCharacter()
    {

    }

    public void MoveCharacter()
    {

    }

    public bool ConfirmationCharacter()
    {
        MenuStateNum = (int)DataBase.EN_MenuState.ChooseCommand;
        m_bVisibleCommandMenuUI = true;

        //味方の数だけループ
        for (int i = 0; i < m_characterMapID[(int)m_playerTurnNum].Count; i++)
        {
            //選択したマップIDと同じ場所に味方がいるか判定
            if (m_characterMapID[(int)m_playerTurnNum][i] == m_choiceMapID)
            {
                //味方のコマンド選択
                m_choiceCharacterID = (uint)i;
                m_nowCommandData = m_characterCommandData[(int)m_playerTurnNum][i];
                Debug.Log("味方");
                return true;
            }
        }
        //ターン終了以外選択不可
        m_nowCommandData.bAttacked = true;
        m_nowCommandData.bMoved = true;
        m_nowCommandData.bWaiting = true;

        Debug.Log("Enpty");
        return false;
    }

    public bool ConfirmationEvent()
    {

        return false;
    }

    /// <summary>
    /// 攻撃できるか確認する
    /// </summary>
    /// <returns>攻撃できる場合trueを返す</returns>
    public bool ConfirmationAttack()
    {

        Debug.Log("攻撃判定_GameStageMnager");
        return false;
    }

    /// <summary>
    /// 移動できるか確認する
    /// </summary>
    /// <returns>移動できる場合trueを返す</returns>
    public bool ConfirmationMove()
    {
        //侵入不可かどうか確かめる
        if (m_mapDate[(int)m_choiceMapID] != (int)DataBase.EN_MapState.NotAvailable)
        {
            for(int i = 0; i < m_canMoveMapID.Count; i++)
            {
                Debug.Log("m_canMoveMapID " + m_canMoveMapID[(int)m_choiceMapID]);
                if (m_canMoveMapID[i].x == (int)m_choiceMapID)
                {
                    var setPos = m_sp_MapGenerator.m_mapPosData[(int)m_choiceMapID];
                    m_characterObj[(int)m_playerTurnNum][(int)m_choiceCharacterID].transform.position = new Vector3(setPos.x, setPos.y, 0.0f);
                    m_characterMapID[(int)m_playerTurnNum][(int)m_choiceCharacterID] = m_choiceMapID;
                    ResetMapCost();
                    m_menuStateNum = (int)DataBase.EN_MenuState.ChoosePlayr;
                    return true;
                }
            }
        }
        return true;
    }

    public void SettingActionCostMap()
    {
        //キャラの数だけループ
        for (int i = 0; i < m_characterMapID.Count; i++)
        {
            for (int j = 0; j < m_characterMapID[i].Count; j++)
            {
                //m_actionCostMap[(int)m_characterMapID[i][j]] = (int)DataBase.EN_MapCost.NoEntry;

                //選択したマップIDと同じ場所に味方がいるか判定
                if ((int)m_characterMapID[i][j] == (int)DataBase.EN_MapCost.NoEntry)
                {
                }
            };
        }
        Debug.Log("移動判定_GameStageMnager");

        CreateCanMoveMapID();
    }

    public void WaitCharactor()
    {
        DataBase.CharacterCommandData a = m_characterCommandData[(int)m_playerTurnNum][(int)m_choiceCharacterID];
        m_characterCommandData[(int)m_playerTurnNum][(int)m_choiceCharacterID] = new DataBase.CharacterCommandData(true, true, true);
        m_nowCommandData = new DataBase.CharacterCommandData(true, true, true);
    }
}
