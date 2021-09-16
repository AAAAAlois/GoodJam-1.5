using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XStartAnim : MonoBehaviour
{
    public XTextAnim textAnim;



    public void StartText()
    {
        textAnim.StartAnim();
    }

    public void StartGame()
    {
        Score_manager.startScore = true;
        BallCreat_test.StartCreate = true;
    }
}
