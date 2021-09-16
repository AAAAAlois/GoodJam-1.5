using TMPro;
using UnityEngine;

public class XCostView : MonoBehaviour
{
    [SerializeField] private int techIndex;
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        text.text = Score_manager.instance.GetNextPrice(techIndex).ToString();
    }
}
