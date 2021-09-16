using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    protected AudioSource m_bgm;
    public AudioClip mainBgm;

    private void Start()
    {
        m_bgm = FindObjectOfType<GlobalMusic>().GetComponent<AudioSource>();

        if (m_bgm.clip != mainBgm)
        {
            m_bgm.Stop();
            m_bgm.clip = mainBgm;

            StartCoroutine(DelayPlayerMusic(1.0f));
        }

    }

    public void SetMusicVolume(float Num)
    {
        m_bgm.volume = Num;
    }

    IEnumerator DelayPlayerMusic(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        m_bgm.Stop();
        m_bgm.Play();
    }

    public void SwitchMusic(AudioClip audioClip)
    {
        m_bgm.Stop();
        m_bgm.clip = audioClip;

        StartCoroutine(DelayPlayerMusic(1.0f));
    }
}
