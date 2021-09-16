using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastText : MonoBehaviour
{

    public int index;
    public Text _text;



    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = Score_manager.instance.taskComplete[index].ToString();
    }
}
