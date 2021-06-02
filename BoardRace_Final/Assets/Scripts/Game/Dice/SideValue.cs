using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideValue : MonoBehaviour
{
    bool m_isGround;

    public int m_sideValue; //주사위 각 면의 숫자 받음

    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Ground")
        {
            m_isGround = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Ground")
        {
            m_isGround = false;
        }
    }

    public bool OnGround()
    {
        return m_isGround;
    }
}
