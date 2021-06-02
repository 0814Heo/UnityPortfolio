using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToPanel : MonoBehaviour
{
    #region ======[public methods]======
    public List<GameObject> panels = new List<GameObject>();

    public Text m_nameText;
    public Text m_moneyText;    //잔액 표시
    public int m_steps;   //몇 칸 이동할지(주사위 값 받아옴)
    public int m_curPanelNum;   //현재 서있는 패널의 숫자
    public int m_money;

    public int m_moveCount;    //m_curPanelNum과 달리 한바퀴 돌았을 때 초기화되지 않음, 바퀴수 체크에 필요
    #endregion

    #region ======[methods]======
    [SerializeField]
    Animator m_anim;    //각 플레이어들 애니메이션
    [SerializeField]
    GameManager m_gm;
    [SerializeField]
    AudioSource m_as;
    [SerializeField]
    AudioClip m_acWalk;
    [SerializeField]
    Text m_moneyInfo;

    Vector3 m_nextPanel;

    bool m_isMoving;    //이동 중인지 아닌지
    #endregion

    void Start()
    {
        m_steps = 0;
        m_curPanelNum = 0;
        m_moveCount = 0;
        m_money = 1000000;
        m_isMoving = false;

        this.transform.position = panels[0].transform.position;
    }

    public IEnumerator Move()
    {
        if (m_isMoving)
            yield break;

        m_isMoving = true;

        yield return new WaitForSeconds(1f);
        Debug.Log("이동한다");

        while (m_steps > 0)
        {
            if (m_curPanelNum == 23) 
            {
                m_nextPanel = panels[0].transform.position; 

                while (MoveNextPanel(m_nextPanel)) 
                {
                    this.transform.SetParent(panels[0].transform);
                    yield return null;
                }
                m_as.PlayOneShot(m_acWalk);
                yield return new WaitForSeconds(0.1f);

                m_curPanelNum = 0; 
                m_steps--;
                m_money += 200000;
                ShowMoney();

                if (((m_moveCount+1) % 24 == 0))
                {
                    m_gm.m_curRoundNum++;
                }
                m_moveCount++;
            }

            else
            {
                m_nextPanel = panels[m_curPanelNum + 1].transform.position; 

                while (MoveNextPanel(m_nextPanel)) 
                {
                    this.transform.SetParent(panels[m_curPanelNum + 1].transform);
                    yield return null;
                }
                m_as.PlayOneShot(m_acWalk);

                if (m_curPanelNum == 22)
                    this.transform.LookAt(panels[0].transform.position);
                else if (m_curPanelNum == 23)
                    this.transform.LookAt(panels[1].transform.position);
                else
                    this.transform.LookAt(panels[m_curPanelNum + 2].transform.position);


                yield return new WaitForSeconds(0.1f);

                m_steps--;
                m_curPanelNum++;
                m_moveCount++;
            }
        }
        m_isMoving = false;
        m_gm.EventCheck(this.gameObject, Panel.instance.panels[m_curPanelNum].tag);
    }

    public IEnumerator ShowMoneyInfo(string pm, int money)
    {
        float alpha = 1.0f;
        float speed = 0;

        m_moneyInfo.text = pm + money.ToString();

        while (alpha > 0)
        {
            m_moneyInfo.color = new Color(1,1, 1, alpha);
            alpha -= Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }

        if (alpha == 0)
        {
            m_moneyInfo.color = new Color(1, 1, 1, 1.0f); ;
            m_moneyInfo.text = "";
        }
        yield return null;
    }

    bool MoveNextPanel(Vector3 goal)
    {
        return goal != (this.transform.position = Vector3.MoveTowards(this.transform.position, goal, 50f * Time.deltaTime));
    }

    public void ShowMoney()
    {
        if(m_money == 0)
            m_moneyText.text = "x" + 0.ToString();
        else
            m_moneyText.text = "x" + string.Format("{0:#,###}", m_money).ToString();
    }
}
