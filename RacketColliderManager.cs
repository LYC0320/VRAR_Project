using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketColliderManager : MonoBehaviour {

    // Use this for initialization
    public GameObject racket, racketBackHand;
    public ControllerGrabObject isStart;

	void Start ()
    {
        racket.GetComponent<BoxCollider>().enabled = false;
        racketBackHand.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isStart.isGrab && isStart.isRelease)
        {
            racket.GetComponent<BoxCollider>().enabled = true;
            racketBackHand.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            racket.GetComponent<BoxCollider>().enabled = false;
            racketBackHand.GetComponent<BoxCollider>().enabled = false;
        }
	}
}
