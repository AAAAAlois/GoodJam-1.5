using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_point : Basic_point
{
    //todo:连点
    public override void OnClick()
    {
        
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        hasInteract = true;
        DestroyPoint();
    }

    public override void OnSlide()
    {
        base.OnSlide();
        hasInteract = true;
        PlayInteractFeedback();
        DestroyPoint();
    }
}
