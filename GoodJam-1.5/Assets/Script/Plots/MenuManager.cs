using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MenuManager : SerializedMonoBehaviour
{
    public GameObject loadOut;
    public GameObject hint_text;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DealyLoadScence(1, 1.6f));
        }
    }


    IEnumerator DealyLoadScence(int sceneIndex, float dealyTime)
    {
        loadOut.SetActive(true);
        yield return new WaitForSeconds(dealyTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
