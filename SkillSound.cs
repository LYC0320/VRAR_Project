using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSound : MonoBehaviour {

    private AudioSource skillAudioSource;
    public HandShock HS;
    bool soundBool = false;

	void Start ()
    {
        skillAudioSource = GetComponent<AudioSource>();
    }
	
	
	void Update ()
    {

        if (HS.isSkill && !soundBool) 
        {
            skillAudioSource.Play();
            soundBool = true;
        }

        if (!HS.isSkill && soundBool)
        {
            skillAudioSource.Stop();
            soundBool = false;
        }

	}
}
