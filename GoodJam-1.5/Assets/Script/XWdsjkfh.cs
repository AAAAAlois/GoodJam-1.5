using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XWdsjkfh : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("P");
        }
    }
}
