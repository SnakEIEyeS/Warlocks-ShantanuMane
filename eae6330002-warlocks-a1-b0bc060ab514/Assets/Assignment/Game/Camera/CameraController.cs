using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float m_PanMouseRegion = 3f;
    [SerializeField]
    private float m_PanCameraSpeed = 100f;
    [SerializeField]
    private float m_PanLimitX = 100f;
    [SerializeField]
    private float m_PanLimitZ = 100f;

    [SerializeField]
    private float m_ZoomSpeed = 2000f;
    [SerializeField]
    private float m_ZoomLimit = 30f;

    private Vector3 m_OriginalPosition;

	// Use this for initialization
	void Start () {
        m_OriginalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //CameraPan();
        //CameraZoom();
        
	}

    void CameraPan()
    {
        Vector3 UpdatedPosition = transform.position;

        if (Input.mousePosition.x <= m_PanMouseRegion)
        {
            UpdatedPosition.x -= m_PanCameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - m_PanMouseRegion)
        {
            UpdatedPosition.x += m_PanCameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= m_PanMouseRegion)
        {
            UpdatedPosition.z -= m_PanCameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - m_PanMouseRegion)
        {
            UpdatedPosition.z += m_PanCameraSpeed * Time.deltaTime;
        }

        UpdatedPosition.x = Mathf.Clamp(UpdatedPosition.x, m_OriginalPosition.x - m_PanLimitX, m_OriginalPosition.x + m_PanLimitX);
        UpdatedPosition.z = Mathf.Clamp(UpdatedPosition.z, m_OriginalPosition.z - m_PanLimitZ, m_OriginalPosition.z + m_PanLimitZ);

        transform.position = UpdatedPosition;
    }

    void CameraZoom()
    {
        Vector3 UpdatedPosition = transform.position;

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        UpdatedPosition.y -= scrollValue * m_ZoomSpeed * Time.deltaTime;
        UpdatedPosition.y = Mathf.Clamp(UpdatedPosition.y, m_OriginalPosition.y - m_ZoomLimit, m_OriginalPosition.y + m_ZoomLimit);

        transform.position = UpdatedPosition;
    }
}
