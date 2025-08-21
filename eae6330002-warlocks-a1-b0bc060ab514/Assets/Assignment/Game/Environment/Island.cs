using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour {

    //[SerializeField]
    //private GameObject m_CylinderChild = null;

    private Vector3 OriginalScale = new Vector3(1f, 1f, 1f);

    [SerializeField]
    private Vector3 ScaleReductionFactor = new Vector3(0f, 0f, 0f);

    [SerializeField]
    private Vector3 MinimumScale = new Vector3(1f, 1f, 1f);

    void Start()
    {
        OriginalScale = transform.localScale;
    }

    public void Shrink()
    {
        //print("ShrinkIsland called");
        if(transform.localScale.x > MinimumScale.x 
            && transform.localScale.y >= MinimumScale.y 
            && transform.localScale.z > MinimumScale.z)
        {
            transform.localScale -= ScaleReductionFactor;
            //transform.localScale.Scale(ScaleReductionFactor);
        }
    }

    public void ResetIslandSize()
    {
        transform.localScale = OriginalScale;
    }

    
}
