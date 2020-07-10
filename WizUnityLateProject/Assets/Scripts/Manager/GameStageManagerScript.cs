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
    }
}

public class GameStageManagerScript: MonoBehaviour
{
    [SerializeField]
    GameObject m_mapDataManagerObj;
    [SerializeField]
    GameObject m_playerMapDataManagerObj;

    [SerializeField]
    List<int> m_mapDate;

    [Header("キャラクターデータ")] [SerializeField]
    List<DataBase.CharacterData> m_debugCharacterData;
    [SerializeField]
    List<uint> m_debugCharacterMapID;

    
    [SerializeField]
    List<List<DataBase.CharacterData>> m_characterData = new List<List<DataBase.CharacterData>>();
    [SerializeField]
    List<List<DataBase.CharacterCommandData>> m_characterCommandData;

    //キャラクターのいるマップデータ
    List<List<uint>> m_characterMapID = new List<List<uint>>();

    [Header("マネージャースクリプト")]
    public MapGeneratorScript m_sp_MapDataManager;
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

    /// <summary>
    /// 処理
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        m_sp_MapDataManager = m_mapDataManagerObj.GetComponent<MapGeneratorScript>();
        CreateCharacterData();
        CreateCharacterCommandData();
        CreateCharacterMapID();
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
        m_characterCommandData = new List<List<DataBase.CharacterCommandData>>();
        for (int i = 0; i < m_maxPlayerNum; i++)
        {
            m_characterCommandData.Add(new List<DataBase.CharacterCommandData>());

            for (int j = 0; j < m_maxCharacterNum; j++)
            {
                //DataBase.CharacterData charaData = new DataBase.CharacterData();

                //デバッグ用のデータを追加
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

            for (int j = 0; j < m_maxCharacterNum; j++)
            {
                //デバッグ用のデータを追加
                m_characterMapID[i].Add(m_debugCharacterMapID[j + i * m_maxPlayerNum]);
            }
        }
    }

    void DebugCreateMapData()
    {
        for (int i = 0; i < m_sp_MapDataManager.m_mapPosData.Count; i++)
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

        //侵入不可かどうか確かめる
        if (m_mapDate[(int)m_choiceMapID] != (int)DataBase.EN_MapState.NotAvailable)
        {
            //味方の数だけループ
            for (int i = 0; i < m_characterMapID[(int)m_playerTurnNum].Count; i++)
            {
                //選択したマップIDと同じ場所に味方がいるか判定
                if (m_characterMapID[(int)m_playerTurnNum][i] == m_choiceMapID)
                {
                    //味方のコマンド選択
                    m_choiceCharacterID = (uint)i;
                    Debug.Log("味方");
                    return true;
                }
            }
        }

        //ターン終了以外選択不可

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
        Debug.Log("移動判定_GameStageMnager");
        return false;
    }
}
