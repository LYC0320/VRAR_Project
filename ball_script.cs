using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_script : MonoBehaviour {

    public GameObject floor;
    private Rigidbody rb;
    private Vector3 init_pos;

	// Use this for initialization
	void Start () {
        init_pos = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        

	}


    void OnCollisionStay(Collision collision)
    {
        //Debug.Log(rb.velocity);
        if (collision.gameObject == floor && Mathf.Abs(rb.velocity.y) < 0.05)
        {
            Debug.Log("Hit floor");
            transform.position = init_pos;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
