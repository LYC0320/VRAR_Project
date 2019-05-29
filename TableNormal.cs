using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableNormal : MonoBehaviour {

    public Vector3 normal;

    public GameObject normalPos1;
    public GameObject normalPos2;

	// Use this for initialization
	void Start () {
    

        
		
	}
	
	// Update is called once per frame
	void Update () {

        normal = (normalPos2.transform.position - normalPos1.transform.position).normalized;
        //Debug.Log(normal);

    }
}
