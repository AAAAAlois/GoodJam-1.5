using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class Score_manager : MonoBehaviour
{
    static public Score_manager instance;
    static public bool startScore = false;

    [Title("默认值")]
    [SerializeField] private int defualtMoney;
    [SerializeField] private float defaultTemperature;
    [SerializeField] private bool playOnAwake = true;

    [Title("温度增长参数")]
    [LabelText("初速度")][SerializeField] private float[] _inc;
    [LabelText("速度改变的温度临界值")] [SerializeField] private float[] _tem_level;
    // [LabelText("加速度")] [SerializeField] private float _a;
    [LabelText("显示刷新频率")] [SerializeField] private float tempUpdateFreq = 0.2f;
    [LabelText("刷新启动延时")] [SerializeField] private float tempUpdateDelay = 2f;
    [LabelText("欧米茄价钱")] [SerializeField] private int omegePrice = 10000;
    [LabelText("触发omega后等待时间")] [SerializeField] private float LastWaitTime = 5f;
    public GameObject omega;
    [Title("显示")]
    public TextMeshProUGUI Money_text, CO2_text;//文字
    public GameObject mon_panel, Up_panel;//钱和升级
    public GameObject[] upg_panels;//升级

    public bool isRightActive = false;//整个Panel是否打开
    [HideInInspector] public int Current_tec_index = 0;//当前升级科技的index

    [Title("海平面")]
    [LabelText("海平面贴图（由好变坏）")] public Sprite[] earth_sprits;
    [LabelText("关闭结点")] public Transform[] pointsToSubmerge;
    [LabelText("放海平面贴图")] public Image earth_image;
    [Title("海平面上升数值")]
    public float rise1_value, rise2_value, rise3_value;

    [Title("球体生成")]
    public BallCreat_test ballCreat_Test;

    public delegate void OnUpgradeTechnologyEvent(int index, int level);

    private bool ISLast = false;
    public CanvasGroup temTextCG;
    public GameObject NUM1_5;
    public GameObject lastCanvas;

    /// <summary>
    /// 总计消除球数目
    /// </summary>
    public float totalDisPointNUm { get; set; }
    /// <summary>
    /// 温度
    /// </summary>
    public float CO2 { get; set; }
    /// <summary>
    /// 钱
    /// </summary>
    public int Money {
        get => _money;
        set
        {
            int incrementedMoney = value - _money;
            if (incrementedMoney > 0) TotalGainedMoney += incrementedMoney;
            _money = value;
        }
    }
    private int _money;
    /// <summary>
    /// 总获得的钱数（包含已消费）
    /// </summary>
    public int TotalGainedMoney { get; private set; }
    public OnUpgradeTechnologyEvent OnUpgradeTech { get; set; }

    private float _currentTime;//当前时间
    private float index_mul = 0;
    /// <summary>
    /// 五个科技的等级
    /// </summary>
    [HideInInspector] public int[] TechLevels = new int[5];

    public int[] taskComplete = new int[5];


    private int seaLevelState = 0;

    private void Awake()
    {
        instance = this;
        totalDisPointNUm = 0;
        startScore = playOnAwake;
        ISLast = false;
    }

    private void Start()
    {
        //初始化参数
        Money = defualtMoney;
        CO2 = defaultTemperature;
        _currentTime = 0;
        for(int i = 0; i < TechLevels.Length; i++)
        {
            TechLevels[i] = -1;
        }
        TechLevels[0] = 0; 
    }

    private void Update()
    {
        if (!startScore)
        {
            return;
        }

        tempUpdateDelay -= Time.deltaTime;
        if(ISLast)
        {
            CO2 = 1.5f;
            RefreshValue();
        }   
        else
        {
            if (tempUpdateDelay <= 0)//增加温度
            {
                if (CO2 < _tem_level[0])
                {
                    CO2 = 1.51f;
                }
                //CO2 += (_inc + _a * index_mul) * Time.deltaTime;
                if (CO2 < _tem_level[1])
                {
                    CO2 += (_inc[0]) * Time.deltaTime;
                }
                else if (CO2 >= _tem_level[1] && CO2 < _tem_level[2])
                {
                    CO2 += (_inc[1]) * Time.deltaTime;
                }
                else if (CO2 >= _tem_level[2])
                {
                    CO2 += (_inc[2]) * Time.deltaTime;
                }

                _currentTime += Time.deltaTime;
                index_mul += Time.deltaTime;
                if (_currentTime > tempUpdateFreq)
                {
                    _currentTime = 0;
                    RefreshValue();
                }
            }
        }
       

        //更新海平面
        if(seaLevelState == 0 && CO2 > rise1_value)
        {
            earth_image.sprite = earth_sprits[1];
            pointsToSubmerge[seaLevelState++].gameObject.SetActive(false);
        }
        else if(seaLevelState == 1 && CO2 > rise2_value)
        {
            earth_image.sprite = earth_sprits[2];
            pointsToSubmerge[seaLevelState++].gameObject.SetActive(false);
        }
        else if(seaLevelState == 2 && CO2 > rise3_value)
        {
            earth_image.sprite = earth_sprits[3];
            pointsToSubmerge[seaLevelState++].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 更新显示
    /// </summary>
    public void RefreshValue()
    {
        Money_text.text = Money.ToString();
        CO2_text.text = CO2.ToString("F3");
    }

    /// <summary>
    /// 显示Panel
    /// </summary>
    public void ShowRight()
    {
        Up_panel.SetActive(true);
        mon_panel.SetActive(true);
    }

    /// <summary>
    /// 升级其他技术
    /// </summary>
    /// <param name="index"></param>
    public void UpgradeTech(int index)
    {
        int price = GetNextPrice(index);
        if (Money >= price) //如果买得起
        {
            TechLevels[index]++;//升级技术
            Money -= price;
            if (Current_tec_index == index && TechLevels[index] >= BallCreat_test.costData[index].preLevel)//如果该技术第一次升级，解锁下一个技术
            {
                Current_tec_index++;//下一项科技可以使用
                if (index < 5)
                    upg_panels[index].SetActive(true);
            }
            OnUpgradeTech?.Invoke(index, TechLevels[index]);
            RefreshValue();
        }
    }

    public bool CheckTechUnlocked(int index) => TechLevels[index] >= 0;

    /// <summary>
    /// 解锁欧米茄
    /// </summary>
    public void GetOmege()
    {
        if(Money>=omegePrice)
        {
            Money -= omegePrice;
            omega.SetActive(true);
            StartCoroutine(LastPlot());
        }
    }

    IEnumerator LastPlot()
    {
        ISLast = true;
        NUM1_5.SetActive(true);
        temTextCG.alpha = 0.0f;
        yield return new WaitForSeconds(LastWaitTime);
        lastCanvas.SetActive(true);
    }

    public int GetNextPrice(int index) => TechLevels[index] == -1 ? BallCreat_test.costData[index].unlockCost : BallCreat_test.costData[index].beginnerCost + TechLevels[index] * BallCreat_test.costData[index].costPerUpgrade;
}
