using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField]
    private LayerMask m_MapLayerMask = -1;

    private const float MAX_RAY_DIST = 10000f;
    private Vector3 HIGH_RAY = Vector3.up * MAX_RAY_DIST / 2f;

    public bool GetMapPointFromScreenPoint(Vector3 screenPos, out Vector3 mapPos, out Vector3 mapNormal)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(ray, out raycastHit, MAX_RAY_DIST, m_MapLayerMask.value);
        if (hit)
        {
            mapPos = raycastHit.point;
            mapNormal = raycastHit.normal;
        }
        else
        {
            mapPos = Vector3.zero;
            mapNormal = Vector3.up;
        }
        return hit;
    }

    public bool GetMapPointFromWorldPoint(Vector3 worldPos, out Vector3 mapPos, out Vector3 mapNormal)
    {
        Ray ray = new Ray(worldPos + HIGH_RAY, Vector3.down);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(ray, out raycastHit, MAX_RAY_DIST, m_MapLayerMask.value);
        if (hit)
        {
            mapPos = raycastHit.point;
            mapNormal = raycastHit.normal;
        }
        else
        {
            mapPos = Vector3.zero;
            mapNormal = Vector3.up;
        }
        return hit;
    }

}

	