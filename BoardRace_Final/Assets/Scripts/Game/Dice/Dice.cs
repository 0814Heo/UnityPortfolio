using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static Dice instance;

    List<string> m_state = new List<string>();

    Animation m_diceAnim;

    Vector3 m_initPosition;

    AudioSource m_as;

    void Start()
    {
        instance = this;
        m_initPosition = new Vector3(0,0.124f,-0.243f);
        m_diceAnim = this.transform.GetComponent<Animation>();
        m_as = this.gameObject.GetComponent<AudioSource>();

        foreach (AnimationState state in m_diceAnim)
        {
            m_state.Add(state.name);
        } 
    }

    public void RollDice()
    {
        m_as.PlayOneShot(m_as.clip);
        int random = Random.Range(0, 6);
        m_diceAnim.Play(m_state[random]);
    }

    void Reset()
    {
        transform.position = m_initPosition;
    }

    void CheckValue(int value)
    {
        var gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gm.m_curTurnNum % 2 != 0)
        {
            gm.m_player.GetComponent<MoveToPanel>().m_steps = value;
            StartCoroutine(gm.m_player.GetComponent<MoveToPanel>().Move());
        }

        else
        {
            gm.m_com.GetComponent<MoveToPanel>().m_steps = value;
            StartCoroutine(gm.m_com.GetComponent<MoveToPanel>().Move());
        }

        gm.m_diceInfo.text = value.ToString() + "칸 이동!";
        Reset();
    }
}
