using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public BallCreat_test ballCreat_Test;
    public Score_manager score_Manager;
    public Animator startMenuAnim;
    public float dealayTime;
    public GameObject startCanvas;

    private void Start()
    {
        StartCoroutine(DealyStart());
    }

    IEnumerator DealyStart()
    {
        yield return new WaitForSeconds(dealayTime);
        startMenuAnim.Play("menuStartMove");
        ballCreat_Test.enabled = true;
        score_Manager.enabled = true;
        startCanvas.SetActive(false);
    }
}
