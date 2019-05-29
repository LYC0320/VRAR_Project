using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSelf : MonoBehaviour
{

    public GameObject ball;
    public GameObject arm;

    private Vector3 origin;
    private Vector3 waitcircle;

    private float now;

    private ball_script_new bsn;

    // Use this for initialization
    void Start()
    {
        origin = transform.position;
        waitcircle = origin;
        now = 0.0f;

        bsn = ball.GetComponent<ball_script_new>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var source = transform.position;
        var target = ball.transform.position;
        //var hit = ball.GetComponent<Ball>().hitpoint;

        var dist = Vector3.Distance(target, source);
        var x_dist = target.x - transform.position.x;
        //Debug.Log("Dist: " + dist);
        //Debug.Log("x_Dist: " + x_dist);

        now += Time.deltaTime;
        if (dist < 0.3f)
        {
            Swing();
        }
        else
        {
            Recover();
        }

        if (x_dist < 2.0f)
        {
            if (x_dist > 1.5f)
            { // z threshold
                target.z = transform.position.z;
            }
            if (bsn.end)
            {
                target = transform.position;
            }
            if (x_dist > 0.3f)
            {
                target.x = transform.position.x;
                transform.position = Vector3.MoveTowards(source, target, Time.deltaTime * 5.0f); // speed
            }
            else
            {
                transform.position = Vector3.MoveTowards(source, target/*hit*/, Time.deltaTime * 5.0f); // speed
            }
            
        }
        else
        {
            waitcircle.y = origin.y + 0.05f * Mathf.Sin(now * 5.0f);
            waitcircle.z = origin.z + 0.02f * Mathf.Cos(now * 7.0f);
            transform.position = Vector3.MoveTowards(source, waitcircle, Time.deltaTime * 3.0f);
        }

        //Debug.Log(waitcircle);
    }

    void Swing()
    {
        Quaternion goal = Quaternion.Euler(0, 30.0f, 0);
        arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, goal, Time.deltaTime * 3.0f);
    }

    void Recover()
    {
        Quaternion start = Quaternion.Euler(0, -30.0f, 0);
        arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, start, Time.deltaTime * 10.0f);
    }
}
