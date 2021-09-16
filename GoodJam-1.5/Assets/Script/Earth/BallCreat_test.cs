using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BallCreat_test : MonoBehaviour
{
    static public bool StartCreate = false;

    public struct CostData
    {
        public int unlockCost;
        public int beginnerCost;
        public int costPerUpgrade;
        public int preLevel;
    }
    static public CostData[] costData;

    [Serializable]
    public struct PointData
    {
        [LabelText("预制体")] public GameObject prefab;
        [LabelText("生成间隔")] public float createFreq;
        [LabelText("过量后生成间隔")] public float secondCreateFreq;
        [LabelText("温度减幅")] public float temperatureDecrease;
        [LabelText("初始增加金钱")] public int baseMoney;
        [LabelText("每等级增加金钱")] public int moneyPerLevel;
        [LabelText("解锁价格")] public int unlockCost;
        [LabelText("初始价格")] public int beginnerCost;
        [LabelText("每次升级增加价格")] public int costPerUpgrade;
        [LabelText("解锁下一项科技前该科技应达到等级")] public int preLevel;
    }
    [LabelText("图标列表(解锁顺序从早到晚)")] public PointData[] balls;

    [Title("Gamma图标额外生成参数")]
    [LabelText("图标生成间隔")] [SerializeField] private float generateInter = 0.25f;

    [Title("生成参数")]
    [LabelText("游戏开始时生成延时")] public float delay = 2;
    [LabelText("最大生成数量")] public int max_pointNum;
    public bool playOnAwake = true;

    private float[] _nextCreTime = new float[5];//下一个时间
    private E_point onlyEPoint;
    private float lastEDestroyedTime = 0;

    private void Awake()
    {
        costData = new CostData[5];
        for (int i = 0; i < balls.Length; i++)
        {
            CostData data;
            data.beginnerCost = balls[i].beginnerCost;
            data.costPerUpgrade = balls[i].costPerUpgrade;
            data.unlockCost = balls[i].unlockCost;
            data.preLevel = balls[i].preLevel;
            costData[i] = data;
        }
        StartCreate = playOnAwake;
    }

    private void Start()
    {
        _nextCreTime[0] = delay + balls[0].createFreq;
    }

    private void Update()
    {
        if (!StartCreate) return;

        for (int i = 0; i <= Score_manager.instance.Current_tec_index; i++) //对所有已经解锁的科技
        {
            if (i < 4)
            {
                if (XPointContainer.containedPoints > max_pointNum) //如果生成数量超过最大正常生成数量
                {
                    _nextCreTime[i] -= Time.deltaTime;
                    if (_nextCreTime[i] < 0.0f)//如果到生成时间
                    {
                        _nextCreTime[i] = balls[i].secondCreateFreq;//生成点，刷新生成速度
                        Creat_point(i);
                    }
                }
                else
                {
                    _nextCreTime[i] -= Time.deltaTime;
                    if (_nextCreTime[i] < 0.0f)//如果到生成时间
                    {
                        _nextCreTime[i] = balls[i].createFreq;
                        Creat_point(i);
                    }
                }
            }
            if(i == 4)
            {
                if (!onlyEPoint)
                {
                    float time = UnityEngine.Random.Range(balls[i].createFreq, balls[i].secondCreateFreq);
                    if(Time.time - lastEDestroyedTime > time)
                    {
                        Creat_point(i);
                    }
                }
                else
                {
                    lastEDestroyedTime = Time.time;
                }
            }
        }
    }

    private void Creat_point(int index)
    {
        if (index == 4)
        {
            Basic_point createPoint = Instantiate(balls[index].prefab, transform).GetComponent<Basic_point>();
            createPoint.CO2_dec = balls[index].temperatureDecrease;
            createPoint.money_add = balls[index].baseMoney;
            createPoint.money_mul = balls[index].moneyPerLevel;
            createPoint.m_index = index;
            createPoint.transform.position = transform.position;
            onlyEPoint = createPoint.GetComponent<E_point>();
            return;
        }


        XPointContainer container = XPointContainer.RandomGetContainer();
        if (container)//如果还有容器
        {
            //设置点的初始数值
            Basic_point createPoint = Instantiate(balls[index].prefab, transform).GetComponent<Basic_point>();
            createPoint.CO2_dec = balls[index].temperatureDecrease;
            createPoint.money_add = balls[index].baseMoney;
            createPoint.money_mul = balls[index].moneyPerLevel;
            createPoint.m_index = index;
            container.ContainPoint(createPoint);

            if(index == 2)//如果是七点生成
            {
                V_ponit gammaPoint = createPoint.GetComponent<V_ponit>();
                gammaPoint.BPointTempDec = balls[1].temperatureDecrease;
                gammaPoint.BPointBaseMoney = balls[1].baseMoney;
                gammaPoint.BPointMoneyPerLevel = balls[1].moneyPerLevel;
                gammaPoint.EarthCenter = transform.position;
                gammaPoint.GenerateInter = generateInter;
            }
        }

  
    }
}
