using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRangeSwipe : MonoBehaviour
{
    public static XRangeSwipe instance;

    [SerializeField] private float maxHeight;
    [SerializeField] private float swipeSpeed;
    [SerializeField] private GameObject upParticle;
    [SerializeField] private GameObject downParticle;
    
    private BoxCollider collider;
    private Vector3 center,sizeVec;

    private bool startSwipe = false;
    private float size;
    private bool isUp = false;

    private void Awake()
    {
        instance = this;
        collider = GetComponent<BoxCollider>();
        center = collider.center;
        sizeVec = collider.size;
        collider.enabled = false;
    }

    private void Update()
    {
        if (startSwipe)
        {
            size += swipeSpeed * Time.deltaTime;
            if (isUp)
            {
                collider.center = center + new Vector3(0, size / 2, 0);
                collider.size = sizeVec + new Vector3(0, size, 0);
            }
            else
            {
                collider.center = center - new Vector3(0, size / 2, 0);
                collider.size = sizeVec + new Vector3(0, size, 0);
            }

            if(size >= maxHeight)
            {
                collider.enabled = false;
                startSwipe = false;
                size = 0;
                upParticle.SetActive(false);
                downParticle.SetActive(false);
            }
        }
    }


    public void Swipe(bool up)
    {
        startSwipe = true;
        isUp = up;
        collider.center = center;
        collider.size = sizeVec;
        collider.enabled = true;
        if (up)
        {
            upParticle.SetActive(true);
        }
        else
        {
            downParticle.SetActive(true);
        }
    }
}
