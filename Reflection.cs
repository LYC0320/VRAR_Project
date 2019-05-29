using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{

    public GameObject reflectionRb, reflectionLt;
    float x, y, z;
    private Vector3 randomPosition;


    void Start()
    {

        x = reflectionRb.transform.position.x - reflectionLt.transform.position.x;
        z = reflectionRb.transform.position.z - reflectionLt.transform.position.z;
    }

    void Update()
    {

    }

    public Vector3 getRandomPoint()
    {

        randomPosition = new Vector3(
            Random.Range(reflectionLt.transform.position.x, reflectionLt.transform.position.x + x)
            , reflectionLt.transform.position.y
            , Random.Range(reflectionLt.transform.position.z, reflectionLt.transform.position.z + z));

        return randomPosition;

    }
}
