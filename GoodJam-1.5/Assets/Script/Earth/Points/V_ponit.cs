using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class V_ponit : Basic_point
{
    [SerializeField] private GameObject B_point;
    public float BPointTempDec { get; set; }
    public int BPointBaseMoney { get; set; }
    public int BPointMoneyPerLevel { get; set; }

    public Vector3 EarthCenter { get; set; }

    public float GenerateInter { get; set; }


    public override void OnClick()
    {
        hasInteract = true;
        PlayInteractFeedback();
        CreatSevenPoints(false);
    }

    public override void OnSelect()
    {
        base.OnSelect();
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        hasInteract = true;
        CreatSevenPoints(true);
    }

    private void CreatSevenPoints(bool imm)
    {
        Vector3 next_pos = transform.position;
        Vector3 dir = (EarthCenter - next_pos).normalized * GenerateInter;
        dir.z = 0;
        bool isReverse = Random.Range(0, 2) == 0;
        if (isReverse)
        {
            Quaternion q = Quaternion.AngleAxis(-32, Vector3.forward);
            dir = q * dir;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(32, Vector3.forward);
            dir = q * dir;
        }
        next_pos += dir;

        V_B_point createPoint = Instantiate(B_point, next_pos, Quaternion.identity, transform.parent).GetComponent<V_B_point>();
        createPoint.CO2_dec = BPointTempDec;
        createPoint.money_add = BPointBaseMoney;
        createPoint.money_mul = BPointMoneyPerLevel;
        createPoint.PointCount = 1;
        createPoint.NextDir = dir;
        createPoint.IsReverset = isReverse;
        createPoint.m_index = 1;
        createPoint.Prefab = B_point;
        createPoint.DestroyImmediate = imm;

        DestroyPoint();
    }
}
