using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class XTextAnim : MonoBehaviour
{
    [SerializeField] private AnimationCurve animation;

    private Text text;
    private bool startAnim = false;
    private float time;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (startAnim)
        {
            time += Time.deltaTime;
            text.text = animation.Evaluate(time).ToString("F3");
        }
    }

    [Button("Start Anim")]
    public void StartAnim()
    {
        startAnim = true;
        time = 0;
    }


}
