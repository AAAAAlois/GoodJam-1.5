using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class om_button : MonoBehaviour
{
    public Image Button_image;
    public Sprite enable_s, unEnable_s;
    private void Update()
    {
        if (Score_manager.instance.Money >= 10000)
        {
            Button_image.sprite = enable_s;
        }
        else
        {
            Button_image.sprite = unEnable_s;
        }
    }
}
