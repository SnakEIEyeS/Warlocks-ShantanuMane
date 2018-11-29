using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    [SerializeField]
    private Camera m_MainCamera = null;

    [SerializeField]
    private Camera m_MinimapCamera = null;

    [SerializeField]
    private RawImage m_MinimapImage = null;

    [SerializeField]
    private LayerMask m_RayCastLayers = -1;
    //[SerializeField]
    private float maxRaycastDistance = 200f;

    // Use this for initialization
    void Start () {
        maxRaycastDistance += Mathf.Abs(m_MinimapCamera.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ScrollCamera(Vector3 viewportPosition)
    {
        Ray minimapToWorldRay = m_MinimapCamera.ViewportPointToRay(viewportPosition);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(minimapToWorldRay, out raycastHit, maxRaycastDistance, m_RayCastLayers.value);
        if (hit)
        {
            Vector3 cameraTarget = raycastHit.point;
            Ray mainCameraToWorldRay = new
            Ray(m_MainCamera.transform.position, m_MainCamera.transform.forward);
            hit = Physics.Raycast(mainCameraToWorldRay, out raycastHit, maxRaycastDistance, m_RayCastLayers.value);
            if (hit)
            {
                Vector3 mainCameraCurrentTarget = raycastHit.point;
                Vector3 delta = cameraTarget - mainCameraCurrentTarget;
                delta.y = 0;    // only tranlsate in the x-y plane
                m_MainCamera.transform.position += delta;
            }
        }
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        UpdatePointEvent(pointerEventData);
    }

    public void OnPointerDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        UpdatePointEvent(pointerEventData);
    }

    private void UpdatePointEvent(PointerEventData pointerEventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_MinimapImage.rectTransform, pointerEventData.position, null, out localPoint);
        Vector2 normalized = Rect.PointToNormalized(m_MinimapImage.rectTransform.rect, localPoint);
        Vector3 viewportPoint = new Vector3(normalized.x, normalized.y, 0);
        ScrollCamera(viewportPoint);
    }
}
