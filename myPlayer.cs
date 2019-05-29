using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlayer : MonoBehaviour {
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

        //transform.Translate(new Vector3(0, 0, 5 * Time.deltaTime));
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("player move");
            rb.AddForce(new Vector3(0, 0, 200));
        }

	}

    void OnTriggerStay(Collider other)
    {
       
        Debug.Log("HIT!!!!!");

    }
}
