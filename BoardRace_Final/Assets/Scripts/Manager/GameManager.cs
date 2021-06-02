using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    #region ======[public methods]======
    public int m_count; 
    public int m_curTurnNum;
    public int m_curRoundNum; 
    public int m_playersPanelCount;
    public int m_COMsPanelCount;
    public bool m_isTouch = true;
    public bool m_isEvent3 = false;

    public GameObject m_winObject; 
    public GameObject m_loseObject; 
    public GameObject m_player;  
    public GameObject m_com; 

    public Text m_diceInfo;   
    #endregion

    #region ======[methods]======
    MoveToPanel m_playerMove;
    MoveToPanel m_comMove;

    bool m_isEvent1 = false;
    bool m_isEvent2 = false;

    [SerializeField]
    SoundManager m_sm;
    [SerializeField]
    Text m_info;  
    [SerializeField]
    Text m_panelMoney;
    [SerializeField]
    Text m_chickMoney; 
    [SerializeField]
    Image m_red;
    [SerializeField]
    Image m_blue; 
    [SerializeField]
    Material m_colorRed;   
    [SerializeField]
    Material m_colorBlue;
    [SerializeField]
    GameObject m_buyPanel;
    [SerializeField]
    GameObject m_buyChick;
    [SerializeField]
    GameObject m_noMoney;
    [SerializeField]
    GameObject m_eventPopup;
    [SerializeField]
    Text m_event1Name;
    [SerializeField]
    Text m_event2Name;
    [SerializeField]
    Text m_event3Name;
    [SerializeField]
    Text m_event1;
    [SerializeField]
    Text m_event2;
    [SerializeField]
    Text m_event3;
    [SerializeField]
    GameObject m_First; 
    [SerializeField]
    ButtonManager m_btn;
    [SerializeField]
    AudioSource m_as;
    [SerializeField]
    AudioClip m_acMoney;
    #endregion

    void Start()
    {
        m_isTouch = false; 
        m_count = 0;
        m_curTurnNum = 1;
        m_playersPanelCount = 0;
        m_COMsPanelCount = 0;
        m_diceInfo.text = "주사위를 굴려주세요";

        m_buyPanel.SetActive(false);
        m_buyChick.SetActive(false);
        m_loseObject.SetActive(false);
        m_winObject.SetActive(false);
        m_noMoney.SetActive(false);
        m_eventPopup.SetActive(false);
        m_event1Name.enabled = false;
        m_event2Name.enabled = false;
        m_event3Name.enabled = false;
        m_event1.enabled = false;
        m_event2.enabled = false;
        m_event3.enabled = false;
        StartCoroutine(ShowUI(m_curTurnNum));
    }

    int i = 0;
    int j = 0;
    void Update()
    {
        if (m_player == null || m_com == null)
        {
            m_info.text = "플레이어가 인식되지 않음";
            m_diceInfo.text = "재시작 해주세요";
        }
           
        if (Input.GetKeyDown(KeyCode.R))
        {
            EndGame("Win");
        }

        else if (Input.GetKeyDown(KeyCode.B))
        {
            EndGame("Lose");
        }

        if (m_curTurnNum % 2 != 0 && m_isTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << 9))
                    {
                        m_isTouch = false;
                        Dice.instance.RollDice();
                    }
                }
            }
        }
    }

    IEnumerator Event1()  
    {
        if (m_isEvent1)
            yield break;

        m_isEvent1 = true;
        int result = 0;
        if (m_curTurnNum % 2 != 0)
        {
            for (int i = 0; i < 24; i++)
            {
                if (GameObject.Find("Panels").GetComponent<Panel>().panels[i].tag == "SoldP")
                {
                    result += GameObject.Find("Panels").GetComponent<Panel>().subPanels[i].GetComponent<Data>().m_chickCount + 
                        (GameObject.Find("Panels").GetComponent<Panel>().subPanels[i].GetComponent<Data>().m_chickenCount * 3) + 
                        (m_playersPanelCount);
                }
            }

            m_playerMove.m_money -= result * 5000;
            StartCoroutine(m_playerMove.ShowMoneyInfo("-", result * 5000));

            if(m_playerMove.m_money < 0)
                EndGame("Lose");
            else
                m_playerMove.ShowMoney();
        }

        else 
        {
            for (int i = 0; i < 24; i++)
            {
                if (GameObject.Find("Panels").GetComponent<Panel>().panels[i].tag == "SoldC")
                {
                    result += GameObject.Find("Panels").GetComponent<Panel>().subPanels[i].GetComponent<Data>().m_chickCount +
                       (GameObject.Find("Panels").GetComponent<Panel>().subPanels[i].GetComponent<Data>().m_chickenCount * 3) +
                       (m_COMsPanelCount);
                }
            }

            m_comMove.m_money -= result * 10000;
            StartCoroutine(m_comMove.ShowMoneyInfo("-", result * 10000));
            

            if (m_comMove.m_money < 0)
                EndGame("Win");
            else
                m_comMove.ShowMoney();
        }

        m_isEvent1 = false;
        ++m_curTurnNum; //턴 넘김
        StartCoroutine(ShowUI(m_curTurnNum));   //UI변경
    }
    IEnumerator Event2()
    {
        if (m_isEvent2)
            yield break;

        m_isEvent2 = true;

        if (m_curTurnNum % 2 != 0)
        {
            m_playerMove.m_moveCount += 23 - m_playerMove.m_curPanelNum;
            m_player.transform.position = m_playerMove.panels[0].transform.position;
            m_player.transform.LookAt(Panel.instance.panels[1].transform);
            m_playerMove.m_curPanelNum = 0;
            m_playerMove.m_money += 200000;
            m_playerMove.ShowMoney();
            StartCoroutine(m_playerMove.ShowMoneyInfo("+", 200000));
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }

        else if (m_curTurnNum % 2 == 0)
        {
            m_comMove.m_moveCount += 23 - m_comMove.m_curPanelNum;
            m_com.transform.position = m_comMove.panels[0].transform.position;
            m_com.transform.LookAt(Panel.instance.panels[1].transform);
            m_comMove.m_curPanelNum = 0;
            m_comMove.m_money += 200000;
            m_comMove.ShowMoney();
            StartCoroutine(m_comMove.ShowMoneyInfo("+", 200000));

            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }

        m_isEvent2 = false;
    }

    public IEnumerator ShowUI(int turnNum)
    {
        m_diceInfo.text = "";
        yield return new WaitForSeconds(1f);

        if (turnNum % 2 != 0)
        {
            if(m_player.name == "Red")
            {
                m_red.color = new Color(233/255f, 8/255f, 8/255f, 135/255f);
                m_blue.color = new Color(0, 0, 0, 132/255f);
            }

            else
            {
                m_red.color = new Color(0, 0, 0, 132/255f);
                m_blue.color = new Color(9/255f, 113/255f, 233/255f, 135/255f);
            }
            m_info.text = "플레이어 차례입니다";
            yield return new WaitForSeconds(0.1f);

            m_isTouch = true;
        }

        else
        {
            if (m_player.name == "Red")
            {
                m_red.color = new Color(0, 0, 0, 132/255f);
                m_blue.color = new Color(9 / 255f, 113 / 255f, 233 / 255f, 135 / 255f);
            }

            else
            {
                m_red.color = new Color(233 / 255f, 8 / 255f, 8 / 255f, 135 / 255f);
                m_blue.color = new Color(0, 0, 0, 132/255f);
            }
            m_info.text = "상대 플레이어 차례입니다";
            yield return new WaitForSeconds(0.1f);

            Dice.instance.RollDice();
        }
    }

    public void EndGame(string tag)
    {
        if (tag == "Win")
        {
            m_winObject.SetActive(true);
            m_sm.Win();
        }

        else if (tag == "Lose")
        {
            m_loseObject.SetActive(true);
            m_sm.Lose();
        }
    }

    public void EventCheck(GameObject player, string panelTag)
    {
        if (panelTag == "None")
        {
            if (player.tag == "Player")
            {
                m_buyPanel.SetActive(true);
                m_panelMoney.text = string.Format("{0:#,###}", Panel.instance.PanelValue(m_playerMove.m_curPanelNum)).ToString();
            }

            else if(player.tag == "COM")
            {
                if(m_comMove.m_money >= Panel.instance.PanelValue(m_comMove.m_curPanelNum))
                {
                    m_comMove.m_money -= Panel.instance.PanelValue(m_comMove.m_curPanelNum);
                    m_comMove.ShowMoney();
                    StartCoroutine( m_comMove.ShowMoneyInfo("-", Panel.instance.PanelValue(m_comMove.m_curPanelNum)));

                    if (m_com.name == "Red")
                    {
                        Panel.instance.panels[m_comMove.m_curPanelNum].GetComponent<MeshRenderer>().material = m_colorRed;
                        Panel.instance.panels[m_comMove.m_curPanelNum].tag = "SoldC";
                        m_COMsPanelCount++;
                    }

                    else
                    {
                        Panel.instance.panels[m_comMove.m_curPanelNum].GetComponent<MeshRenderer>().material = m_colorBlue;
                        Panel.instance.panels[m_comMove.m_curPanelNum].tag = "SoldC";
                        m_COMsPanelCount++;
                    }
                    ++m_curTurnNum; 
                    StartCoroutine(ShowUI(m_curTurnNum));
                }

                else
                {
                    ++m_curTurnNum;
                    StartCoroutine(ShowUI(m_curTurnNum));
                }
            }
        }

        else if(panelTag == "SoldC")
        {
            if (player.tag == "Player")
            {
                m_info.text = "COM에게 통행료를 냅니다.";
                if (m_playerMove.m_money >= Panel.instance.PanelPayment(m_playerMove.m_curPanelNum))
                {
                    m_as.PlayOneShot(m_acMoney);
                    m_playerMove.m_money -= Panel.instance.PanelPayment(m_playerMove.m_curPanelNum);
                    m_playerMove.ShowMoney();
                    StartCoroutine(m_playerMove.ShowMoneyInfo("-", Panel.instance.PanelPayment(m_playerMove.m_curPanelNum)));
                    m_comMove.m_money += Panel.instance.PanelPayment(m_playerMove.m_curPanelNum);
                    m_comMove.ShowMoney();
                    StartCoroutine(m_comMove.ShowMoneyInfo("+", Panel.instance.PanelPayment(m_playerMove.m_curPanelNum)));
                    ++m_curTurnNum;
                    StartCoroutine(ShowUI(m_curTurnNum));
                }

                else
                {
                    StartCoroutine(m_playerMove.ShowMoneyInfo("-", m_playerMove.m_money));
                    m_playerMove.m_money = 0;
                    m_playerMove.ShowMoney();
                    m_info.text = "통행료를 지불할 수 없습니다...";
                    EndGame("Lose");
                }
            }

            else if (player.tag == "COM")
            {
                if (m_comMove.m_money >= Panel.instance.subPanels[m_comMove.m_curPanelNum].GetComponent<Data>().m_chickValue)
                {
                    m_as.PlayOneShot(m_acMoney);
                    m_comMove.m_money -= Panel.instance.subPanels[m_comMove.m_curPanelNum].GetComponent<Data>().m_chickValue;
                    m_comMove.ShowMoney();
                    StartCoroutine(m_comMove.ShowMoneyInfo("-", Panel.instance.subPanels[m_comMove.m_curPanelNum].GetComponent<Data>().m_chickValue));
                    Panel.instance.subPanels[m_comMove.m_curPanelNum].GetComponent<Data>().SpawnChick();
                }
                    ++m_curTurnNum;
                    StartCoroutine(ShowUI(m_curTurnNum));
            }
        }

        else if(panelTag == "SoldP") 
        {
            if (player.tag == "Player")
            {
                m_buyChick.SetActive(true);
                m_chickMoney.text = string.Format("{0:#,###}", Panel.instance.subPanels[m_playerMove.m_curPanelNum].GetComponent<Data>().m_chickValue).ToString();
            }

            else if (player.tag == "COM")
            {
                m_info.text = "COM에게 통행료를 받습니다.";
                if(m_comMove.m_money >= Panel.instance.PanelPayment(m_comMove.m_curPanelNum))
                {
                    m_as.PlayOneShot(m_acMoney);
                    m_comMove.m_money -= Panel.instance.PanelPayment(m_comMove.m_curPanelNum);
                    m_playerMove.m_money += Panel.instance.PanelPayment(m_comMove.m_curPanelNum);
                    m_playerMove.ShowMoney();
                    m_comMove.ShowMoney();
                    StartCoroutine(m_comMove.ShowMoneyInfo("-", Panel.instance.PanelPayment(m_comMove.m_curPanelNum)));
                    StartCoroutine(m_playerMove.ShowMoneyInfo("+", Panel.instance.PanelPayment(m_comMove.m_curPanelNum)));
                    ++m_curTurnNum;
                    StartCoroutine(ShowUI(m_curTurnNum));
                }

                else
                {
                    StartCoroutine(m_comMove.ShowMoneyInfo("-", m_comMove.m_money));
                    m_comMove.m_money = 0;
                    m_comMove.ShowMoney();
                    m_info.text = "COM은 돈이 없습니다...!";
                    EndGame("Win");
                }
            }
        }

        else if(panelTag == "GoldKey")
        {
            GoldKeyEvent();
        }
        
        else
        {
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }
    }
    
    public void BuyPanel()
    {
        if(m_playerMove.m_money >= Panel.instance.PanelValue(m_playerMove.m_curPanelNum))
        {
            m_as.PlayOneShot(m_acMoney);
            m_playerMove.m_money -= Panel.instance.PanelValue(m_playerMove.m_curPanelNum);
            m_playerMove.ShowMoney();
            StartCoroutine(m_playerMove.ShowMoneyInfo("-", Panel.instance.PanelValue(m_playerMove.m_curPanelNum)));

            if (m_player.name == "Red")
            {
                Panel.instance.panels[m_playerMove.m_curPanelNum].GetComponent<MeshRenderer>().material = m_colorRed;
                Panel.instance.panels[m_playerMove.m_curPanelNum].tag = "SoldP";
                m_playersPanelCount++;
            }

            else
            {
                Panel.instance.panels[m_playerMove.m_curPanelNum].GetComponent<MeshRenderer>().material = m_colorBlue;
                Panel.instance.panels[m_playerMove.m_curPanelNum].tag = "SoldP";
                m_playersPanelCount++;
            }

            m_buyPanel.SetActive(false);
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }
        
        else
        {
            m_buyPanel.SetActive(false);
            m_noMoney.SetActive(true);
        }
    }

    public void BuyChick()
    {
        if (m_playerMove.m_money >= Panel.instance.subPanels[m_playerMove.m_curPanelNum].GetComponent<Data>().m_chickValue)
        {
            m_as.PlayOneShot(m_acMoney);
            m_playerMove.m_money -= Panel.instance.subPanels[m_playerMove.m_curPanelNum].GetComponent<Data>().m_chickValue;
            StartCoroutine(m_playerMove.ShowMoneyInfo("-", Panel.instance.subPanels[m_playerMove.m_curPanelNum].GetComponent<Data>().m_chickValue));
            Panel.instance.subPanels[m_playerMove.m_curPanelNum].GetComponent<Data>().SpawnChick();
            m_playerMove.ShowMoney();

            m_buyChick.SetActive(false);
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }

        else
        {
            m_buyChick.SetActive(false);
            m_noMoney.SetActive(true);
        }
    }

    public void ClosePopupInGame()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "panel_no")
        {
            m_buyPanel.SetActive(false);
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }

        else if(EventSystem.current.currentSelectedGameObject.name == "chick_no")
        {
            m_buyChick.SetActive(false);
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum));
        }

        else if(EventSystem.current.currentSelectedGameObject.name == "nomoney_ok")
        {
            m_noMoney.SetActive(false);
            ++m_curTurnNum;
            StartCoroutine(ShowUI(m_curTurnNum)); 
        }

        else if (m_event1.enabled == true && EventSystem.current.currentSelectedGameObject.name == "popup_ok") 
        {
            m_eventPopup.SetActive(false);
            StartCoroutine("Event1");
        }

        else if (m_event2.enabled == true && EventSystem.current.currentSelectedGameObject.name == "popup_ok") 
        {
            m_eventPopup.SetActive(false);
            StartCoroutine("Event2");
        }

        else if (m_event3.enabled == true && EventSystem.current.currentSelectedGameObject.name == "popup_ok")
        {
            m_eventPopup.SetActive(false);
            StartCoroutine(m_btn.Fire());
        }
    }

    void GoldKeyEvent()
    {
        //int i = Random.Range(0, 3);
        int i = 2;
        m_info.text = "이벤트 발생!";
        m_diceInfo.text = "";

        if (i == 0)
        {
            m_eventPopup.SetActive(true);
            m_event1.enabled = true;
            m_event2.enabled = false;
            m_event3.enabled = false;
            m_event1Name.enabled = true;
            m_event2Name.enabled = false;
            m_event3Name.enabled = false;
        }

        else if (i == 1)
        {
            m_eventPopup.SetActive(true);
            m_event1.enabled = false;
            m_event2.enabled = true;
            m_event3.enabled = false;
            m_event1Name.enabled = false;
            m_event2Name.enabled = true;
            m_event3Name.enabled = false;
        }

        else if (i == 2)
        {
            m_eventPopup.SetActive(true);
            m_event1.enabled = false;
            m_event2.enabled = false;
            m_event3.enabled = true;
            m_event1Name.enabled = false;
            m_event2Name.enabled = false;
            m_event3Name.enabled = true;
        }
    }

    #region =======[select_Player]=======
    public void Select()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "btn_Red")
        {
            m_red.color = new Color(233 / 255f, 8 / 255f, 8 / 255f, 135 / 255f);
            m_blue.color = new Color(0, 0, 0, 132 / 255f);
        }

        else if(EventSystem.current.currentSelectedGameObject.name == "btn_Blue")
        {
            GameObject temp = m_player;
            m_player = m_com;
            m_com = temp;
            
            m_red.color = new Color(0, 0, 0, 132 / 255f);
            m_blue.color = new Color(9 / 255f, 113 / 255f, 233 / 255f, 135 / 255f);
        }

        m_player.tag = "Player";
        m_playerMove = m_player.GetComponent<MoveToPanel>();
        m_playerMove.m_nameText.text = "Player";
        m_com.tag = "COM";
        m_comMove = m_com.GetComponent<MoveToPanel>();
        m_com.GetComponent<MoveToPanel>().m_nameText.text = "COM";

        m_playerMove.m_moneyText.text = "x" + string.Format("{0:#,###}", 1000000).ToString();
        m_comMove.m_moneyText.text = "x" + string.Format("{0:#,###}", 1000000).ToString();

        if(m_player.tag == "Player" && m_com.tag == "COM")
            m_First.SetActive(false);
    }
    #endregion
}