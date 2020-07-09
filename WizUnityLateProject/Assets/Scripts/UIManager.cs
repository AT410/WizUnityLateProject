using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    // 今の状態だとメニューの種類 = 階層になってるかも

    // メニューの種類
    public enum MenuSelect { None, MainMenu, AttackMenu};

    // 親キャンバス
    [SerializeField]
    private Canvas m_ParentCanvas;
    // メインメニュー(1つ目)
    [SerializeField]
    private GameObject m_mainMenuPanel;
    // 攻撃用メニュー
    [SerializeField]
    private GameObject m_attackMenuPanel;
    // 現在アクティブのメニュー
    [SerializeField]
    private GameObject m_currentMenu;
    // ひとつ前アクティブのメニュー
    [SerializeField]
    private GameObject m_beforeMenu;
    // 現在のメニューの種類
    [SerializeField]
    private MenuSelect m_currentMenuNum = MenuSelect.None;

    private void Awake()
    {
        // 1番最初にメニューは隠しておく
        NotActiveAllMenu();
    }

    // メニューパネルの表示・非表示
    public void ActiveMenu(bool isActive, MenuSelect menu)
    {
        // 初めに選択されるよう設定されていたオブジェクトを初期化
        EventSystem.current.SetSelectedGameObject(null);
        m_beforeMenu = m_currentMenu;
        if (isActive)
        {
            // 表示
            // 現在アクティブのメニューを更新
            m_currentMenu = GetMenu(menu);
            m_currentMenuNum = menu;
            // メニューを表示
            GetMenu(menu).SetActive(true);
            // 操作できるボタンの設定
            ActiveMenuButton(true, menu);
            // 初めに選択されるオブジェクトの設定
            EventSystem.current.SetSelectedGameObject(GetMenu(menu).transform.GetChild(0).gameObject);
        }
        else
        {
        // 非表示
            // もともと何も入っていないなら終了
            if (!m_currentMenu) return;
            // メインメニュー（一つ目のメニュー）ならアクティブになっているメニューは無しに
            if (m_currentMenu == m_mainMenuPanel)
            {
                m_currentMenu = null;
            }
            else
            {
                // それ以外は自分のひとつ前のメニューをアクティブに
                m_currentMenu = GetMenu(menu - 1);
                // 操作ボタンと初期選択オブジェクトの設定
                ActiveMenuButton(false, menu);
                EventSystem.current.SetSelectedGameObject(GetMenu(menu - 1).transform.GetChild(0).gameObject);
            }
            // ひとつ前のメニューの種類
            m_currentMenuNum = menu - 1;
            // メニューの非表示
            GetMenu(menu).SetActive(false);
        }
    }

    // メニューパネルの取得
    public GameObject GetMenu(MenuSelect menu)
    {
        // メニューの種類別に取得
        switch(menu)
        {
            case MenuSelect.MainMenu:// メインメニュー
                return m_mainMenuPanel;
            case MenuSelect.AttackMenu:// 攻撃メニュー
                return m_attackMenuPanel;
            default:
                return null; ;//何もないならnull
        }
    }

    // メニューのボタン操作のアクティブ化
    private void ActiveMenuButton(bool isActive, MenuSelect menu)
    {
        // 見えているボタンオブジェクトの非アクティブ化
        foreach (var obj in FindObjectsOfType<Button>())
        {
            obj.interactable = false;
        }
        // アクティブ化がtrueなら
        if(isActive)
        {
            // 指定したメニューのアクティブ化
            foreach(Transform obj in GetMenu(menu).transform)
            {
                Button button = obj.GetComponent<Button>();
                if(button)
                {
                    button.interactable = isActive;
                }
            }
        }
        else
        {
            // アクティブ化がfalseなら最初のメニュー以外はひとつ前のボタンのアクティブ化

            if (menu == MenuSelect.MainMenu) return;
            foreach (Transform obj in GetMenu(menu - 1).transform)
            {
                Button button = obj.GetComponent<Button>();
                if (button)
                {
                    button.interactable = true;
                }
            }
        }
    }

    // 全メニュー非表示
    public void NotActiveAllMenu()
    {
        // メニューごとに非表示
        foreach (Transform child in m_ParentCanvas.transform)
        {
            child.gameObject.SetActive(false);
        }
        //現在アクティブ化メニューをnullに
        m_currentMenu = null;
        m_currentMenuNum = MenuSelect.None;
    }

    // ゲッター
    // 現在アクティブのメニュー
    public GameObject GetCurrentMenu()
    {
        return m_currentMenu;
    }
    // ひとつ前アクティブのメニュー
    public GameObject GetBeforeMenu()
    {
        return m_beforeMenu;
    }
    // ひとつ前アクティブのメニューセット
    public void SetBeforeMenu(GameObject menu)
    {
        m_beforeMenu = menu;
    }
    // 現在のメニューの種類
    public MenuSelect GetCurrentMenuNum()
    {
        return m_currentMenuNum;
    }

    // どうにか改良したいが仮でボタンの名前取ってきて色変更
    public void SetEndButtonColor(string buttonName)
    {
        foreach(var bt in FindObjectsOfType<Button>())
        {
            if(bt.gameObject.name == buttonName)
            {
                bt.gameObject.GetComponent<Image>().color = new Color(1, 0.2f, 0.2f, 1);
            }
        }

    }
    // 色のリセット
    public void ResetAllButtonColor()
    {
        foreach (var bt in FindObjectsOfType<Button>())
        {
            bt.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}
