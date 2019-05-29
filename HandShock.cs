using UnityEngine;
using System.Collections;

public class HandShock : MonoBehaviour
{

    SteamVR_TrackedObject Hand;
    SteamVR_Controller.Device device;
    public Ball ball;
    public bool isSkill = false;

    void Start()
    {
        Hand = GetComponent<SteamVR_TrackedObject>();
    }


    void Update()
    {

        if (Hand.isValid)
        {
            Hand = GetComponent<SteamVR_TrackedObject>();
        }

        device = SteamVR_Controller.Input((int)Hand.index);

        if (ball.shock)
        {
            StartCoroutine("Shock", 0.07f);
        }

        if (device.GetHairTriggerDown()) 
        {
            if (ball.whoHit == 1)
            {
                isSkill = true;
            } 
            //Debug.Log("down");
        }
        
        if(device.GetHairTriggerUp())
        {
            //isSkill = false;
            //Debug.Log("up");
        }

    }

    IEnumerator Shock(float durationTime)
    {

        Invoke("StopShock", durationTime);

        while (ball.shock)
        {
            device.TriggerHapticPulse(5000);

            yield return new WaitForEndOfFrame();

        }

    }

    void StopShock()
    {
        ball.shock = false;
    }
}
