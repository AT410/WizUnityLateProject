using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCursorScript : MonoBehaviour
{
    [Header("オブジェクト")]
    [SerializeField] GameObject m_mapDataManagerObj;
    [SerializeField] GameObject m_cursorObj;
    [SerializeField] Image m_menuCursorObj;

    [Header("スクリプト")]
    [SerializeField] GameStageManagerScript m_sp_gameStageManager;

    [Header("マップデータ")]
    [SerializeField] uint m_maxMapNum;
    [SerializeField] uint m_moveMapCursorNum;
    [SerializeField] uint m_moveCommandCursorNum;
    [SerializeField] List<Vector2> m_mapPosData;

    [Header("クールタイム")]
    [SerializeField] float m_inputTimer;
    [SerializeField] float m_maxInputTimer;
    bool m_bCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        m_maxMapNum = (uint)m_mapDataManagerObj.GetComponent<MapGeneratorScript>().m_mapPosData.Count;
        m_moveMapCursorNum = m_maxMapNum / 2;
        m_mapPosData = m_mapDataManagerObj.GetComponent<MapGeneratorScript>().m_mapPosData;
        m_cursorObj.transform.position = new Vector3(m_mapPosData[(int)m_moveMapCursorNum].x, m_mapPosData[(int)m_moveMapCursorNum]. y, 0.0f);

        m_sp_gameStageManager = GameObject.Find("GameStageManager").GetComponent<GameStageManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        InputHunder();
        MoveCoolTime();
    }

    //
    void InputHunder()
    {
        float inputHorizon = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        bool bInput = inputHorizon == 0 && inputVert == 0;
        uint cursorNum = m_moveMapCursorNum;
        uint mapIdRad = (uint)Mathf.Sqrt(m_maxMapNum);

        PushA();

        bool bVisibleCommandUI = m_sp_gameStageManager.m_bVisibleCommandMenuUI;

        if (!m_bCoolTime && !bVisibleCommandUI)
        {
            if (inputHorizon != 0)
            {
                cursorNum += (uint)inputHorizon;
                MoveMapCursor(cursorNum);
            }

            if (inputVert != 0)
            {
                cursorNum -= (uint)inputVert * mapIdRad;
                MoveMapCursor(cursorNum);
            }
        }

        if (bInput)
        {
            m_bCoolTime = false;
            m_inputTimer = 0;
        }
    }

    //カーソル移動のクールタイム処理
    void MoveCoolTime()
    {
        if (m_bCoolTime && m_inputTimer <= m_maxInputTimer)
        {
            m_inputTimer += Time.deltaTime;
        }
        else
        {
            m_bCoolTime = false;
        }
    }

    //カーソルを動かす処理
    void MoveMapCursor(uint cursorNum)
    {
        if (m_mapPosData.Count > cursorNum && 0 <= cursorNum)
        {
            m_moveMapCursorNum = cursorNum;
            m_cursorObj.transform.position = new Vector3(m_mapPosData[(int)m_moveMapCursorNum].x, m_mapPosData[(int)m_moveMapCursorNum].y, 0.0f);
        }
        else
        {
            //SEを鳴らす
        }
        m_bCoolTime = true;
        m_inputTimer = 0;
    }

    //Aボタンを押したとき
    void PushA()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            if (m_sp_gameStageManager.MenuStateNum == (int)DataBase.EN_MenuState.ChoosePlayr)
            {
                Debug.Log("データ転送_ChoosePlayr");
                m_sp_gameStageManager.ChoiceMapID = m_moveMapCursorNum;
                m_sp_gameStageManager.ConfirmationCharacter();
            }

            if (m_sp_gameStageManager.MenuStateNum == (int)DataBase.EN_MenuState.ChooseEnemy)
            {
                Debug.Log("データ転送_ChooseEnemy");

            }

            if (m_sp_gameStageManager.MenuStateNum == (int)DataBase.EN_MenuState.ChooseMap)
            {
                Debug.Log("データ転送_ChooseMap");

            }
        }
    }
}
