using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPointContainer : MonoBehaviour
{
    public static List<XPointContainer> containers = new List<XPointContainer>();
    static public int containedPoints;

    public bool IsContaining => containingPoint != null;

    private List<XPointContainer> nearbyContainers = new List<XPointContainer>();
    private Basic_point containingPoint;
    private CircleCollider2D rangeDetector;
    private int nearbyContainerCount;

    private void Awake()
    {
        rangeDetector = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.GetMask("Container");
        filter.useTriggers = true;

        Collider2D[] nearbyColliders = new Collider2D[6];
        nearbyContainerCount = rangeDetector.OverlapCollider(filter, nearbyColliders);
        for (int i = 0; i < nearbyContainerCount; i++) 
        {
            nearbyContainers.Add(nearbyColliders[i].GetComponent<XPointContainer>());
        }
    }

    private void OnEnable()
    {
        containers.Add(this);
    }

    public static XPointContainer RandomGetContainer()
    {
        if (containers.Count <= 0) return null;
        int index = Random.Range(0, containers.Count);
        return containers[index];
    }

    public XPointContainer RandomGetNearbyEmptyContainer()
    {
        List<int> indices = new List<int>();
        for(int i = 0; i < nearbyContainerCount; i++)
        {
            indices.Add(i);
        }
        while(indices.Count != 0)
        {
            int index = Random.Range(0, indices.Count);
            if (!nearbyContainers[index].IsContaining)
            {
                return nearbyContainers[index];
            }
        }
        return null;
    }

    public void ContainPoint(Basic_point point)
    {
        point.transform.position = transform.position;
        point.Container = this;
        containingPoint = point;
        containers.Remove(this);
        containedPoints++;
    }


    public void RemovePoint()
    {
        containers.Add(this);
        containedPoints--;
        containingPoint = null;
    }

    private void OnDisable()
    {
        Debug.Log("disable container");
        if(containingPoint != null)
        {
            Debug.Log("Disable Points");
            containingPoint.OnDestroyPoint();
            containedPoints--;
        }
        else
        {
            containers.Remove(this);
        }

    }
}
