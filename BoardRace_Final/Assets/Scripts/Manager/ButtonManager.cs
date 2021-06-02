using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_helpInfo;
    [SerializeField]
    GameObject m_empty;
    [SerializeField]
    GameObject m_vol;
    [SerializeField]
    GameObject m_particle;
    [SerializeField]
    AudioSource m_as;
    [SerializeField]
    AudioClip m_acVol;
    [SerializeField]
    GameManager m_gm;

    int num1, num2, num3;
    int[] arrNum = { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 13,
                    14, 15, 16, 17, 19, 20, 21, 22, 23 };

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ShowInfo()
    {
        if (m_helpInfo.activeSelf)
        {
            m_helpInfo.SetActive(false);
        }
        else
        {
            m_helpInfo.SetActive(true);
        }
    }

    void Update()
    { 
      if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("Fire");
        }
    }
    public IEnumerator Fire()
    {
        if (m_gm.m_isEvent3)
            yield break;

        m_gm.m_isEvent3 = true;

        print("화산폭발");

        GameObject p = Instantiate(m_particle, this.transform.position, Quaternion.identity, this.transform) as GameObject;
        p.GetComponent<ParticleSystem>().Play();

        RandomNumber();

        GameObject p1 = Instantiate(m_empty, this.transform) as GameObject;
        GameObject p2 = Instantiate(m_empty, this.transform) as GameObject;
        GameObject p3 = Instantiate(m_empty, this.transform) as GameObject;
        GameObject g1 = Instantiate(m_vol, p1.transform) as GameObject;
        GameObject g2 = Instantiate(m_vol, p2.transform) as GameObject;
        GameObject g3 = Instantiate(m_vol, p3.transform) as GameObject;

        iTween.MoveBy(g1, iTween.Hash("y", 4f, "speed", 120/2f, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(g1, iTween.Hash("y", -4f, "speed", 120/2f, "delay", 3f, "easeType", iTween.EaseType.easeInCubic));

        iTween.MoveTo(p1, iTween.Hash("position", GameObject.Find("Panels").GetComponent<Panel>().panels[num1].transform.position, "speed", 120f, "easeType", iTween.EaseType.linear));
        
        iTween.MoveBy(g2, iTween.Hash("y", 4f, "speed", 120/2f, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(g2, iTween.Hash("y", -4f, "speed", 120/2f, "delay", 3f, "easeType", iTween.EaseType.easeInCubic));

        iTween.MoveTo(p2, iTween.Hash("position", GameObject.Find("Panels").GetComponent<Panel>().panels[num2].transform.position, "speed", 120f, "easeType", iTween.EaseType.linear));
       
        iTween.MoveBy(g3, iTween.Hash("y", 4f, "speed", 120/2f, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(g3, iTween.Hash("y", -4f, "speed", 120/2f, "delay", 3f, "easeType", iTween.EaseType.easeInCubic));

        iTween.MoveTo(p3, iTween.Hash("position", GameObject.Find("Panels").GetComponent<Panel>().panels[num3].transform.position, "speed", 120f, "easeType", iTween.EaseType.linear));

        m_as.PlayOneShot(m_acVol);

        Destroy(p);

        m_gm.m_isEvent3 = false;

       yield return new WaitForSeconds(3f);

            m_gm.m_curTurnNum++;
            StartCoroutine(m_gm.ShowUI(m_gm.m_curTurnNum));

    }

    void RandomNumber()
    {
        num1 = arrNum[Random.Range(0, arrNum.Length)];
        num2 = arrNum[Random.Range(0, arrNum.Length)];
        num3 = arrNum[Random.Range(0, arrNum.Length)];

        if (num1 == num2 || num2 == num3 || num1 == num3)
            RandomNumber();
    }
}
