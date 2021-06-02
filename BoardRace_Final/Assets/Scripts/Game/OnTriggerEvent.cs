using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField]
    Material m_none;
    [SerializeField]
    AudioSource m_as;
    [SerializeField]
    AudioClip m_ac;

    GameObject m_gm;

    void Awake()
    {
        m_gm = GameObject.Find("GameManager");   
    }
   
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "None" || other.gameObject.tag == "GoldKey")
        {
            Destroy(this.transform.parent.gameObject);
        }

        else if(other.gameObject.tag == "SoldC" || other.gameObject.tag == "SoldP")
        {
            if (other.transform.GetChild(2).GetComponent<Data>().m_chickCount != 0)
            {
                for (int i = 0; i < other.transform.GetChild(2).GetComponent<Data>().m_chickList.Count; i++)
                {
                    Destroy(other.transform.GetChild(2).GetComponent<Data>().m_chickList[i]);
                    other.transform.GetChild(2).GetComponent<Data>().m_chickList.Clear();
                }
                other.transform.GetChild(2).GetComponent<Data>().m_chickCount = 0;
                other.transform.GetChild(2).GetComponent<Data>().m_chickValue = 30000;
            }

            if(other.transform.GetChild(2).GetComponent<Data>().m_chickenCount != 0)
            {
                for (int j = 0; j < other.transform.GetChild(2).GetComponent<Data>().m_chickenList.Count; j++)
                { 
                    Destroy(other.transform.GetChild(2).GetComponent<Data>().m_chickenList[j]);
                    other.transform.GetChild(2).GetComponent<Data>().m_chickenList.Clear();
                }
                other.transform.GetChild(2).GetComponent<Data>().m_chickenCount = 0;
                other.transform.GetChild(2).GetComponent<Data>().m_chickValue = 30000;
            }
           
            other.GetComponent<MeshRenderer>().material = m_none;

            if (other.gameObject.tag == "SoldP")
                m_gm.GetComponent<GameManager>().m_playersPanelCount--;
            else if (other.gameObject.tag == "SoldC")
                m_gm.GetComponent<GameManager>().m_COMsPanelCount--;

            other.tag = "None";
                Destroy(this.transform.parent.gameObject);
        }
    }
}
