using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omega : MonoBehaviour
{
    public float ReverTime = 2.0f,stayTime=0.2f;
    private float _currentTime = 0.0f;


    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        _currentTime = stayTime;
       // StartCoroutine(EnableOmega());
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if(_currentTime < 0)
        {
            _currentTime = ReverTime;
            sphereCollider.enabled = true;
            Debug.Log("d");
            StartCoroutine(EnableOmega());
        }
    }

    IEnumerator EnableOmega()
    {
        yield return new WaitForSeconds(stayTime);
        sphereCollider.enabled = false;
    }
}
