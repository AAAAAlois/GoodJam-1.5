using UnityEngine;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

public class XMouse : MonoBehaviour
{
    [Title("相机参数")]
    [SerializeField] private Camera usingCamera;
    [SerializeField] private AnimationCurve mouseToCameraPosCurve;
    [SerializeField] private float cameraMoveRadius = 0.75f;
    
    [Title("选择层")]
    [SerializeField] private LayerMask layerToSelect;

    private RectTransform rectTransform;
    private Vector2 cameraCenter;
    private Vector3 cameraTargetPos;

    private Basic_point selectingPoint;
    private XButton selectingButton;

    public bool EnableVisual { get; set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cameraCenter = usingCamera.transform.position;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        //Update Mouse Pos
        Vector2 mousePos = Input.mousePosition;
        Vector2 viewportPos = usingCamera.ScreenToViewportPoint(mousePos);
        rectTransform.anchorMin = rectTransform.anchorMax = viewportPos;
        rectTransform.anchoredPosition = Vector2.zero;

        //Update Camera Pos
        Vector2 relativeDir = -(viewportPos - new Vector2(0.5f, 0.5f)).normalized;
        Vector2 cameraPos = mouseToCameraPosCurve.Evaluate(Vector2.Distance(viewportPos, new Vector2(0.5f, 0.5f))) * relativeDir * cameraMoveRadius + cameraCenter;
        cameraTargetPos = cameraPos;
        cameraTargetPos.z = usingCamera.transform.position.z;
        usingCamera.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTargetPos, 0.1f);
        usingCamera.transform.LookAt(Vector3.zero);

        //Update Detect Pos
        Ray ray = usingCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerToSelect)) 
        {
            if (hit.collider.CompareTag("point"))
            {
               // Debug.Log("hit point:" + hit.collider.name);
                if (selectingButton) selectingButton.OnLeaveButton();
                else
                {
                    Basic_point hitPoint = hit.collider.GetComponent<Basic_point>();
                    if (hitPoint != selectingPoint) 
                    {
                        if(selectingPoint) selectingPoint.OnLeave();
                        selectingPoint = hitPoint;
                        selectingPoint.OnSelect();
                    }
                }

            }
            else if (hit.collider.CompareTag("button"))
            {
                if (selectingPoint) selectingPoint.OnLeave();
                else
                {
                    XButton hitButton = hit.collider.GetComponent<XButton>();
                    if(hitButton != selectingButton)
                    {
                        if (selectingButton) selectingButton.OnLeaveButton();
                        selectingButton = hitButton;
                        selectingButton.OnSelectButton();
                    }
                }
            }
        }
        else
        {
            if (selectingPoint) selectingPoint.OnLeave();
            if (selectingButton) selectingButton.OnLeaveButton();
            selectingPoint = null;
            selectingButton = null;
        }
    }
}
