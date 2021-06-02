using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;

    [SerializeField]
    GameObject m_popup;
    [SerializeField]
    GameObject m_btnRetry;
    [SerializeField]
    GameObject m_btnQuit;

    GameManager m_gm;
    Vector3 m_initPosition;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        m_popup.SetActive(false);
        m_initPosition = m_btnQuit.transform.position;
        if (SceneManager.GetActiveScene().name == "1_Game")
        {
            m_gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (SceneManager.GetActiveScene().name == "0_Title")
        {
            StartCoroutine("Level");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_popup.activeSelf)
            {
                Time.timeScale = 1.0f;
                m_popup.SetActive(false);
                if(SceneManager.GetActiveScene().name == "0_Title")
                    StartCoroutine("Level");
            }

            else
            {
                Time.timeScale = 0.0f;
                m_popup.SetActive(true);
                if (SceneManager.GetActiveScene().name == "0_Title")
                {
                    m_btnRetry.SetActive(false);
                    m_btnQuit.transform.localPosition = new Vector3(0, m_btnQuit.transform.localPosition.y, m_btnQuit.transform.localPosition.z);
                    StopCoroutine("Level");
                }
                else
                {
                    m_btnRetry.SetActive(true);
                    m_btnQuit.transform.position = m_initPosition;

                }

            }
        }
    }

    public void NextScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void Retry()
    {
        m_popup.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("1_Game");
    }

    public void QuitApp()
    {
        Debug.Log("앱 종료");
        Application.Quit();
    }

    public void ClosePopup()
    {
        Time.timeScale = 1.0f;
        m_popup.SetActive(false);
        if (SceneManager.GetActiveScene().name == "0_Title")
            StartCoroutine("Level");
    }

    IEnumerator Level()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("1_Game");
    }
}
