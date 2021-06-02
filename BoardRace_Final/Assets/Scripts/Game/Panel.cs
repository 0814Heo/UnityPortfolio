using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public static Panel instance;

    public List<GameObject> panels = new List<GameObject>();
    public List<GameObject> subPanels = new List<GameObject>();

    [SerializeField]
    GameObject m_subPanel;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < 24; i++)
        {
            //panels[i].transform.LookAt(new Vector3(0, panels[i].transform.position.y, 0));
            panels[i].transform.LookAt(new Vector3(transform.parent.transform.position.x, panels[i].transform.position.y, transform.parent.transform.position.z));
            if (i == 0)
                panels[i].tag = "StartPos";
            else if (i == 6 || i == 12 || i == 18)
                panels[i].tag = "GoldKey";
            else
                panels[i].tag = "None";

            GameObject g = Instantiate(m_subPanel, panels[i].transform);
            g.transform.localPosition = new Vector3(0, 0, 0.4f);
            if (i == 0)
                g.transform.localScale = new Vector3(0.8571427f, 0.8571427f, 0.8571427f);
            subPanels.Add(g);
        }
        //Debug.Log("패널 " + m_panelCount + "개 리스트에 담김");
    }

    public int PanelValue(int panelNum)
    {
        if (panelNum < 6)
            return 50000;

        else if (panelNum < 12)
            return 100000;

        else if (panelNum < 18)
            return 200000;

        else
            return 300000;
    }

    public int PanelPayment(int panelNum)
    {
        if (panelNum < 6)
            return (10000 + (subPanels[panelNum].GetComponent<Data>().m_chickValue / 10))*2;

        else if (panelNum < 12)
            return (20000 + (subPanels[panelNum].GetComponent<Data>().m_chickValue / 10))*2;

        else if (panelNum < 18)
            return (30000 + (subPanels[panelNum].GetComponent<Data>().m_chickValue / 10))*2;

        else
            return (40000 + (subPanels[panelNum].GetComponent<Data>().m_chickValue / 10))*2;

    }
}
