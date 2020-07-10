using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandUI : MonoBehaviour
{
    // 一つのフラグですぐにメニューが立ち上がるのは避けたいが書き方的にどうなのか

    // UIマネージャーの取得
    public UIManager m_uiManager;
    // 攻撃終了フラグ
    [SerializeField]
    private bool m_bAttackEnd = false;
    // 移動終了フラグ
    [SerializeField]
    private bool m_bMoveEnd = false;
    // 待機フラグ
    [SerializeField]
    private bool m_bWait = false;
    // ターンエンドフラグ
    [SerializeField]
    private bool m_bTurnEnd = false;
    // 移動中フラグ
    [SerializeField]
    private bool m_bMove = false;

    // 1つ前の移動フラグ
    bool m_bBeforeMove = false;

    [SerializeField]
    bool m_bLateTimeStart = false;// A押したときのタイミングをボタンUIクリックとずらす用
    float m_lateTime = 0.1f;
    float m_clickTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        GetLateTime();
        // Aを押した瞬間
        if(Input.GetButtonDown("Fire1") && !m_bLateTimeStart)
        {
            // 待機またはターン終了していないなら処理する （待機またはターン終了ならもうメニューが出ないように）
            if (!m_bWait && !m_bTurnEnd)
            {
                // メニューが何もアクティブになっていないかつ行動していなかったら
                if (!m_uiManager.GetCurrentMenu() && !m_bMove)
                {
                    //メインメニューを開く
                    m_uiManager.ActiveMenu(true, UIManager.MenuSelect.MainMenu);
                }
                //移動中なら
                else if (m_bMove && m_bBeforeMove)
                {
                    //移動の終了
                    m_bMove = false;
                }
            }
        }
        // Bを押した瞬間
        if (Input.GetButtonDown("Fire2"))
        {
            // 現在アクティブになっているメニューの非表示
            m_uiManager.ActiveMenu(false, m_uiManager.GetCurrentMenuNum());
        }
        // 現在のフレームで移動が始まったら
        if(!m_bBeforeMove && m_bMove)
        {
            Debug.Log("移動始め");
        }
        // 現在のフレームで移動が終わったら
        else if(m_bBeforeMove && !m_bMove)
        {
            Debug.Log("移動終わり");
            m_bMoveEnd = true;
        }
        m_bBeforeMove = m_bMove;
    }

    // A押したときのタイミングをボタンUIクリックとずらす用
    void GetLateTime()
    {
        if(m_bLateTimeStart)
        {
            m_clickTime += Time.deltaTime;
            if(m_clickTime >= m_lateTime)
            {
                m_clickTime = 0.0f;
                m_bLateTimeStart = false;
            }
        }
    }

    // 攻撃ボタンを押したとき
    public void OnAttackButtonClick()
    {
        if (!m_bAttackEnd)
        {
            // 攻撃メニューを開く
            m_uiManager.ActiveMenu(true, UIManager.MenuSelect.AttackMenu);
        }
    }

    // 移動ボタンを押したとき
    public void OnMoveButtonClick()
    {
        if (!m_bMoveEnd)
        {
            // 選べないボタンのカラー変更
            m_uiManager.SetEndButtonColor("MoveButton");
            //全メニューを非アクティブにする
            m_uiManager.NotActiveAllMenu();

            // 移動開始
            m_bMove = true;
            m_bLateTimeStart = true;
        }
    }

    // 待機ボタンを押したとき
    public void OnWaitButtonClick()
    {
        if(!m_bWait)
        {
            // 選べないボタンのカラー変更
            m_uiManager.SetEndButtonColor("WaitButton");
            //全メニューを非アクティブにする
            m_uiManager.NotActiveAllMenu();

            m_bLateTimeStart = true;
            // 待機に
            m_bWait = true;
            Debug.Log("待機しました");
        }
    }

    // ターン終了ボタンを押したとき
    public void OnTurnEndButtonClick()
    {
        if (!m_bTurnEnd)
        {
            // 選べないボタンのカラー変更
            m_uiManager.SetEndButtonColor("TurnEndButton");
            //全メニューを非アクティブにする
            m_uiManager.NotActiveAllMenu();

            m_bLateTimeStart = true;
            // ターン終了
            m_bTurnEnd = true;
            Debug.Log("ターン終了です");
        }
    }

    //攻撃メニューのbuttonボタンを押したとき
    public void OnButtonInAttackClick()
    {
        // 選べないボタンのカラー変更
        m_uiManager.SetEndButtonColor("AttackButton");
        //全メニューを非アクティブにする
        m_uiManager.NotActiveAllMenu();

        // 攻撃終了
        m_bAttackEnd = true;
        m_bLateTimeStart = true;
        Debug.Log("攻撃終了");
    }
}
