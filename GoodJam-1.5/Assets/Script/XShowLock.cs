using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XShowLock : MonoBehaviour
{
    [SerializeField] public int techIndex;
    [SerializeField] private TextMeshProUGUI text;
    public Image Button_image;
    public Sprite enable_s, unEnable_s;
    private void Update()
    {
        if(Score_manager.instance.CheckTechUnlocked(techIndex))
        {
            text.text = "升级";
        }
        if(Score_manager.instance.Money>= Score_manager.instance.GetNextPrice(techIndex))
        {
            Button_image.sprite = enable_s;
        }
        else
        {
            Button_image.sprite = unEnable_s;
        }
    }
}
