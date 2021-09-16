using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

public abstract class Basic_point : MonoBehaviour
{
    [Title("参数")]
    [LabelText("温度降幅")] public float CO2_dec;
    [LabelText("金币增加")] public int money_add;
    [LabelText("金币升级增加系数")] public int money_mul;

    [Title("视觉表现")]
    [LabelText("彩色配色")] [SerializeField] private Color originColor;
    [LabelText("黑白配色")] [SerializeField] private Color grayColor;
    [LabelText("删除表现")] [SerializeField] private MMFeedbacks destroyFeedback;
    [LabelText("交互表现")] [SerializeField] private MMFeedbacks interactFeedback;

    [HideInInspector] public bool isCanClick = false;
    [HideInInspector] public int m_index;
    [HideInInspector] public int cre_father_index, cre_son_index;

    public XPointContainer Container { get; set; }

    private Animator animator;
    private Color init_color;
    protected bool hasInteract = false;//如果被点击
    private bool isIn = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        init_color = GetComponent<Image>().color;
    }



    private void Start()
    {
        destroyFeedback?.Initialization();
        if(Score_manager.instance.CheckTechUnlocked(m_index))
        {
            Debug.Log("Point not unlocked");
            GetComponent<XWhiteMask>().originColor = originColor;
            isCanClick = true;

        }
        else
        {
            GetComponent<XWhiteMask>().originColor = grayColor;
            isCanClick = false;
        }
        animator.SetBool("IsActivated", isCanClick);
    }

    protected virtual void Update()
    {
        if (!isCanClick)
        {
            if (Score_manager.instance.CheckTechUnlocked(m_index)) 
            {
                isCanClick = true;
                GetComponent<XWhiteMask>().originColor = originColor;
                animator.SetBool("IsActivated", isCanClick);
            }
        }

        if (isCanClick && isIn && !hasInteract) 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnClick();
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                OnSlide();
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                OnScroll();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                OnScroll();
            }
        }

    }

    /// <summary>
    /// 选择时动画反馈
    /// </summary>
    public virtual void OnSelect()
    {
        animator.SetBool("Select", true);
        isIn = true;   
    }

    /// <summary>
    /// 点击
    /// </summary>
    public abstract void OnClick();

    /// <summary>
    /// 滚动
    /// </summary>
    public virtual void OnScroll()
    {

    }

    /// <summary>
    /// 滑动
    /// </summary>
    public virtual void OnSlide()
    {

    }

    /// <summary>
    /// 离开时动画反馈
    /// </summary>
    public virtual void OnLeave()
    {
        isIn = false;
        if (!hasInteract)//只有还没被点的时候可以点
            animator.SetBool("Select", false);
    }

    /// <summary>
    /// 删除时动画反馈
    /// </summary>
    public virtual void OnDestroyPoint()
    {
        animator.SetTrigger("onDestroy");
    }


    public void PlayDestroyFeedback()
    {
        destroyFeedback?.PlayFeedbacks();
    }

    public void PlayInteractFeedback()
    {
        interactFeedback?.PlayFeedbacks();
    }

    /// <summary>
    /// 删除点
    /// </summary>
    public virtual void DestroyPoint()
    {
        if(!GetComponent<V_B_point>()) Score_manager.instance.taskComplete[m_index]++;
        if (!Score_manager.instance.isRightActive)
        {
            Score_manager.instance.isRightActive = true;
            Score_manager.instance.ShowRight();
        }
        Score_manager.instance.CO2 -= CO2_dec;
        Score_manager.instance.Money += money_add + money_mul * Score_manager.instance.TechLevels[m_index];

        Score_manager.instance.RefreshValue();

        if(Container) Container.RemovePoint();
        OnDestroyPoint();
    }

    //删除动画Event
    public void DestroyPointEvent()
    {
        Destroy(gameObject);
    }

    public virtual void DestroyFullPoint()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destroy Intrigger");
        if (other.CompareTag("boom") && !hasInteract) 
        {
            DestroyFullPoint();
        }
    }
    
}
