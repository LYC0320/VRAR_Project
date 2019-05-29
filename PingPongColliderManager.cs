using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongColliderManager : MonoBehaviour {

    private SphereCollider pingPongSphereCollider;
    public GameObject LeftHand;
    private ControllerGrabObject CGO;

	// Use this for initialization
	void Start () {

        pingPongSphereCollider = GetComponent<SphereCollider>();
        //LeftHand = GameObject.FindWithTag("LeftHand");

        CGO = LeftHand.GetComponent<ControllerGrabObject>();

    }
	
	// Update is called once per frame
	void Update () {

        if (CGO.isGrab && CGO.isRelease)
        {
            pingPongSphereCollider.radius = 1f;
        }
        else
        {
            pingPongSphereCollider.radius = 0.5f;
        }
		
	}
}
