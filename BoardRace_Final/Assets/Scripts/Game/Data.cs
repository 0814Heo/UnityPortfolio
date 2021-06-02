using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Data : MonoBehaviour
{
    public List<GameObject> m_chickList = new List<GameObject>();
    public List<GameObject> m_chickenList = new List<GameObject>();
    public List<GameObject> m_chickPos = new List<GameObject>();
    public List<GameObject> m_chickenPos = new List<GameObject>();

    public int m_chickValue; 
    public int m_chickCount;  
    public int m_chickenCount; 
    public int m_panelValue;  

    [SerializeField]
    GameObject m_chick;
    [SerializeField]
    GameObject m_chicken;

    void Start()
    {
        m_chickValue = 30000;
        m_chickCount = 0;
        m_chickenCount = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SpawnChick();
        }
    }
    public void SpawnChick()
    { 
        GameObject g = Instantiate(m_chick, m_chickPos[m_chickCount].transform) as GameObject;
        g.transform.localPosition = new Vector3(0, 0, 0);
        g.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        g.transform.LookAt(this.transform.parent.transform);
        g.name = "병아리" + m_chickCount.ToString();
        m_chickList.Add(g);
        m_chickCount++;
        m_chickValue += 30000;

        if(m_chickCount == 3)
        {
            for(int i = 0; i < m_chickList.Count; i++)
            {
                Destroy(m_chickList[i].gameObject);
            }
            m_chickList.Clear();
            m_chickCount = 0;
            GameObject gg = Instantiate(m_chicken, m_chickenPos[m_chickenCount].transform) as GameObject;
            gg.transform.localPosition = new Vector3(0, 0, 0);
            gg.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            gg.transform.LookAt(this.transform.parent.transform);
            m_chickenList.Add(gg);
            m_chickenCount++;
        }
    }
}
