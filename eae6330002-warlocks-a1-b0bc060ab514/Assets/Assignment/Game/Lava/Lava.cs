using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        //print("Lava says " + other.gameObject.name);
        Health otherHealth = other.gameObject.GetComponentInParent<Health>();
        if (otherHealth != null)
        {
            //print("Calling HealthTest");
            otherHealth.TakeDamage(10 * Time.fixedDeltaTime);
        }
    }
}
