using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_ponit : Basic_point
{

    public override void OnClick()
    {
        hasInteract = true;
        PlayInteractFeedback();
        DestroyPoint();
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        hasInteract = true;
        DestroyPoint();
    }

    public override void OnSlide()
    {
        if(Score_manager.instance.CheckTechUnlocked(1))
        if(Score_manager.instance.Current_tec_index>0)
        if (Score_manager.instance.CheckTechUnlocked(1)) 
        {
            base.OnSlide();
            hasInteract = true;
            PlayInteractFeedback();
            DestroyPoint();
        }
    }
}
