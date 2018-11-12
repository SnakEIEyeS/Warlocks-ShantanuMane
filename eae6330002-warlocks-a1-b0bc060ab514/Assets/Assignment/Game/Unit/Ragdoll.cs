using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

    [SerializeField]
    private Transform m_AliveParent = null;
    [SerializeField]
    private Transform m_RagdollParent = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AlignRagdoll()
    {
        AlignSubTree(m_AliveParent, m_RagdollParent);
    }

    private void AlignSubTree(Transform a, Transform b)
    {
        for(int i =0; i<a.childCount; i++)
        {
            string name = a.name;
            Transform aChild = a.GetChild(i);
            Transform bChild = b.Find(name);

            if(bChild!=null)
            {
                bChild.position = aChild.position;
                bChild.rotation = bChild.rotation;

                AlignSubTree(aChild, bChild);
            }
            
        }
    }

    public Transform getAliveParent()
    { return m_AliveParent; }
}
