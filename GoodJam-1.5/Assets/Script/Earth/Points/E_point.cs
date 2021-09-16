using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class E_point : Basic_point
{
    public override void OnClick()
    {
        
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        XRangeSwipe.instance.Swipe(Input.GetAxis("Mouse ScrollWheel") > 0);
        hasInteract = true;
        DestroyPoint();
    }


    public override void OnScroll()
    {
        base.OnScroll();
        XRangeSwipe.instance.Swipe(Input.GetAxis("Mouse ScrollWheel") > 0);
        PlayInteractFeedback();
        hasInteract = true;
        DestroyPoint();
    }

}
