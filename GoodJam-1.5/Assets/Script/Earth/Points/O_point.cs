using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class O_point : Basic_point
{
    [SerializeField] private Transform circle;
    [SerializeField] private Vector2 circleSizeRange = new Vector2(0.1f, 1);
    [SerializeField] private float allowClickTime = 1.2f;
    [SerializeField] private float maxBoomRange = 1;

    private float countDown = 0;
    private SphereCollider boomRange;
    private float targetRange;
    private float explodeSpeed = 20f;
    private float radiusToScale;

    protected override void Update()
    {
        base.Update();
        if (!hasInteract)
        {
            countDown += Time.deltaTime;
            float scale = (circleSizeRange.y - circleSizeRange.x) * ((allowClickTime - countDown) / allowClickTime);
            circle.localScale = new Vector3(scale, scale, 1);
            //Debug.LogError($"count down:{scale}");
            //Debug.LogError($"count down:{scale}");

            if (countDown >= allowClickTime)
            {
                Debug.Log("Time up");
                hasInteract = true;
                circle.gameObject.SetActive(false);
                DestroyPoint();
            }
        }
    }

    public override void OnClick()
    {
        hasInteract = true;

        PlayInteractFeedback();

        //打开范围
        boomRange = circle.GetComponent<SphereCollider>();
        targetRange = maxBoomRange * (countDown / allowClickTime);//目标爆炸范围
        radiusToScale = 1 / boomRange.radius;//获得一下比例
        boomRange.radius = 0;
        circle.localScale = new Vector3(0, 0, 1);
        boomRange.GetComponent<Animator>().SetBool("explode", true);//打开爆炸

        Debug.Log($"update radius to scale:{radiusToScale}");

        StartCoroutine(DelayDis());
    }

    public override void DestroyFullPoint()
    {
        if (hasInteract) Debug.LogError("played twice");
        hasInteract = true;
        boomRange = circle.GetComponent<SphereCollider>();
        targetRange = maxBoomRange * (countDown / allowClickTime);//目标爆炸范围
        radiusToScale = 1 / boomRange.radius;//获得一下比例
        boomRange.radius = 0;
        circle.localScale = new Vector3(0, 0, 1);
        boomRange.GetComponent<Animator>().SetBool("explode", true);//打开爆炸

        StartCoroutine(DelayDis());
    }

    IEnumerator DelayDis()
    {
        while(boomRange.radius < targetRange)
        {
            boomRange.radius = boomRange.radius + explodeSpeed * Time.fixedDeltaTime;
            float scale = boomRange.radius * radiusToScale;
            circle.localScale = new Vector3(scale, scale, 1);
            Debug.Log($"explode scale {scale} = {boomRange.radius} * {radiusToScale}, radius = {boomRange.radius}");
            yield return new WaitForFixedUpdate();
        }

        boomRange.GetComponent<Animator>().SetBool("explode", false);
        yield return new WaitForSeconds(1.5f);
        DestroyPoint();
    }
}
