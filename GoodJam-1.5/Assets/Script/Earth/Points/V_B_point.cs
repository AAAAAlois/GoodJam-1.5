using UnityEngine;
using System.Collections;


public class V_B_point : Basic_point
{
    public float life;
    public float generateInter;
    public GameObject Prefab { get; set; }
    /// <summary>
    /// 这是第几个生成点
    /// </summary>
    public int PointCount { get; set; }
    /// <summary>
    /// 生成点方向
    /// </summary>
    public Vector3 NextDir { get; set; }
    /// <summary>
    /// 是否逆向生成
    /// </summary>
    public bool IsReverset { get; set; }

    private bool hasGenerate = false;

    public bool DestroyImmediate { get; set; }

    protected override void Update()
    {
        base.Update();

        if (DestroyImmediate && !hasInteract)
        {
            DestroyFullPoint();
        }

        life -= Time.deltaTime;
        generateInter -= Time.deltaTime;

        if(generateInter <= 0 && !hasGenerate)
        {
            GenerateNextPoint(false);
        }

        if(life <= 0 && !hasInteract)
        {
            hasInteract = true;
            OnDestroyPoint();
        }
    }

    public override void OnClick()
    {
        
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        hasInteract = true;
        if (!hasGenerate) GenerateNextPoint(true);
        DestroyPoint();
    }

    public override void OnSlide()
    {
        base.OnSlide();
        hasInteract = true;

        if (!hasGenerate) GenerateNextPoint(false);
        PlayInteractFeedback();

        DestroyPoint();
    }

    private void GenerateNextPoint(bool imm)
    {
        if (PointCount != 7)
        {
            if (IsReverset)
            {
                Quaternion q = Quaternion.AngleAxis(8, Vector3.forward);
                NextDir = q * NextDir;
            }
            else
            {
                Quaternion q = Quaternion.AngleAxis(-8, Vector3.forward);
                NextDir = q * NextDir;
            }

            Vector3 next_pos = transform.position;
            next_pos = next_pos + NextDir;

            V_B_point createPoint = Instantiate(Prefab, next_pos, Quaternion.identity, transform.parent).GetComponent<V_B_point>();
            createPoint.CO2_dec = CO2_dec;
            createPoint.money_add = money_add;
            createPoint.money_mul = money_mul;
            createPoint.PointCount = 1;
            createPoint.NextDir = NextDir;
            createPoint.IsReverset = IsReverset;
            createPoint.m_index = 1;
            createPoint.PointCount = PointCount + 1;
            createPoint.Prefab = Prefab;
            createPoint.DestroyImmediate = imm;
        }
        hasGenerate = true;
    }

}
