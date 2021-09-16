using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecFirstClickPoint : MonoBehaviour
{
    private List<DecFirstClickPoint> blockers;

    public GameObject mon_panel, Up_panel;

    bool state;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            state = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (state)
                CheckGuiRaycastObjects();
        }
    }


    public bool CheckGuiRaycastObjects()
    {
        state = false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject go = list[i].gameObject;

                if (go.GetComponent<A_ponit>()!=null)
                {
                    ShowRight();
                    return false;
                }
            }
        }
        ShowRight();
        return true;
    }

    private void ShowRight()
    {
        Up_panel.SetActive(true);
        mon_panel.SetActive(true);
        this.enabled = false;
    }
}
