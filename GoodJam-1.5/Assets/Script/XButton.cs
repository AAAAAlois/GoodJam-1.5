using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class XButton : MonoBehaviour
{
    [SerializeField] private string defaultText;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private UnityEvent onClickEvent;

    private bool isSelected = false;
    private Animator animator;

    public string Info
    {
        get => text.text;
        set
        {
            text.text = value;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Info = defaultText;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                onClickEvent?.Invoke();
            }
        }
    }

    public void OnSelectButton()
    {
        isSelected = true;
        animator.SetBool("isSelect", true);
    }

    public void OnLeaveButton()
    {
        isSelected = false;
        animator.SetBool("isSelect", false);
    }
}
