using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_acSoundEffect;
    [SerializeField]
    AudioSource m_asGame;
    [SerializeField]
    AudioClip m_acGame;
    [SerializeField]
    AudioClip m_acGameWin;
    [SerializeField]
    AudioClip m_acGameOver;
    [SerializeField]
    AudioClip m_acBtn;
    void Start()
    {
        m_asGame.clip = m_acGame;
        m_asGame.Play();
        m_asGame.volume = 0.5f;
    }

    public void Win()
    {
        m_asGame.Stop();
        //m_acSoundEffect.clip = m_acGameWin;
        m_acSoundEffect.PlayOneShot(m_acGameWin);
    }

    public void Lose()
    {
        m_asGame.Stop();
        //m_acSoundEffect.clip = m_acGameOver;
        m_acSoundEffect.PlayOneShot(m_acGameOver);
    }

    public void ButtonSoundEffect()
    {
        m_acSoundEffect.PlayOneShot(m_acBtn);
    }
}
